using testPR.DataBase;

namespace testPR
{
    internal class ConsoleView
    {
        private readonly Func<Task<bool>> _csvToDB;
        private readonly Func<Task<bool>> _DBtoElastic;
        private readonly Func<int, Task<bool>> _delite;
        private readonly Func<string, Task<List<int>>> _search;
        private readonly Func<List<int>, Task<List<Article>>> _databaseRequest;

        public ConsoleView(Func<Task<bool>> csvToDB, Func<Task<bool>> DBtoElastic, Func<int, Task<bool>> delite, Func<string, Task<List<int>>> search, Func<List<int>, Task<List<Article>>> databaseRequest)
        {
            _csvToDB = csvToDB;
            _DBtoElastic = DBtoElastic;
            _search = search;
            _databaseRequest = databaseRequest;
            _delite = delite;
        }

        public async Task OneScreen()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("1 - импорт данных из csv в базу данных и дальше в эластик");
                Console.WriteLine("2 - удалить данные по id");
                Console.WriteLine("3 - поиск");
                Console.WriteLine("0 - выход");
                try
                {
                    int read = Convert.ToInt32(Console.ReadLine());
                    switch (read)
                    {
                        case 0: return;
                        case 1:
                            if (!await _csvToDB())
                                ErrorMassage("не удалась миграция из csv в базу данных");
                            if(!await _DBtoElastic())
                                ErrorMassage("не удалась миграция из базы данных в еластик");
                            break;
                        case 2:
                            int delId = DeliteID();
                            if (delId != -1)
                                if (!await _delite(delId))
                                    ErrorMassage("удаление не удалось");
                            break;
                        case 3:
                            await TwoScreen();
                            break;
                        default:
                            ErrorMassage("нет такого варианта");
                            break;
                    }
                }
                catch
                {
                    ErrorMassage("используйте цифры");
                }
            }

        }

        public int DeliteID()
        {
            Console.Clear();
            Console.WriteLine("введите id для удаления");
            try
            {
                return Convert.ToInt32(Console.ReadLine());
            }
            catch
            {
                ErrorMassage("не удалось удалить по id");
                return -1;
            }
        }

        public async Task TwoScreen()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("введите текст запроса");
                Console.WriteLine("Для возврата оставьте строку пустой");
                string searchWords = Console.ReadLine();

                if (string.IsNullOrEmpty(searchWords))
                {
                    return;
                }
                List<Article> article = await _databaseRequest(await _search(searchWords));
                foreach (Article articleItem in article)
                {
                    Console.WriteLine(articleItem.ID);
                    Console.WriteLine(articleItem.CreatedDate);
                    Console.WriteLine(articleItem.Rubrics.ToString());
                    Console.WriteLine(articleItem.Text);
                    Console.WriteLine("============================");
                }

                Console.WriteLine($"Найдено:{article.Count()}/20");
                Console.WriteLine("Нажмите любую наливишу для нового поиска");
                Console.ReadKey();
            }
        }

        private void ErrorMassage(string masage)
        {
            Console.WriteLine(masage);
            Console.WriteLine("для продолжения наждмите любую кнопку");
            Console.ReadKey();
        }
    }
}
