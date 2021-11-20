namespace DataProcessing.Inputs
{
    public interface IInput : ISource
    {
        bool HasData();
        
        string[] GetData();
    }
}