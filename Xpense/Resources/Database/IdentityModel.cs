namespace Xpense.Resources.Database
{
    public class IdentityModel : IIdentityModel
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
    }
}
