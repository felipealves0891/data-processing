namespace DataProcessing.Transformations
{
    public interface ITransformation
    {
        string[] Transform(string[] data);       
    }
}