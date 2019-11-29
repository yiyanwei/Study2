namespace BooksApi.Models
{
    public class BookstoreDatabaseSettings : IBookstoreDatabaseSettings
    {
        public string BooksCollectionName { get; set; }
        public string ConnectinString { get; set; }
        public string DatabaseName { get; set; }
    }
    public interface IBookstoreDatabaseSettings
    {
        string BooksCollectionName { get; set; }
        string ConnectinString { get; set; }
        string DatabaseName { get; set; }
    }
}