using SQLite;

namespace Xpense.Resources.Database
{
    public abstract class DatabaseAccessLayer<TEntity> where TEntity : new()
    {
        protected SQLiteAsyncConnection Database;

        public DatabaseAccessLayer()
        {
            Init().Wait(); // Ensure the database is initialized synchronously
        }

        private async Task Init()
        {
            if (Database != null)
                return;

            Database = new SQLiteAsyncConnection(DatabaseConstants.DatabasePath, DatabaseConstants.Flags);
            await Database.CreateTableAsync<TEntity>();
        }

        public Task<int> SaveAsync(TEntity entity)
        {
            return Database.InsertOrReplaceAsync(entity);
        }

        public Task<int> DeleteAsync(TEntity entity)
        {
            return Database.DeleteAsync(entity);
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return Database.Table<TEntity>().ToListAsync();
        }
    }
}
