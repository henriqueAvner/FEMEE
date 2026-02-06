namespace FEMEE.Domain.Enums
{
    [Flags]
    public enum Permissions
    {
        None = 0,
        View = 1,

        EditProfile = 2,
        EditPublication = 4,
        EditTeam = 8,
        Admin = 16

        
    }
}