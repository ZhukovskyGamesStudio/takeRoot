using UnityEngine;

public class CareStation : MonoBehaviour {
    private AI.Settler _settler, _caregiver;

    public Transform NearPos;
    public Transform CarePos;
    public Transform CaregiverPos;
    public bool IsFree => _settler == null;
    public Race SettlerRace => _settler.Data.names.Race;
    public bool IsFreeCaregiver => _caregiver == null;

    public bool CareSettlerReady = false;

    public void SetSettler(AI.Settler settler) {
        _settler = settler;
        settler.Data.targets.SitOnCareStation = this;
    }

    public void ReleaseCareSettler() {
        if (IsFree) {
            return;
        }

        CareSettlerReady = false;
        _settler.Data.targets.SitOnCareStation = null;
        _settler = null;
    }

    public void AddCare() {
        _settler.Data.needs.Value.CareData.currentCare += _settler.Data.needs.Value.CareData.onStationCareChange;
    }

    public void SetCaregiverSettler(AI.Settler settler) {
        _caregiver = settler;
        settler.Data.targets.SitOnCareStation = this;
    }

    public void ReleaseCaregiverSettler() {
        if (IsFreeCaregiver) {
            return;
        }

        _caregiver.Data.targets.SitOnCareStation = null;
        _caregiver = null;
    }
}