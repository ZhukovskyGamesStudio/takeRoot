using Cysharp.Threading.Tasks;

public interface ILevelGenerationService : IService {
    public UniTask Generate();
}