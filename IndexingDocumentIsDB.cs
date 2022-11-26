using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using testPR.DataBase;

namespace testPR
{
    internal class IndexingDocumentIsDB : IStartMigration
    {
        private readonly ApplicationContext _applicationContext;
        private readonly ElasticsearchClient _elasticsearchClient;
        private readonly string _indexName;

        public IndexingDocumentIsDB(ApplicationContext applicationContext,
            ElasticsearchClient elasticsearchClient, string indexName)
        {
            _applicationContext = applicationContext;
            _elasticsearchClient = elasticsearchClient;
            _indexName = indexName;
        }
        public async Task<bool> StartMigrationTry()
        {
            try
            {
                int count = await _applicationContext.Articles.CountAsync();
                for (int i = 0; i < count; i++)
                {
                    IndexArticle article = (IndexArticle)await _applicationContext.Articles.Where(article => article.ID == i + 1).FirstAsync();
                    var response = await _elasticsearchClient.IndexAsync(article, request => request.Index(_indexName));
                    if (response.IsValidResponse)
                    {
                        Console.WriteLine($"Index document with ID {response.Id} succeeded.");
                    }
                    else
                    {
                        Console.WriteLine("произошла ошибка добавления данных в еластик");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"не удалось мигрировать из бд в еластик {ex}");
                return false;
            }
        }
    }
}
