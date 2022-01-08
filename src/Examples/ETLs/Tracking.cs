using DataProcessing;

namespace Examples.ETLs
{
    public static class Tracking
    {
        private static int _second = -1;

        public static void Track(object sender, TrackArgs e)
        {
            if(_second != DateTime.Now.Second)
            {
                _second = DateTime.Now.Second;
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
}