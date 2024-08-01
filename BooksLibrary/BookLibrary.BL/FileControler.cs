namespace BooksLibrary.BL
{
    public class FileControler
    {
        public (bool isCorrect, string errorMessage) IsFileCorrect(string filePath)
        {
            if (filePath == null || filePath.Replace(" ", "") == "")
            {
                return (false, "File path is empty");
            }

            FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                return (false, "File doesn't exist");
            }
            else if (fileInfo.Extension != ".csv")
            {
                return (false, "File doesn't csv");
            }
            else if (fileInfo.Length == 0)
            {
                return (false, "File is empty");
            }
            return (true, "");
        }

        public bool IsDirectoryCorrect(string directoryPath)
        {
            if (directoryPath == null || directoryPath.Replace(" ", "") == "")
            {
                return false;
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);
            if (!directoryInfo.Exists)
            {
                return false;
            }

            return true;
        }
    }
}
