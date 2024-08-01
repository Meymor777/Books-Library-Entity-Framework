using Microsoft.VisualBasic.FileIO;
using BooksLibrary.DB;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BooksLibrary.BL
{
    public class ControlerDB : IControlerDB
    {
        private readonly BooksLibraryDBContext _dBContext;
        private readonly AuthorRepository _authorRepository;
        private readonly BookRepository _bookRepository;
        private readonly GenreRepository _genreRepository;
        private readonly PublisherRepository _publisherRepository;

        public ControlerDB(BooksLibraryDBContext dBContext)
        {
            _dBContext = dBContext;
            _authorRepository = new AuthorRepository(_dBContext);
            _bookRepository = new BookRepository(_dBContext);
            _genreRepository = new GenreRepository(_dBContext);
            _publisherRepository = new PublisherRepository(_dBContext);
        }

        public async Task<ResultsReadingCSV> ReadCSVFile(string? filePath)
        {

            ResultsReadingCSV resultsReadingCSV = new ResultsReadingCSV();
            FileControler fileControler = new FileControler();
            (bool isCorrect, string errorMessage) resultOfCheckingFile = fileControler.IsFileCorrect(filePath);
            if (!resultOfCheckingFile.isCorrect)
            {
                resultsReadingCSV.ErrorMessage = resultOfCheckingFile.errorMessage;
                return (resultsReadingCSV);
            }

            using (TextFieldParser parser = new TextFieldParser(filePath))
            {
                resultsReadingCSV.Success = true;
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");
                int indRow = 0;
                while (!parser.EndOfData)
                {
                    //Processing row
                    indRow++;
                    string[]? fields = parser.ReadFields();
                    if (fields.Length != 6)
                    {
                        resultsReadingCSV.IncorrectRow.Add(indRow);
                        resultsReadingCSV.IncorrectData.Add(new List<string>(fields));
                        if (indRow == 1)
                        {
                            resultsReadingCSV.Success = false;
                            resultsReadingCSV.ErrorMessage = "Inccorect columns in file";
                            return (resultsReadingCSV);
                        }
                    }

                    if (indRow == 1)
                    {
                        resultsReadingCSV.Columns.AddRange(fields);
                    }
                    else
                    {
                        resultsReadingCSV.Data.Add(new List<string>(fields));
                    }

                }
            }
            return (resultsReadingCSV);
        }
        public async Task<AddedDataCounter> AddDataToDataBase(ResultsReadingCSV resultsReadingCSV)
        {
            AddedDataCounter addedNewData = new AddedDataCounter();

            foreach (var dataRow in resultsReadingCSV.Data)
            {
                string bookTitle = dataRow[0];
                bool isCorrectConvertingPages = Int32.TryParse(dataRow[1], out int bookPages);
                string genreName = dataRow[2];
                bool isCorrectConvertingReleaseDate = DateTime.TryParse(dataRow[3], out DateTime bookReleaseDate);
                string authorName = dataRow[4];
                string publisherName = dataRow[5];


                Genre genre = await _genreRepository.Get(genreName);
                if (genre == null)
                {
                    Guid genreID = Guid.NewGuid();
                    await _genreRepository.Add(genreID, genreName);
                    genre = await _genreRepository.Get(genreID);
                    addedNewData.NewGenre++;
                }


                Author author = await _authorRepository.Get(authorName);
                if (author == null)
                {
                    Guid authorID = Guid.NewGuid();
                    await _authorRepository.Add(authorID, authorName);
                    author = await _authorRepository.Get(authorID);
                    addedNewData.NewAuthor++;
                }


                Publisher publisher = await _publisherRepository.Get(publisherName);
                if (publisher == null)
                {
                    Guid publisherID = Guid.NewGuid();
                    await _publisherRepository.Add(publisherID, publisherName);
                    publisher = await _publisherRepository.Get(publisherID);
                    addedNewData.NewPublisher++;
                }


                Book book = await _bookRepository.Get(bookTitle);
                if (book == null && isCorrectConvertingPages && isCorrectConvertingReleaseDate)
                {
                    if (genre != null && author != null && publisher != null)
                    {
                        await _bookRepository.Add(Guid.NewGuid(), bookTitle, bookPages, genre, author, publisher, bookReleaseDate);
                        addedNewData.NewBook++;
                    }
                }

            }
            return addedNewData;
        }

        public async Task<List<Book>> GetBooksByFilter()
        {
            List<Book> currentTable = await _bookRepository.Get();
            Filter filter = await GetFilter();
            return currentTable
                .Where(book => filter.Title == null || book.Title == filter.Title)
                .Where(book => filter.Genre == null || (book.Genre != null && book.Genre.Name == filter.Genre))
                .Where(book => filter.Author == null || (book.Author != null && book.Author.Name == filter.Author))
                .Where(book => filter.Publisher == null || (book.Publisher != null && book.Publisher.Name == filter.Publisher))
                .Where(book => filter.MoreThanPages == null || book.Pages > filter.MoreThanPages)
                .Where(book => filter.LessThanPages == null || book.Pages <= filter.LessThanPages)
                .Where(book => filter.PublishedBefore == null || book.ReleaseDate <= filter.PublishedBefore)
                .Where(book => filter.PublishedAfter == null || book.ReleaseDate > filter.PublishedAfter)
                .ToList();
        }
        private async Task<Filter> GetFilter()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            IConfiguration config = builder.Build();

            string parentSection = "QueryBooks";

            return new Filter(config, parentSection);
        }
        public async Task<ResultsCreatingCSV> CreateCSVFile(string fileDirectory, string fileName, List<Book> books)
        {
            string filePath = $@"{fileDirectory}\{fileName}.csv";
            ResultsCreatingCSV resultsCreatingCSV = new ResultsCreatingCSV();
            FileControler fileControler = new FileControler();
            (bool isCorrect, string errorMessage) resultOfCheckingFile = fileControler.IsFileCorrect(filePath);
            if (resultOfCheckingFile.isCorrect)
            {
                resultsCreatingCSV.ErrorMessage = "This file already exists";
                return (resultsCreatingCSV);
            }
            if (!fileControler.IsDirectoryCorrect(fileDirectory))
            {
                resultsCreatingCSV.ErrorMessage = "Directory is not exist";
                return (resultsCreatingCSV);
            }

            resultsCreatingCSV.FilePath = filePath;
            resultsCreatingCSV.Success = true;
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
            foreach (var book in books)
            {
                data.Add(new Dictionary<string, object> {
                    { "Title", book.Title },
                    { "Pages", book.Pages },
                    { "Genre", book.Genre.ToString() },
                    { "ReleaseDate", $"{book.ReleaseDate.Year}-{book.ReleaseDate.Month}-{book.ReleaseDate.Day}" },
                    { "Author", book.Author.ToString() },
                    { "Publisher", book.Publisher.ToString() } });
            }

            // Write the data to the CSV file
            using (var writer = new StreamWriter(filePath))
            {
                // Write the header row
                string header = string.Join(",", data[0].Keys) + "\n";
                writer.Write(header);

                // Write each data row
                foreach (var row in data)
                {
                    string rowStr = string.Join(",", row.Values) + "\n";
                    writer.Write(rowStr);
                }
            }
            return (resultsCreatingCSV);
        }

        public async Task ClearAllDataInTables()
        {
            await _bookRepository.DeleteAll();
            await _authorRepository.DeleteAll();
            await _genreRepository.DeleteAll();
            await _publisherRepository.DeleteAll();
        }

        public ControlerDB GetControlerDB(BooksLibraryDBContext dBContext)
        {
            return new ControlerDB(dBContext);
        }
    }
}
