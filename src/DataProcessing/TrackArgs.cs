namespace DataProcessing
{
    public class TrackArgs
    {
        public int Lines { get; private set; }

        public int ReadPerSecond { get; private set; }

        public int CollectionCountG0 { get; private set; }

        public int CollectionCountG1 { get; private set; }

        public int CollectionCountG2 { get; private set; }

        public long ElapsedTimeInMileseconds { get; private set; }

        public long ElapsedTimeInSeconds { get; private set; }

        public long MemoryUsed { get; private set; }

        public TrackArgs(
            int lines,
            int collectionCountG0,
            int collectionCountG1,
            int collectionCountG2,
            long elapsedTimeInMileseconds,
            long memoryUsed
        )
        {
            CollectionCountG0 = collectionCountG0;
            CollectionCountG1 = collectionCountG1;
            CollectionCountG2 = collectionCountG2;
            ElapsedTimeInMileseconds = elapsedTimeInMileseconds;
            ElapsedTimeInSeconds = elapsedTimeInMileseconds / 1000;
            MemoryUsed = memoryUsed;
            Lines = lines;
            ReadPerSecond = ElapsedTimeInSeconds > 0 ? Lines / (int)ElapsedTimeInSeconds : 0; 
        }
    }
}