namespace IdentityServer.Models.Account
{
    public class ExternalProvider
    {
        public ExternalProvider()
        {
        }

        public ExternalProvider(string authenticationScheme)
        {
            AuthenticationScheme = authenticationScheme;
        }

        public string DisplayName { get; set; }

        public string AuthenticationScheme { get; set; }

        public string IconUrl => $"/img/ext/{AuthenticationScheme}.png";
    }
}