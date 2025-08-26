using UnityEngine;

public class Bed : MonoBehaviour {
    [SerializeField]
    private AI.Settler _settler;

    public Transform SleepPos;
    public Transform NearPos; //??
    public bool IsFree => _settler == null;

    public void SetSettler(AI.Settler settler) {
        _settler = settler;
        settler.Data.energy.bed = this;
    }

    public void ReleaseBed() {
        if (IsFree) {
            return;
        }

        _settler.Data.energy.bed = null;
        _settler = null;
    }
}