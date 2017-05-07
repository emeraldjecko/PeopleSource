using System.Linq;
using System.Security.Principal;
using System.Web.Security;
using PeoplesSource.Domain;

namespace PeoplesSource.Extensions
{
    /// <summary>
    /// Class IPrincipalExtensions.
    /// </summary>
    public static class IPrincipalExtensions
    {
        /// <summary>
        /// Determines whether the specified user has roles.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns><c>true</c> if the specified user has roles; otherwise, <c>false</c>.</returns>
        public static bool HasRoles(this IPrincipal user)
        {
            return new RolePrincipal(user.Identity).GetRoles().Any();
        }

        /// <summary>
        /// Determines whether [is in role] [the specified user].
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="role">The role.</param>
        /// <returns><c>true</c> if [is in role] [the specified user]; otherwise, <c>false</c>.</returns>
        public static bool IsInRole(this IPrincipal user, RoleEnum role)
        {
            return user.IsInRole(role.ToString());
        }

        /// <summary>
        /// Determines whether the specified user is administrator.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns><c>true</c> if the specified user is administrator; otherwise, <c>false</c>.</returns>
        public static bool IsAdministrator(this IPrincipal user)
        {
            return user.IsInRole(RoleEnum.Administrator);
        }
       
    }
}