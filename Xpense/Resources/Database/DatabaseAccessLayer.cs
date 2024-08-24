using SQLite;

namespace Xpense.Resources.Database
{
    public abstract class DatabaseAccessLayer<TEntity> where TEntity : IdentityModel, new()
    {
        protected SQLiteAsyncConnection Database;

        protected async Task Init()
        {
            if (Database != null)
                return;

            Database = new SQLiteAsyncConnection(DatabaseConstants.DatabasePath, DatabaseConstants.Flags);
            await Database.CreateTableAsync<TEntity>();
        }

        public async Task<int> SaveAsync(TEntity entity)
        {
            await Init();
            return await Database.InsertOrReplaceAsync(entity);
        }

        public async Task<int> DeleteAsync(TEntity entity)
        {
            await Init();
            return await Database.DeleteAsync(entity);
        }

        public async Task<List<TEntity>> GetAllAsync()
        {
            await Init();
            return await Database.Table<TEntity>().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            await Init();
            return await Database.Table<TEntity>().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
