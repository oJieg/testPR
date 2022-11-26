using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using testPR.DataBase;

namespace testPR
{
    internal class DeliteItem
    {
        private readonly ApplicationContext _applicationContext;
        private readonly ElasticsearchClient _client;
        private readonly string _indexName;

        public DeliteItem(ApplicationContext applicationContext, ElasticsearchClient client, string indexName)
        {
            _applicationContext = applicationContext;
            _client = client;
            _indexName = indexName;
        }

        public async Task<bool> DeliteId(int id)
        {
            Article? article = await _applicationContext.Articles.Where(x => x.ID == id).FirstOrDefaultAsync();
            if (article == null)
                return false;

            _applicationContext.Remove(article);
            await _applicationContext.SaveChangesAsync();

            var response = await _client.DeleteAsync(_indexName, id);

            if (response.IsValidResponse)
                return false;

            return true;
        }
    }
}
