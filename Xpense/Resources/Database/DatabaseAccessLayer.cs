using SQLite;

namespace Xpense.Resources.Database
{
    public abstract class DatabaseAccessLayer<TEntity> where TEntity : IdentityModel, new()
    {
        /// <summary>
        /// The database
        /// </summary>
        protected SQLiteAsyncConnection Database;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseAccessLayer{TEntity}"/> class.
        /// </summary>
        protected DatabaseAccessLayer()
        {
            Database = GetDatabaseConnection();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        protected async Task Init()
        {
            if (Database != null)
                return;

            Database = GetDatabaseConnection();
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

        private SQLiteAsyncConnection GetDatabaseConnection()
        {
            return new SQLiteAsyncConnection(DatabaseConstants.DatabasePath, DatabaseConstants.Flags);
        }
    }
}
