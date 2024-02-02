namespace Teqniqly.AzureDigitalTwinsQueryApi
{
    /// <summary>
    /// Provides an interface for interacting with Azure Digital Twin service.
    /// </summary>
    public interface IAzureDigitalTwinService
    {
        /// <summary>
        /// Sends a query to the Azure Digital Twin service and retrieves the results.
        /// </summary>
        /// <param name="query">The query to send to the Azure Digital Twin service.</param>
        /// <returns>A Task that represents the asynchronous operation. The Task result contains a QueryResult object.</returns>
        Task<QueryResult> Query(string query);
    }
}
