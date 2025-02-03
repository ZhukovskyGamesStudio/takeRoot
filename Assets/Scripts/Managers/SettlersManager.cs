using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SettlersManager : MonoBehaviour, IInitableInstance {
    private HashSet<Settler> _settlers = new HashSet<Settler>();
    private HashSet<SettlerData> _settlersDatas = new HashSet<SettlerData>();
    public static SettlersManager Instance => Core.SettlersManager;

    public HashSet<Settler> Settlers => _settlers;
    public HashSet<SettlerData> SettlersDatas => _settlersDatas;

    public IEnumerable<SettlerData> MySettlers {
        get {
            Race race = Core.Instance.MyRace();
            return _settlersDatas.Where(d => d.Race == Race.Both || d.Race == race);
        }
    }

    public void Init() {
        Core.SettlersManager = this;
        _settlers = new HashSet<Settler>(FindObjectsByType<Settler>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
        _settlersDatas = new HashSet<SettlerData>(_settlers.Select(s => s.SettlerData));
    }
}