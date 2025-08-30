using System;

namespace AI.Node.Jobs {
    public class Job_Harvest : Sequence {
        public Job_Harvest(Settler settler, IFarmingService farmingService) {
            AddChild(new Conditional(() => settler.Data.names.Race == Race.Plants));
            AddChild(new Action_FindFarmingPlotWithCondition(settler, farmingService, CanBeHarvested));

            SettlerData data = settler.Data;
            Func<bool> condition = () => CanBeHarvested(data.farming.FarmingPlot);
            ConditionalAction move = new ConditionalAction().Do(new Action_MoveToPos(settler)).While(condition);

            Selector moveOrCancel = new Selector().AddChild(move).AddChild(new Action_ClearFarmingPlotFromFarmer(settler));

            AddChild(moveOrCancel);
            AddChild(new Action_Harvest(settler));
        }

        private bool CanBeHarvested(FarmingPlot plot) => plot && plot.PlantState == FarmingPlantState.ReadyToHarvest;
    }
}