using System;

namespace AI.Node.Jobs {
    public class Job_Plant : Sequence {
        public Job_Plant(Settler settler, IFarmingService farmingService) {
            AddChild(new Conditional(() => settler.Data.names.Race == Race.Plants));
            AddChild(new Action_FindFarmingPlotWaitingForPlant(settler, farmingService));

            SettlerData data = settler.Data;
            Func<bool> condition = () => data.farming.FarmingPlot && data.farming.FarmingPlot.NeedsPlanting();
            ConditionalAction move = new ConditionalAction().Do(new Action_MoveToPos(settler)).While(condition);
            Action_Plant plant = new Action_Plant(settler);

            AddChild(move);
            AddChild(plant);
        }
    }
}