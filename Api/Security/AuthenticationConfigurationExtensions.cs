using Domain.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Threading.Tasks;
using static System.Text.Encoding;

namespace Api.Security
{
    static class AuthenticationConfigurationExtensions
    {
        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthorization(options =>
                {
                    var containsDebtorToken = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme).RequireClaim(ClientApiSecurity.Claims.UserToken).Build();

                    options.AddPolicy(ClientApiSecurity.Policies.ContainsUserToken, containsDebtorToken);
                })
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    var authOptions = new AuthOptions();
                    configuration.GetSection("Auth").Bind(authOptions);

                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,

                        IssuerSigningKey = new SymmetricSecurityKey(ASCII.GetBytes(authOptions.Key)),
                        ValidateIssuerSigningKey = true,

                        ValidateLifetime = authOptions.Lifetime.HasValue,
                        RequireExpirationTime = false,
                        RequireSignedTokens = true,
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Request.Query.TryGetValue("access_token", out var values))
                                context.Token = values.First();

                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}
