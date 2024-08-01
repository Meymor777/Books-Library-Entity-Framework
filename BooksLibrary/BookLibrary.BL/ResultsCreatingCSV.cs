namespace BooksLibrary.BL
{
    public class ResultsCreatingCSV
    {
        public bool Success { get; set; }
        public string FilePath { get; set; }
        public string ErrorMessage { get; set; }

        public ResultsCreatingCSV()
        {
            Success = false;
            FilePath = "";
            ErrorMessage = "";
        }
    }
}
