using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WorldObjects;

public class SettlersManager : MonoBehaviour, IInitableInstance {
    [SerializeField]
    private Chamomile _chamomilePrefab;

    [SerializeField]
    private Lamp _lampPrefab;

    [SerializeField]
    private CombinedSettler _combinedSettlerPrefab;

    private HashSet<Settler> _settlers = new HashSet<Settler>();
    private HashSet<SettlerData> _settlersDatas = new HashSet<SettlerData>();
    public static SettlersManager Instance => Core.SettlersManager;

    public HashSet<Settler> Settlers => _settlers;
    public HashSet<SettlerData> SettlersDatas => _settlersDatas;

    public IEnumerable<SettlerData> MySettlers {
        get {
            Race race = Core.MyRace();
            return _settlersDatas.Where(d => d.Race == Race.Both || d.Race == race);
        }
    }

    public void Init() {
        Core.SettlersManager = this;
        _settlers = new HashSet<Settler>(FindObjectsByType<Settler>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
        _settlersDatas = new HashSet<SettlerData>(_settlers.Select(s => s.SettlerData));
    }

    public Settler GetSettlerAt(Vector2Int position) {
        return _settlers.FirstOrDefault(d => d.GetCellOnGrid == position);
    }

    public void DestroySettler(Settler settler) {
        var interactable = settler.GetEcsComponent<TacticalInteractable>();
        Settlers.Remove(settler);
        _settlersDatas.Remove(settler.SettlerData);
        interactable.OnDestroyed();
    }

    public void CreateCombinedSettlerAt(Vector2Int position) {
        var instance = Instantiate(_combinedSettlerPrefab, new Vector3(position.x, position.y), Quaternion.identity) as Settler;
        Settlers.Add(instance);
        _settlersDatas.Add(instance.SettlerData);
    }

    public void SpawnSettlerAt(Race race, Vector2Int position) {
        var settler = Instantiate(race == Race.Plants ? _chamomilePrefab : _lampPrefab, new Vector3(position.x, position.y),
            Quaternion.identity) as Settler;
        Settlers.Add(settler);
        _settlersDatas.Add(settler.SettlerData);
    }
}