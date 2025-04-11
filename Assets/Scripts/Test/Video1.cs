using System;
using System.Collections;
using System.Collections.Generic;
using Settlers.Crafting;
using UnityEngine;
using UnityEngine.Serialization;

public class Video1 : MonoBehaviour
{
    public Settler flowerSettler;
    public Interactable firstFlower;
    public Interactable secondFlower;
    public bool firstCommandAdded;
    public bool secondCommandAdded;
    
    public List<Settler> craftingSettlers;
    public CraftingStationable craftingStation;
    public bool craftingCommandsAdded;


    private void Update()
    {
        if (flowerSettler.TakenCommand == null)
        {
            var commandsManager = Core.CommandsManagersHolder.GetCommandManagerByRace(Race.Plants);
            if (!firstCommandAdded)
            {
                var firstCommand = new CommandData()
                {
                    Settler = flowerSettler,
                    CommandType = Command.Search,
                    Interactable = firstFlower
                };
                commandsManager.AddSubsequentCommand(firstCommand);
                firstCommandAdded = true;
                return;
            }
            if (!secondCommandAdded)
            {
                var secondCommand = new CommandData()
                {
                    Settler = flowerSettler,
                    CommandType = Command.Search,
                    Interactable = secondFlower
                };
                commandsManager.AddSubsequentCommand(secondCommand);
                secondCommandAdded = true;
                return;
            }
        }
    }

}