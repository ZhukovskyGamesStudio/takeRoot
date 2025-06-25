using CodeBase.Services;

public class ServiceLocatorLoader_Main
{
    private readonly ServiceLocator _services;

    public ServiceLocatorLoader_Main()
    {
        _services = ServiceLocator.Container;
    }
    
    
    public void RegisterServices()
    {
        _services.RegisterSingle<IAssetProvider>(new AssetProvider());
        _services.RegisterSingle<IDataProvider>(new DataProvider());
        _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssetProvider>(), _services.Single<IDataProvider>()));
    }
}