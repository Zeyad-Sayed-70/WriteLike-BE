using WriteLike.Api.Features;
using WriteLike.Composition;

var builder = WebApplication.CreateBuilder(args);

// The ONLY line in Api that knows Infrastructure + Ai exist — via the composition root.
builder.Services.AddWriteLike();

var app = builder.Build();

app.MapHealthEndpoints();

app.Run();
