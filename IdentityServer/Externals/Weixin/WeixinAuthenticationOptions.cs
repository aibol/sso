/*
 * Licensed under the Apache License, Version 2.0 (http://www.apache.org/licenses/LICENSE-2.0)
 * See https://github.com/aspnet-contrib/AspNet.Security.OAuth.Providers
 * for more information concerning the license and the contributors participating to this project.
 */

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace IdentityServer.Externals.Weixin
{
    /// <summary>
    /// Defines a set of options used by <see cref="WeixinAuthenticationHandler"/>.
    /// </summary>
    public class WeixinAuthenticationOptions : OAuthOptions
    {
        public WeixinAuthenticationOptions()
        {
            ClaimsIssuer = WeixinAuthenticationDefaults.Issuer;
            CallbackPath = new PathString(WeixinAuthenticationDefaults.CallbackPath);

            AuthorizationEndpoint = WeixinAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = WeixinAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = WeixinAuthenticationDefaults.UserInformationEndpoint;

            Scope.Add("snsapi_login");
            Scope.Add("snsapi_userinfo");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "unionid");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "nickname");
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "sex");
            ClaimActions.MapJsonKey(ClaimTypes.Country, "country");
            ClaimActions.MapJsonKey(WeixinAuthenticationConstants.Claims.OpenId, "openid");
            ClaimActions.MapJsonKey(WeixinAuthenticationConstants.Claims.Province, "province");
            ClaimActions.MapJsonKey(WeixinAuthenticationConstants.Claims.City, "city");
            ClaimActions.MapJsonKey(WeixinAuthenticationConstants.Claims.HeadImgUrl, "headimgurl");
            ClaimActions.MapCustomJson(WeixinAuthenticationConstants.Claims.Privilege, user =>
            {
                var value = user.Value<JArray>("privilege");
                if (value == null)
                {
                    return null;
                }

                return string.Join(",", value.ToObject<string[]>());
            });
        }
    }
}
