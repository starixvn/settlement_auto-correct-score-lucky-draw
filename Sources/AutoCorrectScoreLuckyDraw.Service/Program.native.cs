using AutoCorrectScoreLuckyDraw.Service;
using AutoCorrectScoreLuckyDraw.Service.Services;
using Core.Models;
using Fanex.Data.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddFanexSql(builder.Configuration);

services.Configure<AppSettingsKey>(builder.Configuration);

services.InitializeServices(builder.Configuration);

services.AddHostedService<LuckyDrawService>();

services.AddHostedService<RebateRetentionService>();

services.AddHealthChecks();

var app = builder.Build();
app.MapHealthChecks("/healthz");

await app.RunAsync();