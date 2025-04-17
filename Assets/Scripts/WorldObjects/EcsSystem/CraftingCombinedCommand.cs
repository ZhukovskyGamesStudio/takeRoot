using System;
using System.Collections.Generic;
using Settlers.Crafting;
using UnityEngine;

public class CraftingCombinedCommand : CombinedCommandData {
    private List<CommandData> _activeCraftingCommands = new List<CommandData>(2);

    private List<CommandData> _activeGatherCommands = new List<CommandData>();

    private float _craftingPoints;
    private CraftingStationable _craftingStation;
    private Interactable _interactable;
    public List<Settler> _performingSettlers = new List<Settler>(2);

    public Dictionary<ResourceType, int> _reservedResourceAmount = new Dictionary<ResourceType, int>();

    public CraftingCombinedCommand(CraftingStationable craftingStation) {
        _craftingStation = craftingStation;
        _interactable = _craftingStation.GetComponent<Interactable>();
        _interactable.OnCommandPerformed += OnCommandPerformed;
        foreach (var type in (ResourceType[])Enum.GetValues(typeof(ResourceType))) {
            if (type == ResourceType.None) continue;
            _reservedResourceAmount.Add(type, 0);
        }
    }

    public override void UpdateLogic() {
        base.UpdateLogic();

        FormGatherCommands();
        FormCraftingCommands();
    }

    public void CancelCraftingCommandsRightAway() {
        CancelCraftingCommands();
    }

    public void RemoveReservation(ResourceData resource) {
        _reservedResourceAmount[resource.ResourceType] -= resource.Amount;
    }

    private void FormGatherCommands() {
        foreach (KeyValuePair<ResourceType, int> resourceData in _craftingStation.RequiredResourcesForAllCurrentCrafts) {
            var requiredResourceAmount = LeftToBring(resourceData.Key);
            if (requiredResourceAmount <= 0)
                continue;
            List<ResourceView> fitResourcesOnGround = ResourceManager.FindFitResourcesOnGround(resourceData.Key);
            foreach (ResourceView resource in fitResourcesOnGround) {
                if (requiredResourceAmount == 0)
                    break;
                resource.AmountToGather = Mathf.Min(resource.Amount, requiredResourceAmount);
                _reservedResourceAmount[resourceData.Key] += resource.AmountToGather;
                requiredResourceAmount = LeftToBring(resourceData.Key);
                CommandData command = new CommandData {
                    Interactable = resource.GetEcsComponent<Interactable>(),
                    Additional = _interactable,
                    CommandType = Command.GatherResourcesForCraft,
                };
                command.Interactable.AssignCommand(command);
                command.TriggerCancel += delegate { CancelGatherCommand(command); };
                _activeGatherCommands.Add(command);
                Core.CommandsManagersHolder.CommandsManager.AddCommandManually(command);
            }

            int requiredResourcesAmount = LeftToBring(resourceData.Key);
            if (requiredResourceAmount == 0)
                continue;
            List<Storagable> fitStorages = ResourceManager.FindFitStorages(resourceData.Key);
            foreach (Storagable storage in fitStorages) {
                if (requiredResourceAmount == 0)
                    break;
                storage.AmountToGather = Mathf.Min(storage.Resource.Amount, requiredResourceAmount);
                _reservedResourceAmount[resourceData.Key] += storage.AmountToGather;
                requiredResourceAmount = LeftToBring(resourceData.Key);
                CommandData command = new CommandData {
                    Interactable = storage.GetComponent<Interactable>(),
                    Additional = _interactable,
                    CommandType = Command.GatherResourcesForCraft,
                };
                command.Interactable.AssignCommand(command);
                command.TriggerCancel += delegate { CancelGatherCommand(command); };
                _activeGatherCommands.Add(command);
                Core.CommandsManagersHolder.CommandsManager.AddCommandManually(command);
            }
        }
    }

