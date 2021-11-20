namespace DataProcessing.Outputs
{
    public interface IOutput : ISource
    {
        void Set(string[] data);
    }
}