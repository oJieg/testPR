namespace testPR.DataBase
{
    public class Article
    {
        public int ID { get; set; }
        public List<string> Rubrics { get; set; }
        public string Text { get; set; }
        public DateTime CreatedDate { get; set; }

        public static explicit operator IndexArticle(Article article)
        {
            return new IndexArticle()
            {
                Id = article.ID,
                Text = article.Text
            };
        }
    }


}