    private int LeftToBring(ResourceType type) {
        return _craftingStation.RequiredResourcesForAllCurrentCrafts[type] - _craftingStation.GetResourceAmountFromStorage(type) -
               _reservedResourceAmount[type];
    }

    private void CancelGatherCommand(CommandData command) {
        _activeGatherCommands.Remove(command);
        if (command.Interactable.TryGetComponent(out ResourceView resource))
            _reservedResourceAmount[resource.ResourceType] -= resource.AmountToGather;
        if (command.Interactable.TryGetComponent(out Storagable storage))
            _reservedResourceAmount[storage.Resource.ResourceType] -= storage.AmountToGather;
    }

    private void FormCraftingCommands() {
        if (_craftingStation.CurrentRecipeToCraft != null)
            return;
        if (_craftingStation.RecipesToCraftList.Count == 0)
            return;
        foreach (string recipe in _craftingStation.RecipesToCraftList) {
            bool canCraft = true;
            var config = Core.CraftingManager.GetRecipe(recipe);
            foreach (ResourceData resource in config.RequiredResources) {
                if (_craftingStation.GetResourceAmountFromStorage(resource.ResourceType) < resource.Amount) {
                    canCraft = false;
                    break;
                }
            }

            if (canCraft) {
                PrepareToCraftCommand(config);
                break;
            }
        }
    }

    private void PrepareToCraftCommand(CraftingRecipeConfig recipe) {
        foreach (Vector2Int cell in _craftingStation.Interactable.Gridable.GetOccupiedPositions())
        {
            if (!AStarPathfinding.IsWalkable(new Vector2Int(cell.x, cell.y - 1)))
                return;
        }
        _craftingStation.SetCurrentCraft(recipe);
        _craftingPoints = recipe.CraftingPoints;
        foreach (Race race in (Race[])Enum.GetValues(typeof(Race))) {
            if (race is Race.Both or Race.None)
                continue;
            CommandData command = new CommandData {
                Interactable = _interactable,
                CommandType = Command.PrepareToCraft,
                AdditionalData = new CraftingCommandData() {
                    CraftingStation = _craftingStation
                }
            };
            Core.CommandsManagersHolder.GetCommandManagerByRace(race).AddCommandManually(command);
        }
    }

    private void AddCraftingCommand() {
        foreach (Settler settler in _performingSettlers) {
            CommandData command = new CommandData {
                Interactable = _interactable,
                CommandType = Command.Craft,
                AdditionalData = new CraftingCommandData() {
                    CraftingStation = _craftingStation
                },
                Settler = settler
            };
            _activeCraftingCommands.Add(command);
            Core.CommandsManagersHolder.GetCommandManagerByRace(settler.SettlerData.Race).AddSubsequentCommand(command);
        }
    }

    private void CancelCraftingCommands() {
        foreach (CommandData craftingCommand in _activeCraftingCommands) {
            craftingCommand.TriggerCancel?.Invoke();
        }

        _performingSettlers.Clear();
        _activeCraftingCommands.Clear();
    }

    public void OnCommandPerformed(CommandData cData) {
        if (cData.CommandType == Command.PrepareToCraft) {
            if (!_performingSettlers.Contains(cData.Settler))
            {
                _performingSettlers.Add(cData.Settler);
                var transform = cData.Settler.transform;
                var scaleX = cData.Settler.SettlerData.Race == Race.Plants ? 1 : -1; 
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * scaleX, transform.localScale.y, transform.localScale.z);
            }

            if (_performingSettlers.Count < 2)
                return;

            if (_performingSettlers.Count == 2)
                AddCraftingCommand();
        }

        if (cData.CommandType == Command.Craft) {
            if (_performingSettlers.Count < 2)
                return;

            _craftingPoints -= 0.5f;
            if (_craftingPoints <= 0) {
                _craftingStation.CraftItem();
                CancelCraftingCommands();
            }
        }
    }
}