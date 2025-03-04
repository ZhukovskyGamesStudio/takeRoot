using System.Collections.Generic;
using System.Linq;
using Settlers.Crafting;
using UnityEngine;

public class CraftingCombinedCommand : CombinedCommandData {
    private List<CommandData> _activeGatherCommands;

    private List<ResourceData> _currentResources;
    private string _receiptId;

    private Dictionary<ResourceType, int> _reservedResourceAmount = new Dictionary<ResourceType, int>();
    private CraftingReceiptConfig ReceiptConfig => Core.CraftingManager.GetReceipt(_receiptId);

    public override void UpdateLogic() {
        base.UpdateLogic();

        FormGatherCommands();
    }

    private void FormGatherCommands() {
        foreach (ResourceData requiredResource in ReceiptConfig.RequiredResources) {
            var requiredResourceAmount = LeftToBring(requiredResource.ResourceType);
            if (requiredResourceAmount == 0)
                continue;
            List<ResourceView> fitResourcesOnGround = ResourceManager.FindFitResourcesOnGround(requiredResource.ResourceType);
            foreach (ResourceView resource in fitResourcesOnGround) {
                if (requiredResourceAmount == 0)
                    break;
                resource.AmountToGather = Mathf.Min(resource.Amount, requiredResourceAmount);
                _reservedResourceAmount[requiredResource.ResourceType] += resource.AmountToGather;
                requiredResourceAmount = LeftToBring(requiredResource.ResourceType);
                CommandData command = new CommandData {
                    Interactable = resource.GetEcsComponent<Interactable>(),
                    Additional = MainData.Interactable,
                    CommandType = Command.GatherResources,
                };
                command.Interactable.AssignCommand(command);
                command.TriggerCancel += delegate { CancelGatherCommand(command); };
                _activeGatherCommands.Add(command);
                CommandsManagersHolder.Instance.CommandsManager.AddCommandManually(command);
            }

            int requiredResourcesAmount = LeftToBring(requiredResource.ResourceType);
            if (requiredResourceAmount == 0)
                continue;
            List<Storagable> fitStorages = ResourceManager.FindFitStorages(requiredResource.ResourceType);
            foreach (Storagable storage in fitStorages) {
                if (requiredResourceAmount == 0)
                    break;
                storage.AmountToGather = Mathf.Min(storage.Resource.Amount, requiredResourceAmount);
                _reservedResourceAmount[requiredResource.ResourceType] += storage.AmountToGather;
                requiredResourceAmount = LeftToBring(requiredResource.ResourceType);
                CommandData command = new CommandData {
                    Interactable = storage.GetComponent<Interactable>(),
                    Additional = MainData.Interactable,
                    CommandType = Command.GatherResources,
                };
                command.Interactable.AssignCommand(command);
                command.TriggerCancel += delegate { CancelGatherCommand(command); };
                _activeGatherCommands.Add(command);
                CommandsManagersHolder.Instance.CommandsManager.AddCommandManually(command);
            }
        }
    }

    private bool EnoughResource(ResourceType type) {
        return ReceiptConfig.RequiredResources.Find(r => r.ResourceType == type).Amount ==
               _currentResources.Find(r => r.ResourceType == type).Amount;
    }

    private int LeftToBring(ResourceType type) {
        return ReceiptConfig.RequiredResources.Find(r => r.ResourceType == type).Amount -
               _currentResources.Find(r => r.ResourceType == type).Amount - _reservedResourceAmount[type];
    }

    public bool ResourcesRequirementReached(ResourceType type) {
        return ReceiptConfig.RequiredResources.Find(r => r.ResourceType == type).Amount ==
               _currentResources.Find(r => r.ResourceType == type).Amount;
    }

    private void CancelGatherCommand(CommandData command) {
        _activeGatherCommands.Remove(command);
        if (MainData.Interactable.TryGetComponent(out ResourceView resource))
            _reservedResourceAmount[resource.ResourceType] -= resource.AmountToGather;
        if (MainData.Interactable.TryGetComponent(out Storagable storage))
            _reservedResourceAmount[storage.Resource.ResourceType] -= resource.AmountToGather;
    }

    public void OnCommandPerformed(CommandData obj) {
        if (obj.CommandType == Command.GatherResources) {
            /*
            var resource = new ResourceData()
            {
                ResourceType = Resource.ResourceType,
                Amount = AmountToGather
            };
            var position = _interactable.CommandToExecute.Settler.GetCellOnGrid;
            var resourceToGather = ResourceManager.SpawnResourceAt(resource, position);
            resourceToGather.IsBeingCarried = true;
            resourceToGather.Interactable.CanSelect = false;
            AmountToGather = 0;
            Resource.Amount -= resource.Amount;
            CommandData command = new CommandData()
            {
                Interactable = resourceToGather.Interactable,
                Additional = _interactable.CommandToExecute.Additional,
                CommandType = Command.Delivery,
                Settler = _interactable.CommandToExecute.Settler
            };
            _interactable.CommandToExecute.TriggerCancel?.Invoke();
            _interactable.CancelCommand();
            resourceToGather.Interactable.AssignCommand(command);
            resourceToGather.GetEcsComponent<Networkable>().ChangeParent(resourceToGather.Interactable.CommandToExecute.Settler.ResourceHolder);
            CommandsManagersHolder.Instance.CommandsManager.AddSubsequentCommand(resourceToGather.Interactable.CommandToExecute);
            */
        }
    }
}