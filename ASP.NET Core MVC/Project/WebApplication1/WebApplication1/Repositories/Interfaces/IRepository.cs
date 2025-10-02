namespace WebApplication1.Repositories.Interfaces
{
    // Convenience interface combining both read & write.
    // Use sparingly — prefer injecting only the subset that controllers need.
    public interface IRepository<T> : IReadableRepository<T>, IWritableRepository<T> where T : class
    {
    }
}