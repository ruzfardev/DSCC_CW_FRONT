namespace DSCC_MVC.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public int PublishedYear { get; set; }
        public string ISBN { get; set; }
    }
}
