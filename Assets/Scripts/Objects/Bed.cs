using UnityEngine;

public class Bed : MonoBehaviour {
    private AI.Settler _settler;

    [field: SerializeField]
    public Race Race { get; private set; }

    public Transform SleepPos;
    public Transform NearPos; //??
    public bool IsFree => _settler == null;

    public void SetSettler(AI.Settler settler) {
        _settler = settler;
        settler.Data.targets.Bed = this;
    }

    public void ReleaseBed() {
        if (IsFree) {
            return;
        }

        _settler.Data.targets.Bed = null;
        _settler = null;
    }
}