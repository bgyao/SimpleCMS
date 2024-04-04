namespace SimpleCMS.Permissions;

public static class SimpleCMSPermissions
{
    public const string GroupName = "SimpleCMS";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";

    #region Book Permissions
    public static class Books
    {
        public const string Default = GroupName + ".Books";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
    #endregion
}
