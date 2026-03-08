using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using ParcelTracking.Infrastructure.Configuration;

namespace ParcelTracking.Infrastructure.Persistence;

public class CosmosDbContext
{
    public CosmosClient Client { get; }

    public Database Database { get; }

    public Container ParcelContainer { get; set; }

    public Container ParcelEventsContainer { get; set; }

    public Container TrackingViewContainer { get; set; }

    public Container ProcessedEventsContainer { get; set; }

    public CosmosDbContext(IOptions<CosmosDbOptions> options)
    {
        var settings = options.Value;

        if (string.IsNullOrEmpty(settings.ConnectionString))
        {
            throw new Exception("COSMOS_CONNECTION environment variable not set");
        }

        Client = new CosmosClient(settings.ConnectionString);

        Database = Client.GetDatabase(settings.DatabaseName);
    }
}