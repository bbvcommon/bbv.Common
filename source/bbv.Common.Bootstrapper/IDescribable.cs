namespace bbv.Common.Bootstrapper
{
    public interface IDescribable
    {
        string Name { get; }

        string Describe();
    }
}