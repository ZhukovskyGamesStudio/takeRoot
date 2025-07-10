using UniRx;

public interface ISelectionService : IService
{
	public Selectable Selected { get;}
	public ReactiveProperty<bool> IsEnabled {get; set;}
}