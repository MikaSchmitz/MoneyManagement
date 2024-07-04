namespace Xpense.Resources.Database
{
    internal interface IIdentityModel
    {
        Guid Id { get; set; }
        DateTime Created { get; set; }
    }
}
