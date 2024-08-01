using BooksLibrary.BL;

namespace BooksLibrary.Test
{
    public class FileControlerTests
    {

        [SetUp]
        public void Setup()
        {
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("123")]
        public void IsFileCorrectExpectsFalse(string filePath)
        {
            FileControler fileControler = new FileControler();
            Assert.IsFalse(fileControler.IsFileCorrect(filePath).isCorrect);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("123")]
        public void IsDirectoryCorrectExpectsFalse(string directoryPath)
        {
            FileControler fileControler = new FileControler();
            Assert.IsFalse(fileControler.IsDirectoryCorrect(directoryPath));
        }

    }
}
