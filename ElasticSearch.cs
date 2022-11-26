using Elastic.Clients.Elasticsearch;
using testPR.DataBase;

namespace testPR
{
    internal class ElasticSearch
    {
        private readonly ElasticsearchClient _client;
        private readonly string _indexName;
        public ElasticSearch(ElasticsearchClient client, string indexName)
        {
            _client = client;
            _indexName = indexName;
        }

        public async Task<List<int>> Search(string textSearch)
        {
            var response = await _client.SearchAsync<IndexArticle>(s => s
                .Index("test2")
                .Size(20)
                .Query(q => q.Match(t => t
                    .Field(f => f.Text)
                    .Query(textSearch)
                    ))
            );

            if (response.IsValidResponse)
            {
                List<int> id = new(20);
                id = (response.Documents.Select(x => x.Id).ToList());
                return id;
            }
            return new List<int> { 0 };
        }
    }
}
