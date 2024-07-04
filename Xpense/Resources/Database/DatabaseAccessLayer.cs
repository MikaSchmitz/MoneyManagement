using SQLite;

namespace Xpense.Resources.Database
{
    internal abstract class DatabaseAccessLayer<TEntity> where TEntity : IdentityModel, new()
    {
        private SQLiteAsyncConnection? _database;

        protected DatabaseAccessLayer()
        {
            Init().Wait();
        }

        private async Task Init()
        {
            if (_database != null)
                return;

            _database = new SQLiteAsyncConnection(DatabaseConstants.DatabasePath, DatabaseConstants.Flags);
            await _database.CreateTableAsync<TEntity>();
        }

        protected SQLiteAsyncConnection Database
        {
            get
            {
                if (_database == null)
                    throw new InvalidOperationException("Database not initialized");
                return _database;
            }
        }

        public Task<List<TEntity>> GetAllAsync()
        {
            return Database.Table<TEntity>().ToListAsync();
        }

        public Task<TEntity> GetByIdAsync(int id)
        {
            return Database.FindAsync<TEntity>(id);
        }

        public Task<int> SaveAsync(TEntity entity)
        {
            if (entity.Id != Guid.Empty)
            {
                return Database.UpdateAsync(entity);
            }
            else
            {
                return Database.InsertAsync(entity);
            }
        }

        public Task<int> DeleteAsync(TEntity entity)
        {
            return Database.DeleteAsync(entity);
        }
    }
}
