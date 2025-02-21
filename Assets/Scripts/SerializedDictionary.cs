using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializedDictionary<TKey, TValue> : ISerializationCallbackReceiver {
    [SerializeField]
    private List<TKey> _keys = new();

    [SerializeField]
    private List<TValue> _values = new();

    private Dictionary<TKey, TValue> _dictionary = new();

    public Dictionary<TKey, TValue> Dictionary => _dictionary;

    public TValue this[TKey key] {
        get => _dictionary[key];
        set => _dictionary[key] = value;
    }

    public void OnBeforeSerialize() {
        /* _keys.Clear();
         _values.Clear();

         foreach (var kvp in _dictionary) {
             _keys.Add(kvp.Key);
             _values.Add(kvp.Value);
         }*/
    }

    public void OnAfterDeserialize() {
        _dictionary.Clear();

        for (int i = 0; i < Math.Min(_keys.Count, _values.Count); i++) {
            if (!_dictionary.ContainsKey(_keys[i])) {
                _dictionary[_keys[i]] = _values[i];
            }
        }
    }

    public void Add(TKey key, TValue value) {
        _dictionary[key] = value;
    }

    public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);

    public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);

    public void Remove(TKey key) => _dictionary.Remove(key);

    public void Clear() {
        _dictionary.Clear();
        _keys.Clear();
        _values.Clear();
    }
}