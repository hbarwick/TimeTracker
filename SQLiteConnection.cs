
namespace CodingTracker
{
    internal class SQLiteConnection : IDisposable
    {
        private string? connectionString;

        public SQLiteConnection(string? connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}