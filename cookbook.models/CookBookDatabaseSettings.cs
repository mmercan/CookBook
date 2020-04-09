namespace CookBook.Models
{
    public class CookBookDatabaseSettings : ICookBookDatabaseSettings
    {
        public string CookBookCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface ICookBookDatabaseSettings
    {
        string CookBookCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}