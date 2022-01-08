using DataProcessing;
using DataProcessing.Inputs;
using DataProcessing.Outputs;
using DataProcessing.Transformations;

namespace Examples.ETLs
{
    public static class ReadCsvAndIntoSqlServer
    {
        //Le um arquivo CSV que contem 3 colunas 
        //E efetua a carga em um banco sql server
        
        public static void Run()
        {
            //Extract
            //Define o input da ETL

            string filename = @"D:\Source\DataSets\ml-25m\movies.csv";
            Stream stream = File.Open(filename, FileMode.Open, FileAccess.Read);
            char delimiter = ',';
            IInput input = new TextFileInput(stream, delimiter);

            //Transformation
            //Pula o cabeçalho do arquivo

            int lines = 1;
            RowSkipTransformation rowSkip = new RowSkipTransformation(lines);

            //Load
            //Define o output da ETL

            string connectionString = "Server=localhost,1433;Database=dbDataProcessing;User Id=sa;Password=MyPass@word;";
            string query = "insert into [movies] (movieId,title,genres) values (@1, @2, @3)";
            string[] parameterIdentifier = new string[] {"@1", "@2", "@3"};
            int lote = 5000;
            IOutput output = new SqlServerOutput(connectionString, query, parameterIdentifier, lote);

            //Execute ETL
            Processor processor = new Processor(input, output);
            processor.AddTransformation(rowSkip);
            processor.Track += Traking;
            processor.Run();
        }

        private static void Traking(object sender, TrackArgs e)
        {
            var time = (e.ElapsedTimeInSeconds / 60).ToString("0#") + ":" + (e.ElapsedTimeInSeconds % 60).ToString("0#");
            Console.Clear();
            Console.WriteLine($"\nRows : {e.Lines.ToString("n0")}");
            Console.WriteLine($"Time : {time}");
            Console.WriteLine($"R\\S  : {e.ReadPerSecond}");
            Console.WriteLine($"Gen0 : {e.CollectionCountG0}");
            Console.WriteLine($"Gen1 : {e.CollectionCountG1}");
            Console.WriteLine($"Gen2 : {e.CollectionCountG2}");
            Console.WriteLine($"Memory : {e.MemoryUsed} mb");
        }

    }
}