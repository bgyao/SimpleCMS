namespace SimpleCMS.Permissions;

public static class SimpleCMSPermissions
{
    public const string GroupName = "SimpleCMS";
    public const string BookStoreGroup = "BookStore";
    public const string AuthorsGroup = "Authors";

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";

    #region Book Permissions
    public static class Books
    {
        public const string Default = BookStoreGroup + ".Books";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
    #endregion

    #region Author Permissions
    public static class Authors
    {
        public const string Default = AuthorsGroup + ".Authors";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
    #endregion

    #region CmsContent Permissions
    public static class CmsContentsAdminPolicies
    {
        public const string Default = GroupName + ".CmsContents";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
    #endregion
}
