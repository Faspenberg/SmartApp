using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(config, services) => 
    {
        services.AddDbContext<CosmosContext>(x => x.UseCosmos(config.Configuration.GetConnectionString("CosmosDb")!, "KYH"));

    });
    
    .Build();

host.Run();
