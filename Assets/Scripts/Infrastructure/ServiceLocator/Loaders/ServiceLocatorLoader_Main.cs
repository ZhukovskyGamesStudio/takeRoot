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
    }
}