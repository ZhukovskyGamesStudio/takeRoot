using System;

namespace AI.Node.Jobs {
    public class Job_Plant : Sequence {
        public Job_Plant(Settler settler, IFarmingService farmingService) {
            AddChild(new Conditional(() => settler.Data.names.Race == Race.Plants));
            AddChild(new Action_FindFarmingPlotWithCondition(settler, farmingService, CanBePlanted));

            SettlerData data = settler.Data;
            Func<bool> condition = () => data.targets.FarmingPlot && data.targets.FarmingPlot.NeedsPlanting();
            ConditionalAction move = new ConditionalAction().Do(new Action_MoveToPos(settler)).While(condition);

            Selector moveOrCancel = new Selector().AddChild(move).AddChild(new Action_ClearFarmingPlotFromFarmer(settler));

            AddChild(moveOrCancel);
            AddChild(new Action_Plant(settler));
        }

        private bool CanBePlanted(FarmingPlot plot) => plot && plot.PlantState == FarmingPlantState.WaitingForPlanting;
    }
}