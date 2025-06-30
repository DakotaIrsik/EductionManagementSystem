using System.Collections.Generic;

namespace SilverLeaf.Common.Constants
{
    public class AuthenticationConsts
    {
        public const string UserNameClaimType = "name";
        public const string SignInScheme = "Cookies";
        public const string OidcClientId = "skoruba_identity_admin";
        public const string OidcAuthenticationScheme = "oidc";
        public const string OidcResponseType = "id_token";
        public static List<string> Scopes = new List<string> { "openid", "profile", "email", "roles" };

        public const string ScopeOpenId = "openid";
        public const string ScopeProfile = "profile";
        public const string ScopeEmail = "email";
        public const string ScopeRoles = "roles";
        public const string ScopePermission = "permission";

        public const string AccountLoginPage = "Account/Login";
        public const string AccountAccessDeniedPage = "/Account/AccessDenied/";

        public const string RoleType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
        public const string NameIdentifierType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
    }
}
