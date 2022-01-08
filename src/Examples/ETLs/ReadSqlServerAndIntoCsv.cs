using DataProcessing;
using DataProcessing.Inputs;
using DataProcessing.Outputs;
using DataProcessing.Transformations;

namespace Examples.ETLs
{
    public static class ReadSqlServerAndIntoCsv
    {
        //Efetua uma consulta em um banco de dados sql server
        //E gera um arquivo com o resulta

        public static void Run()
        {
            //Extract
            //Define o input da ETL

            string connectionString = "Server=localhost,1433;Database=dbDataProcessing;User Id=sa;Password=MyPass@word;";
            string query = "select * from movies;";
            IInput input = new SqlServerInput(connectionString, query);
            
            //Transformation
            //Pula o cabeçalho do arquivo

            string filename = @"D:\Source\DataSets\Outputs\movies.csv";
            File.Delete(filename);
            Stream stream = File.Open(filename, FileMode.Create, FileAccess.Write);
            char delimiter = ',';
            IOutput output = new TextFileOutput(stream, delimiter);
            
            //Execute ETL
            Processor processor = new Processor(input, output);
            processor.Track += Tracking.Track;
            processor.Run();
            
            //Resultados
            //Rows : 61.934
            //Time : 01:32
            //R\S  : 673
            //Gen0 : 78
            //Gen1 : 7
            //Gen2 : 0
            //Memory : 33 mb
        }
    }
}