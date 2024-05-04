namespace Repository;

public class BooksLookupService
{
    public List<string> GetGenres()
    {
        //return a new collection with four genres
        return new ()
        {
            "Fiction",
            "Thriller",
            "Comedy",
            "Autobiography"
        };
    }
}