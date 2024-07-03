using AutoCorrectScoreLuckyDraw.Service;
using AutoCorrectScoreLuckyDraw.Service.Services;
using CorrectScoreLuckyDraw.Models;
using Fanex.Data.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFanexSql(builder.Configuration);

builder.Services.Configure<AppSettingsKey>(builder.Configuration);

builder.Services.InitializeServices(builder.Configuration);

builder.Services.AddHostedService<LuckyDrawService>();

var app = builder.Build();

app.Run(async context =>
{
    await context.Response.WriteAsync("OK");
});

app.Run();