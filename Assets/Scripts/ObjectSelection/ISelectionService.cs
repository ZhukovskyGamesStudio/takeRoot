using UniRx;

public interface ISelectionService : IService
{
	public ReactiveProperty<Selectable> SelectedReactive {get; set;}
	public ReactiveProperty<bool> IsEnabled {get; set;}
	public void Unselect();
}