using IdentityServer4.Models;

namespace IdentityServer.Models
{
    // include all extra client information used in aibol systems
    public class ApplicationClient
    {
        /// <summary>
        /// Domain Uri for client
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Client Information in IdentityServer
        /// </summary>
        public Client Client { get; set; }
    }
}