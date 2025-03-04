public class CraftingStationable : ECSComponent {
    private CraftingCombinedCommand _craftingCombinedCommand;
    private Interactable _interactable;

    private void Awake() {
        _interactable = GetComponent<Interactable>();
        _interactable.OnCommandPerformed += OnCommandPerformed;
    }

    private void Update() {
        //TODO try to add command to queue
        if (_craftingCombinedCommand == null && true) {
            //создаёт крафтинг комбайнд команду, наполняем её данными и всё
        }
    }

    public override int GetDependancyPriority() {
        return 3;
    }

    public override void Init(ECSEntity entity) {
        _interactable = entity.GetEcsComponent<Interactable>();
        _interactable.OnCommandPerformed += OnCommandPerformed;
    }

    private void OnCommandPerformed(CommandData obj) {
        _craftingCombinedCommand?.OnCommandPerformed(obj);
    }
}