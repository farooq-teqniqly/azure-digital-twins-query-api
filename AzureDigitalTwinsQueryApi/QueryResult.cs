using Azure.DigitalTwins.Core;

namespace Teqniqly.AzureDigitalTwinsQueryApi
{
    /// <summary>
    /// Represents the result of a query to the Azure Digital Twin service.
    /// </summary>
    public class QueryResult
    {
        /// <summary>
        /// Gets or sets the list of digital twins returned by the query.
        /// </summary>
        public IList<BasicDigitalTwin> Twins { get; set; }

        /// <summary>
        /// Gets or sets the total charge of the query.
        /// </summary>
        public float QueryCharge { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryResult"/> class.
        /// </summary>
        public QueryResult()
        {
            this.Twins = new List<BasicDigitalTwin>();
            this.QueryCharge = 0;
        }
    }
}
