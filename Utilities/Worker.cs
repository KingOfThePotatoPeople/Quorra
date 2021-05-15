using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;
using Quorra.EntityFramework;

namespace Quorra.Utilities
{
    public class Worker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public Worker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using IServiceScope scope = _serviceProvider.CreateScope();

            QuorraDbContext context = scope.ServiceProvider.GetRequiredService<QuorraDbContext>();
            await context.Database.EnsureCreatedAsync(cancellationToken);

            IOpenIddictApplicationManager manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            if (await manager.FindByClientIdAsync("console", cancellationToken) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "console",
                    ClientSecret = "388D45FA-B36B-4988-BA59-B187D329C207",
                    DisplayName = "My client application",
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                        OpenIddictConstants.Permissions.Prefixes.Scope + "api"
                    }
                }, cancellationToken);
            }
            
            if (await manager.FindByClientIdAsync("postman", cancellationToken) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "postman",
                    ClientSecret = "postman-secret",
                    DisplayName = "Postman",
                    RedirectUris = { new Uri("https://oauth.pstmn.io/v1/callback")},
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        
                        OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                        OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                        
                        OpenIddictConstants.Permissions.Prefixes.Scope + "api",
                        OpenIddictConstants.Permissions.ResponseTypes.Code
                    }
                }, cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}