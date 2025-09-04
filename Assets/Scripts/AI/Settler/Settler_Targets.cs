using System;

namespace AI {
	[Serializable]
	public class Settler_Targets {
		public CraftingStation CraftingStation;
		public BuildingBlueprint BuildingBlueprint;
		public FarmingPlot FarmingPlot;
		public TimeMachine TimeMachine;
		public CareStation CareStation;
		public CareStation SitOnCareStation;
		public DinamoMachine DinamoMachine;
		public Bed Bed;
		public Cooler Cooler;
		public ElectricityLevel ElectricitySource;
	}
}