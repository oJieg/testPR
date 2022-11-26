using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using testPR.DataBase;

namespace testPR
{
    public class Program
    {

        static async Task Main(string[] args)
        {
            string IDCloud = "1:dXMtY2VudHJhbDEuZ2NwLmNsb3VkLmVzLmlvJDQyNWE0YTAxYmJlYjQxMjc4Y2IyOGRjYjQ1ZGI4NWE3JGVjYTdmNjViODFjMTRlMTg5YzllZmU1NGJiMWVmNTQ0";
            ApiKey apiKey = new("LWM1UnNJUUJyUVhNUnhrMlhUUlo6NFo0ai1NeTBRaGkwZnY5Q3FHNEdsZw==");

            string indexName = "test3";
            string nameCsvFile = "1.csv";

            ElasticsearchClient client = new ElasticsearchClient(IDCloud, apiKey);
            ElasticSearch elasticSearch = new ElasticSearch(client, indexName);

            using (ApplicationContext applicationContext = new())
            {
                IStartMigration csvToDB = new ParserCsvToDB(nameCsvFile, applicationContext);
                IStartMigration DBtoElastic = new IndexingDocumentIsDB(applicationContext, client, "test2");

                DataBaseSearch dataBaseSearch = new(applicationContext);
                DeliteItem deliteItem = new(applicationContext, client, indexName);

                ConsoleView consoleView = new(
                    csvToDB.StartMigrationTry,
                    DBtoElastic.StartMigrationTry,
                    deliteItem.DeliteId,
                    elasticSearch.Search,
                    dataBaseSearch.Get);

                await consoleView.OneScreen();
            }
        }
    }
}