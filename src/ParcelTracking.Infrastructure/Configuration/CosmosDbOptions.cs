using System.ComponentModel.DataAnnotations;

namespace ParcelTracking.Infrastructure.Configuration;

public class CosmosDbOptions
{
    [Required]
    public string? ConnectionString { get; set; }=string.Empty;

    [Required]
    public string? DatabaseName { get; set; }
}