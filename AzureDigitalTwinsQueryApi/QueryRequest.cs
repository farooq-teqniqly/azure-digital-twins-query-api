namespace Teqniqly.AzureDigitalTwinsQueryApi
{
    /// <summary>
    /// Represents a request to query the Azure Digital Twin service.
    /// </summary>
    public class QueryRequest
    {
        /// <summary>
        /// Gets or sets the query to send to the Azure Digital Twin service.
        /// </summary>
        public string Query { get; set; } = null!;
    }
}
