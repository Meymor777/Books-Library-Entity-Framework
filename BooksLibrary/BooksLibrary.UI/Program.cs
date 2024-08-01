using BooksLibrary.BL;
using BooksLibrary.DB;
using Microsoft.EntityFrameworkCore;
using Ninject;

namespace BooksLibrary.UI
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            IKernel ninjectKernel = new StandardKernel(new SimpleConfigModule());
            BooksLibraryDBContext db = ninjectKernel.Get<BooksLibraryDBContext>();
            ControlerDB controlerDB = ninjectKernel.Get<ControlerDB>();
            db.Database.EnsureCreated();

            bool ContinueWorking = true;
            WriteInConsoleInstuctions();
            while (ContinueWorking)
            {
                ConsoleKeyInfo key = Console.ReadKey();
                Console.WriteLine(":");
                if (!Int32.TryParse(key.KeyChar.ToString(), out int operation))
                {
                    continue;
                }
                Console.WriteLine();
                switch (operation)
                {
                    case 1:
                        await AddDataFromFile(controlerDB);
                        break;
                    case 2:
                        await ShowDataFromDataBase(controlerDB);
                        break;
                    case 3:
                        await CreateFileFromDataBase(controlerDB);
                        break;
                    case 4:
                        await ClearDataBase(controlerDB);
                        break;
                    case 5:
                        ContinueWorking = false;
                        break;
                    default:
                        break;
                }
                Console.WriteLine();
            }
        }

        public static void WriteInConsoleInstuctions()
        {
            Console.WriteLine(String.Empty.PadRight(100, '/'));
            Console.WriteLine("Instuctions:");
            Console.WriteLine("1 - Add data to the database using a file (*.csv)");
            Console.WriteLine("2 - Show books from database by filter");
            Console.WriteLine("3 - Write data from database to file (*.csv)");
            Console.WriteLine("4 - Clear database");
            Console.WriteLine("5 - Close program");
            Console.WriteLine(String.Empty.PadRight(100, '/'));
            Console.WriteLine();
        }
        public static void WriteInConsoleBooks(List<Book> books)
        {
            if (books.Count == 0)
            {
                Console.WriteLine("Database is empty");
            }

            foreach (var book in books)
            {
                Console.WriteLine(book);
            }
        }

        public static async Task AddDataFromFile(ControlerDB controlerDB)
        {
            Console.WriteLine("Write the path to the file:");
            string? filePath = Console.ReadLine();
            ResultsReadingCSV resultsReadingCSV = await controlerDB.ReadCSVFile(filePath);
            if (resultsReadingCSV.Success)
            {
                AddedDataCounter? addedeNewData = await controlerDB.AddDataToDataBase(resultsReadingCSV);
                Console.WriteLine($"Total correct row in file ({resultsReadingCSV.Data.Count})");
                Console.WriteLine($"Incorrect row in file ({String.Join(",", resultsReadingCSV.IncorrectRow)})");
                Console.WriteLine("Added new data:");
                Console.WriteLine($"Book - {addedeNewData.NewBook}, Genre - {addedeNewData.NewGenre}, " +
                    $"Author - {addedeNewData.NewAuthor}, Publisher - {addedeNewData.NewPublisher}");
            }
            else
            {
                Console.WriteLine(resultsReadingCSV.ErrorMessage);
            }
        }
        public static async Task ShowDataFromDataBase(ControlerDB controlerDB)
        {
            List<Book> books = await controlerDB.GetBooksByFilter();
            WriteInConsoleBooks(books);
        }
        public static async Task CreateFileFromDataBase(ControlerDB controlerDB)
        {
            Console.WriteLine("Write the file directory:");
            string? fileDirectory = Console.ReadLine();
            Console.WriteLine("Write the file name:");
            string? fileName = Console.ReadLine();
            List<Book> booksTofile = await controlerDB.GetBooksByFilter();
            ResultsCreatingCSV resultsCreatingCSV = await controlerDB.CreateCSVFile(fileDirectory, fileName, booksTofile);
            if (resultsCreatingCSV.Success)
            {
                Console.WriteLine($"CSV file '{resultsCreatingCSV.FilePath}' was successfully created from the list.");
            }
            else
            {
                Console.WriteLine(resultsCreatingCSV.ErrorMessage);
            }
        }
        public static async Task ClearDataBase(ControlerDB controlerDB)
        {
            await controlerDB.ClearAllDataInTables();
            Console.WriteLine("Database is clear");
        }
    }
}
