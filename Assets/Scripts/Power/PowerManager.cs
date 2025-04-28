using System;
using System.Collections.Generic;
using UnityEngine;

public class PowerManager : MonoBehaviour
{
    private List<PowerConsumer> _powerConsumers = new List<PowerConsumer>();
    private List<PowerProvider> _generators = new List<PowerProvider>();
    private Dictionary<Vector2Int, Wire> _wires = new Dictionary<Vector2Int, Wire>();

    private HashSet<Vector2Int> _checkedWires = new HashSet<Vector2Int>();
    
    public List<Sprite> WireDirectionSprites = new List<Sprite>();
    
    public GameObject wirePrefab;
    private void Awake()
    {
        Core.PowerManager = this;
        AddCreatedBeforeGameStartedWires();
        AddCreatedBeforeGameStartedGenerators();
        AddCreatedBeforeGameStartedPowerConsumers();
    }
    private void Update()
    {
        UpdateWireConnection(); 
        UpdatePowerConsumersConnection();
        if (Input.GetKeyDown(KeyCode.Space))
            CreateWireAt(Camera.main.ScreenToWorldPoint(Input.mousePosition).ToVector2Int());
        if (Input.GetKeyDown(KeyCode.R))
        {
            var wire = _wires[Camera.main.ScreenToWorldPoint(Input.mousePosition).ToVector2Int()];
            OnWireDestroyed(wire);
            Destroy(wire.gameObject);
        }
    }
    public void CreateWireAt(Vector2Int pos)
    {
        if (_wires.ContainsKey(pos)) return;
        Wire wire = Instantiate(wirePrefab, new Vector3(pos.x, pos.y), Quaternion.identity).GetComponent<Wire>();
        _wires.Add(pos, wire);
    }

    public void OnWireDestroyed(Wire wire)
    {
        var pos = wire.transform.position.ToVector2Int();
        if (!_wires.ContainsKey(pos)) return;
        
        _wires.Remove(pos);
    }

    public void CreateGeneratorAt(PowerProvider generatorPrefab, Vector2Int pos)
    {
        PowerProvider generator = Instantiate(generatorPrefab, new Vector3(pos.x, pos.y), Quaternion.identity).GetComponent<PowerProvider>();
        _generators.Add(generator);
    }

    public void OnGeneratorDestroyed(PowerProvider generator)
    {
        if (_generators.Contains(generator))
            _generators.Remove(generator);
    }

    private void UpdateWireConnection()
    {

        _checkedWires.Clear();
        foreach (PowerProvider biogenerator in _generators)
        {
            var connected = FloodFill(biogenerator.PowerSocketPosition, biogenerator);
            biogenerator.SetConnections(connected == 1);
        }
        foreach (Vector2Int wire in _wires.Keys)
        {
            FloodFillNotConnected(wire);
        }
    }

    private void UpdatePowerConsumersConnection()
    {
        foreach (PowerConsumer powerConsumer in _powerConsumers)
        {
            if (_wires.TryGetValue(powerConsumer.PowerSocketPosition, out var wire))
                powerConsumer.SetConnections(true, wire);
            else
                powerConsumer.SetConnections(false, null);
        }
    }
    private byte FloodFillNotConnected(Vector2Int pos)
    {
        if (!_wires.TryGetValue(pos, out var wire)) return 0;
        if (!_checkedWires.Add(pos)) return 1;
        
        byte wireDirection = 0b0000;
        byte shift = 3;
        wireDirection |= Convert.ToByte(FloodFillNotConnected(new Vector2Int(pos.x + 1, pos.y)) << shift--);
        wireDirection |= Convert.ToByte(FloodFillNotConnected(new Vector2Int(pos.x - 1, pos.y)) << shift--);
        wireDirection |= Convert.ToByte(FloodFillNotConnected(new Vector2Int(pos.x, pos.y + 1)) << shift--);
        wireDirection |= Convert.ToByte(FloodFillNotConnected(new Vector2Int(pos.x, pos.y - 1)) << shift);
        wire.SetWire(false, null, wireDirection);
        return 1;
    }
    private byte FloodFill(Vector2Int pos, PowerProvider generator)
    {
        if (!_wires.TryGetValue(pos, out var wire)) return 0;
        if (!_checkedWires.Add(pos)) return 1;
        
        byte wireDirection = 0b0000;
        byte shift = 3;
        wireDirection |= Convert.ToByte(FloodFill(new Vector2Int(pos.x + 1, pos.y), generator) << shift--);
        wireDirection |= Convert.ToByte(FloodFill(new Vector2Int(pos.x - 1, pos.y), generator) << shift--);
        wireDirection |= Convert.ToByte(FloodFill(new Vector2Int(pos.x, pos.y + 1), generator) << shift--);
        wireDirection |= Convert.ToByte(FloodFill(new Vector2Int(pos.x, pos.y - 1), generator) << shift);
        wire.SetWire(true, generator, wireDirection);
        return 1;
    }
    
    private void AddCreatedBeforeGameStartedWires()
    {
        foreach (Wire wire in FindObjectsByType<Wire>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            _wires.Add(wire.transform.position.ToVector2Int(), wire);
        }
    }    
    private void AddCreatedBeforeGameStartedGenerators()
    {
        foreach (PowerProvider generator in FindObjectsByType<PowerProvider>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            _generators.Add(generator);
        }
    }    
    private void AddCreatedBeforeGameStartedPowerConsumers()
    {
        foreach (PowerConsumer powerConsumer in FindObjectsByType<PowerConsumer>(FindObjectsInactive.Exclude, FindObjectsSortMode.None))
        {
            _powerConsumers.Add(powerConsumer);
        }
    }
}

public enum WireDirection
{
    None = 0b0000,
    Down = 0b0001,
    Up = 0b0010,
    UpDown = 0b0011,
    Left = 0b0100,
    LeftDown = 0b0101,
    LeftUp = 0b0110,
    LeftUpDown = 0b0111,
    Right = 0b1000,
    RightDown = 0b1001,
    RightUp = 0b1010,
    RightUpDown = 0b1011,
    RightLeft = 0b1100,
    RightLeftDown = 0b1101,
    RightLeftUp = 0b1110,
    RightLeftUpDown = 0b1111,
}