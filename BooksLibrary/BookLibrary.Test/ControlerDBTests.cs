using BooksLibrary.BL;
using BooksLibrary.DB;
using Microsoft.EntityFrameworkCore;

namespace BooksLibrary.Test
{
    public class ControlerDBTests
    {
        private string _directoryWithTestFiles { get; set; }
        private ControlerDB _controlerDB { get; set; }

        [SetUp]
        public void Setup()
        {
            BooksLibraryDBContext db = new BooksLibraryDBContext(new DbContextOptionsBuilder<BooksLibraryDBContext>().Options);
            _controlerDB = new ControlerDB(db);
            string currentDirectory = Environment.CurrentDirectory;
            string workingDirectory = Directory.GetParent(currentDirectory).Parent.FullName;
            _directoryWithTestFiles = Directory.GetParent(workingDirectory).Parent.FullName + @"\BookLibrary.Test\TestFiles";
        }

        [Test]
        public async Task ReadCSVFileEmptyFileExpectsFalse()
        {
            ResultsReadingCSV resultsReadingCSV = await _controlerDB.ReadCSVFile(_directoryWithTestFiles + @"\EmptyFile.csv");
            Assert.IsFalse(resultsReadingCSV.Success);
        }

        [Test]
        public async Task ReadCSVFileNormalFileExpectsTrue()
        {
            ResultsReadingCSV resultsReadingCSV = await _controlerDB.ReadCSVFile(_directoryWithTestFiles + @"\books.csv");
            Assert.That(resultsReadingCSV.Success, Is.EqualTo(true));
            Assert.That(resultsReadingCSV.Columns.Count, Is.EqualTo(6));
            Assert.That(resultsReadingCSV.Data.Count, Is.EqualTo(42));
            Assert.That(resultsReadingCSV.IncorrectData.Count, Is.EqualTo(5));
        }

        [Test]
        public async Task ReadCSVFileIncorrectFileExpectsFalse()
        {
            ResultsReadingCSV resultsReadingCSV = await _controlerDB.ReadCSVFile(_directoryWithTestFiles + @"\IncorrectFile.csv");
            Assert.IsFalse(resultsReadingCSV.Success);
        }
    }
}