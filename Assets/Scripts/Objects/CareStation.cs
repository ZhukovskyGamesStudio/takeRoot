using UnityEngine;

public class CareStation : MonoBehaviour {
    
    private AI.Settler _settler, _caregiver;
    
    public Transform NearPos;
    public Transform CarePos;
    public Transform CaregiverPos;
    public bool IsFree => _settler == null;
    public bool IsFreeCaregiver => _caregiver == null;

    public void SetSettler(AI.Settler settler) {
        _settler = settler;
        settler.Data.needs.CareData.careStation = this;
    }

    public void ReleaseCareSettler() {
        if (IsFree) {
            return;
        }

        _settler.Data.needs.CareData.careStation = null;
        _settler = null;
    }
}