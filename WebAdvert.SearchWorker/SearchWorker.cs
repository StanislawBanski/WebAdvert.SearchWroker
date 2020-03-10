using AdvertApi.Models.Messages;
using Amazon.Lambda.Core;
using Amazon.Lambda.SNSEvents;
using Nest;
using Newtonsoft.Json;
using System.Threading.Tasks;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
namespace SearchWorker
{
    public class SearchWorker
    {
        private readonly IElasticClient client;

        public SearchWorker() : this(ElasticSearchHelper.GetInstance(ConfigurationHelper.Instance))
        {

        }

        public SearchWorker(IElasticClient client)
        {
            this.client = client;
        }


        public async Task Function(SNSEvent snsEvent, ILambdaContext context)
        {
            foreach (var record in snsEvent.Records)
            {
                context.Logger.LogLine(record.Sns.Message);
                var message = JsonConvert.DeserializeObject<AdvertConfirmedMessage>(record.Sns.Message);
                var document = MappingHelper.Map(message);

                await client.IndexDocumentAsync(document);
            }
        }
    }
}
