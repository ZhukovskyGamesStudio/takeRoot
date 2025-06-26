public interface ISelectionService : IService
{
	public Selectable Selected { get;}
	public bool IsEnabled {get; set;}
}