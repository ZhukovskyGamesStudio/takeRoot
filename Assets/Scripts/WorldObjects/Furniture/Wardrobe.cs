using System.Collections.Generic;
using System.Linq;

public class Wardrobe : Furniture, ISearchable
{
    public ResourseStorage ResorceStorage = new ResourseStorage();

    public bool IsStorageActive;

    protected override void Awake() {
        base.Awake();
        ResorceStorage.Init(2, new List<ResourceData>());
    }

    protected override void OnExplored() {
        IsStorageActive = true;
    }

    private void AddToStorage(ResourceData data) {
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