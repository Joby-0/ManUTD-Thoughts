using Scalar.AspNetCore;
using DbContext;
using DbContext.Extensions;
using DbRepos;
using Services;
using Configuration.Extensions;
using Microsoft.EntityFrameworkCore;
using Configuration;

var builder = WebApplication.CreateBuilder(args);

// NOTE: global cors policy needed for JS and React frontends
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Configuration.AddSecrets("ManUTDAPI");

builder.Services.AddJwtTokenService(builder.Configuration);
builder.Services.AddMainDbContext(builder.Configuration);
builder.Services.AddEncryptions(builder.Configuration);

// Add services to the container.
builder.Services.AddHttpClient<IFotbollService, FotbollService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<AuthRepo>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
