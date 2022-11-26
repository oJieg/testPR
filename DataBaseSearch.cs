using Microsoft.EntityFrameworkCore;
using testPR.DataBase;

namespace testPR
{
    internal class DataBaseSearch
    {
        private readonly ApplicationContext _applicationContext;

        public DataBaseSearch(ApplicationContext applicationContext) 
        {
            _applicationContext = applicationContext;
        }

        public async Task<List<Article>> Get(List<int> idList)
        {
           return await _applicationContext.Articles.Where(x=> idList.Contains(x.ID)).OrderBy(x=>x.CreatedDate).ToListAsync();
        }
    }
}
