using System.Diagnostics;

namespace DataProcessing
{
    public abstract class ProcessorTrack
    {
        public delegate void TrackHandler(object sender, TrackArgs args);

        public event TrackHandler Track;
        
        protected int _readLines;

        private int[] _previousCollections;

        private Stopwatch _stopwatch;

        public ProcessorTrack()
        {
            Track = default!;
            _readLines = 0;
            _stopwatch = new Stopwatch();
            _previousCollections = new int[]
            {
                GC.CollectionCount(0), 
                GC.CollectionCount(1), 
                GC.CollectionCount(3) 
            };
        }

        protected void StartTrack()
        {
            _stopwatch.Start();
        }

        protected void StopTrack()
        {
            _stopwatch.Stop();
        }

        protected void OnTrack()
        {
            TrackHandler handler = Track;
            if(handler != null)
            {
                var args = new TrackArgs(
                    ++_readLines,
                    GC.CollectionCount(0) - _previousCollections[0],
                    GC.CollectionCount(1) - _previousCollections[1],
                    GC.CollectionCount(2) - _previousCollections[2],
                    _stopwatch.ElapsedMilliseconds,
                    Process.GetCurrentProcess().WorkingSet64 / 1024 / 1024
                );

                handler(this, args);
            }
        }
        
    }
}