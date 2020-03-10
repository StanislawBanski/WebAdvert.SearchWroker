using Microsoft.Extensions.Configuration;
using Nest;
using System;

namespace SearchWorker
{
    public static class ElasticSearchHelper
    {
        private static IElasticClient client;

        public static IElasticClient GetInstance(IConfiguration config)
        {
            if (client == null)
            {
                var url = config.GetSection("ES").GetValue<string>("url");
                var settings = new ConnectionSettings(new Uri(url))
                    .DefaultIndex("adverts")
                    .DefaultMappingFor<AdvertType>(m => m.IdProperty(x => x.Id));

                client = new ElasticClient(settings);
            }

            return client;
        }

    }

}
