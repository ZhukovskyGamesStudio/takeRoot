using UnityEngine;

[CreateAssetMenu(fileName = "MainInfoData", menuName = "Scriptable Objects/MainInfoData", order = 0)]
public class MainInfoData : ScriptableObject {
    public Sprite Icon;
    public string Name;
    public string Description;
}