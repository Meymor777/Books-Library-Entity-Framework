namespace BooksLibrary.BL
{
    public class ResultsReadingCSV
    {
        public bool Success { get; set; }
        public List<string> Columns { get; set; }
        public List<List<string>> Data { get; set; }
        public List<int> IncorrectRow { get; set; }
        public List<List<string>> IncorrectData { get; set; }
        public string ErrorMessage { get; set; }

        public ResultsReadingCSV()
        {
            Success = false;
            Columns = new List<string>();
            Data = new List<List<string>>();
            IncorrectRow = new List<int>();
            IncorrectData = new List<List<string>>();
            ErrorMessage = "";
        }
    }
}
