using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Table : Furniture, ISearchable {
    public ResourseStorage ResorceStorage = new ResourseStorage();

    public bool IsStorageActive;

    protected override void Awake() {
        base.Awake();
        ResorceStorage.Init(2, new List<ResorceData>());
    }

    protected override void OnExplored() {
        IsStorageActive = true;
    }

    private void AddToStorage(ResorceData data) {
        ResorceStorage.Add(data);
    }

    protected override InfoBookData GetInfoData() {
        var d = new InfoBookData() {
            Icon = _icon.sprite,
            Name = gameObject.name,
            Resources = ResorceStorage.ResorceDatas.ToList()
        };
        return d;
    }
}