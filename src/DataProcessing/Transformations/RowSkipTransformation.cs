namespace DataProcessing.Transformations
{
    public class RowSkipTransformation : ITransformation
    {
        private readonly int _lines;

        private int _reads;

        public RowSkipTransformation(int lines)
        {
            _lines = lines;
            _reads = 0;
        }

        public string[] Transform(string[] data)
        {
            if(_lines > _reads){
                _reads++;
                return null;
            }
                

            return data;
        }
    }
}