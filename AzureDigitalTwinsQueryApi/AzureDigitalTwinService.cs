using Azure;
using Azure.DigitalTwins.Core;

namespace Teqniqly.AzureDigitalTwinsQueryApi
{
    /// <summary>
    /// Provides a service for interacting with Azure Digital Twin service.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="AzureDigitalTwinService"/> class.
    /// </remarks>
    /// <param name="digitalTwinsClient">The client used to interact with the Azure Digital Twin service.</param>
    public class AzureDigitalTwinService(DigitalTwinsClient digitalTwinsClient) : IAzureDigitalTwinService
    {
        private readonly DigitalTwinsClient digitalTwinsClient = digitalTwinsClient;

        /// <summary>
        /// Sends a query to the Azure Digital Twin service and retrieves the results.
        /// </summary>
        /// <param name="query">The query to send to the Azure Digital Twin service.</param>
        /// <returns>A Task that represents the asynchronous operation. The Task result contains a QueryResult object.</returns>
        public async Task<QueryResult> Query(string query)
        {
            var result = this.digitalTwinsClient.QueryAsync<BasicDigitalTwin>(query);
            var queryResult = new QueryResult();

            await foreach (Page<BasicDigitalTwin> page in result.AsPages())
            {
                if (QueryChargeHelper.TryGetQueryCharge(page, out float charge))
                {
                    queryResult.QueryCharge += charge;
                }

                foreach (BasicDigitalTwin twin in page.Values)
                {
                    queryResult.Twins.Add(twin);
                }
            }

            return queryResult;
        }
    }
}
