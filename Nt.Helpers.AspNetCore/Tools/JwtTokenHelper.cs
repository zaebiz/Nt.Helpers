using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Nt.Helpers.AspNetCore.Tools
{
    /// <summary>
    /// JWT Token generation Helper for Bearere authentication scheme
    /// </summary>
    public class JwtTokenHelper : IJwtTokenHelper
    {
        private readonly string _issuer;
        private readonly string _audience;
        private readonly string _issuerSigningKey;

        public JwtTokenHelper(string issuer, string audience, string issuerSigningKey)
        {
            _issuer = issuer;
            _audience = audience;
            _issuerSigningKey = issuerSigningKey;
        }

        public JwtTokenHelper(IConfiguration config)
        {
            _issuer = config["Auth:ValidIssuer"];
            _audience = config["Auth:ValidAudience"];
            _issuerSigningKey = config["Auth:IssuerSigningKey"];
        }

        public JwtBearerOptions SetDefaultBearerOptions(JwtBearerOptions options)
        {
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_issuerSigningKey)),
                ValidateIssuerSigningKey = true
            };

            return options;
        }

        public string GetEncodedToken(IEnumerable<Claim> claims, int expiresHours)
        {
            var jwt = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                notBefore: DateTime.Now,
                claims: claims,
                expires: DateTime.Now.AddHours(expiresHours),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_issuerSigningKey)), 
                    SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }

    public interface IJwtTokenHelper
    {
        JwtBearerOptions SetDefaultBearerOptions(JwtBearerOptions options);
        string GetEncodedToken(IEnumerable<Claim> claims, int expiresHours);
    }
}
