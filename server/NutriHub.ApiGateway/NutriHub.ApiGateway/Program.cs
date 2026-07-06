var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .ConfigureHttpClient((context, handler) =>
     {
         handler.SslOptions.RemoteCertificateValidationCallback =
             (sender, cert, chain, errors) => true;
     });

var app = builder.Build();

app.MapReverseProxy();

app.Run();
