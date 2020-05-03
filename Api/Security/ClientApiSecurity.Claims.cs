namespace Api.Security
{
    static partial class ClientApiSecurity
    {
        public static class Claims
        {
            public const string UserToken = nameof(UserToken);

            public const string CanImportData = nameof(CanImportData);
        }
    }
}
