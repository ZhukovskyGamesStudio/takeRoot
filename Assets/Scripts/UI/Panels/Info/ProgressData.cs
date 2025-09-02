using System;
using UniRx;

[Serializable]
public class ProgressData {
    public ReactiveProperty<int> Progress;
    public int Needed;
    public string Title;
    public bool InfoViewEnabled = true;
}