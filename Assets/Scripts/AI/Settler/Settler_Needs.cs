using System;
using AI;
using UnityEngine;

[Serializable]
public class Settler_Needs {
    public int Hp;
    public int MaxHp;

    public Settler_StressData StressData;

    public int Hunger;
    public int MaxHunger;

    public int Care;
    public int MaxCare;
}