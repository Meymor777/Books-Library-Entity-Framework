using BooksLibrary.DB;

namespace BooksLibrary.BL
{
    public interface IControlerDB
    {
        ControlerDB GetControlerDB(BooksLibraryDBContext dBContext);
    }
}
