using System;
using AI;

[Serializable]
public class Settler_Needs {
    public int Hp;
    public int MaxHp;

    public Settler_EnergyData Energy;
    public Settler_StressData StressData;
    public Settler_SatietyData SatietyData;
    public Settler_CareData CareData;
}