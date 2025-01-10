var builder = DistributedApplication.CreateBuilder(args);

var keycloakAdminUser = builder.AddParameter("keycloakAdminUser", "admin", publishValueAsDefault: true);
var keycloakAdminPassword = builder.AddParameter("keycloakAdminPassword", secret: true);

var keycloak = builder.AddKeycloak("keycloak", 8080, keycloakAdminUser, keycloakAdminPassword)
                      .WithArgs("--verbose")
                      .WithLifetime(ContainerLifetime.Persistent)
                      .WithRealmImport(@"../keycloak")
                      .WithDataVolume();

var apiService = builder.AddProject<Projects.SetListr_ApiService>("apiservice")
                        .WithReference(keycloak)
                        .WaitFor(keycloak);

builder.AddProject<Projects.SetListr_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(keycloak)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
