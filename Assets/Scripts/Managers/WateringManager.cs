using System;
using System.Collections.Generic;
using UnityEngine;

public class WateringManager : MonoBehaviour
{
    public float WateringThreshold;
    
    public float DryRate;
    public float WaterAmount;
    
    
    private List<CommandData> _activeWateringCommands = new List<CommandData>(20);
    private List<CanBeWatered> _canBeWateredObjects = new List<CanBeWatered>(20);

    private void Awake()
    {
        Core.WateringManager = this;
    }

    private void Update()
    {
        var time = Time.deltaTime;
        foreach (CanBeWatered canBeWatered in _canBeWateredObjects)
        {
            canBeWatered.Dry(time, DryRate);
        }
    }

    public void AddWaterCommands()
    {
        foreach (CanBeWatered canBeWatered in _canBeWateredObjects)
        {
            if (canBeWatered.WaterLevel < WateringThreshold && !canBeWatered.AlreadyWatering)
            {
                CommandData command = new CommandData()
                {
                    Interactable = canBeWatered.Interactable,
                    CommandType = Command.Water,
                    AdditionalData = new WateringCommandData()
                    {
                        WaterAmount = WaterAmount
                    }
                };
                canBeWatered.AlreadyWatering = true;
                Core.CommandsManagersHolder.CommandsManager.AddCommandManually(command);
            }
        }
    }
    public void AddCanBeWateredObject(CanBeWatered canBeWatered)
    {
        _canBeWateredObjects.Add(canBeWatered);
    }

    public void RemoveCanBeWateredObject(CanBeWatered canBeWatered)
    {
        if (_canBeWateredObjects.Contains(canBeWatered))
            _canBeWateredObjects.Remove(canBeWatered);
    }

}