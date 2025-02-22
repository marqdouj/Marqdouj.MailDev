var builder = DistributedApplication.CreateBuilder(args);

var mailDevUsername = builder.AddParameter("maildev-username");
var mailDevPassword = builder.AddParameter("maildev-password");
var maildev = builder.AddMailDev(
    name: "maildev",
    userName: mailDevUsername,
    password: mailDevPassword);

var apiService = builder.AddProject<Projects.Marqdouj_MailDev_ApiService>("apiservice")
    .WithReference(maildev);

builder.AddProject<Projects.Marqdouj_MailDev_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithReference(maildev);

builder.Build().Run();
