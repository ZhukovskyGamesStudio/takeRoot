using System;

namespace AI.Node.Jobs {
    public class Job_WaterPlant: Sequence {
        public Job_WaterPlant(Settler settler, IFarmingService farmingService) {
            AddChild(new Conditional(() => settler.Data.names.Race == Race.Plants));
            AddChild(new Action_FindFarmingPlotWithCondition(settler, farmingService, NeedsWatering));

            SettlerData data = settler.Data;
            Func<bool> condition = () => NeedsWatering(data.targets.FarmingPlot);
            ConditionalAction move = new ConditionalAction().Do(new Action_MoveToPos(settler)).While(condition);

            Selector moveOrCancel = new Selector().AddChild(move).AddChild(new Action_ClearFarmingPlotFromFarmer(settler));

            AddChild(moveOrCancel);
            AddChild(new Action_WaterPlant(settler));
        }

        private bool NeedsWatering(FarmingPlot plot) => plot && plot.PlantState == FarmingPlantState.WaitingForWater;
    }
}