using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ResourseStorage {
    public ResourceData[] ResorceDatas;
    public int MaxStackAmount = 50;

    public void Init(int cells, List<ResourceData> resorceDatas) {
        ResorceDatas = new ResourceData[cells];
        foreach (ResourceData r in resorceDatas) {
            Add(r);
        }
    }

    public void Add(ResourceData data) {
        int cell = 0;
        ResorceDatas = ResorceDatas.OrderByDescending(r => r != null).ToArray();
        while (data.Amount > 0 && cell < ResorceDatas.Length) {
            ResorceDatas[cell] ??= new ResourceData {
                ResourceType = data.ResourceType
            };

            if (ResorceDatas[cell].ResourceType == data.ResourceType) {
                int diff = MaxStackAmount - ResorceDatas[cell].Amount;
                ResorceDatas[cell].Amount += Mathf.Min(diff,data.Amount);
                data.Amount -= diff;
            }

            cell++;
        }
    }

    public int CanFitResource(ResorceData data) {
        int cell = 0;
        int res = 0;
        while (data.Amount > 0 && cell < ResorceDatas.Length) {
            if (ResorceDatas[cell] == null) {
                res += MaxStackAmount;
            } else if (ResorceDatas[cell].ResourceType == data.ResourceType) {
                int diff = MaxStackAmount - ResorceDatas[cell].Amount;
                res += diff;
            }

            cell++;
        }

        if (res > data.Amount) {
            res = data.Amount;
        }

        return res;
    }
}