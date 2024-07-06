using SQLite;

namespace Xpense.Resources.Database
{
    public class IdentityModel : IIdentityModel
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public DateTime Created { get; set; }

        public IdentityModel()
        {
            Id = Guid.NewGuid();
            Created = DateTime.UtcNow;
        }
    }
}
