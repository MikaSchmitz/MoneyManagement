namespace Xpense.Resources.Database
{
    public interface IIdentityModel
    {
        Guid Id { get; set; }
        DateTime Created { get; set; }
    }
}
