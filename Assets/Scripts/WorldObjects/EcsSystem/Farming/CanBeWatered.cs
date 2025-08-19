using System;
using System.Data;
using UnityEngine;

public class CanBeWatered : ECSComponent
{
    public float WaterLevel;
    public bool AlreadyWatering;
    
    public Interactable Interactable {get; private set;}
    public override void Init(ECSEntity entity)
    {
        if (Core.WateringManager == null)
        {
            Debug.LogWarning("Watering Manager not found");
            return;
        }
        Core.WateringManager.AddCanBeWateredObject(this);
        Interactable = entity.GetEcsComponent<Interactable>();
        Interactable.OnCommandPerformed += OnCommandPerformed;
    }

    private void Water(float amount)
    {
        WaterLevel += amount;
        WaterLevel = Mathf.Clamp(WaterLevel, 0f, 100f);
        AlreadyWatering = false;
        UpdateView();
    }

    public void Dry(float time, float dryRate)
    {
        WaterLevel -= time * dryRate;
        WaterLevel = Mathf.Clamp(WaterLevel, 0f, 100f);
        UpdateView();
    }

    private void UpdateView()
    {
        //TODO: Water level state animation or sprite switch
    }

    private void OnCommandPerformed(CommandData command)
    {
        if (command.CommandType == Command.Water)
        {
            WateringCommandData waterCommandData = (WateringCommandData)command.AdditionalData;
            Water(waterCommandData.WaterAmount);
        }
    }
    private void OnDestroy()
    {
        Core.WateringManager.RemoveCanBeWateredObject(this);
    }
    
    public override int GetDependancyPriority()
    {
        return 0;
    }
}