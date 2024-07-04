namespace Xpense.Resources.Database
{
    internal class IdentityModel : IIdentityModel
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
    }
}
