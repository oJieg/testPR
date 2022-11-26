using testPR.DataBase;
using CsvHelper;
using System.Globalization;

namespace testPR
{
    internal class ParserCsvToDB : IStartMigration
    {
        private readonly string _fileName;
        private readonly ApplicationContext _applicationContext;

        public ParserCsvToDB(string fileName, ApplicationContext applicationContext)
        {
            _fileName = fileName;
            _applicationContext = applicationContext;
        }

        public async Task<bool> StartMigrationTry()
        {
            try
            {
                using (StreamReader streamReader = new StreamReader(_fileName))
                {
                    using (CsvReader csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                    {
                        csvReader.Read();
                        csvReader.ReadHeader();
                        while (csvReader.Read())
                        {

                            _applicationContext.Add(ParsCSV(csvReader));
                            await _applicationContext.SaveChangesAsync();
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"не удалось мигрировать из csv в базу ошибка: {ex}");
                return false;
            }
        }

        public virtual Article ParsCSV(CsvReader csvReader)
        {
            return new Article()
            {
                Text = csvReader.GetField("text"),
                CreatedDate = DateTime.Parse(csvReader.GetField("created_date")).ToUniversalTime(),
                Rubrics = csvReader.GetField("rubrics").Trim(new char[] { '[', ']' }).Replace("'", "").Split(", ").ToList()
            };
        }
    }
}
