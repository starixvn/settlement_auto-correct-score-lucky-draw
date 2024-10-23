FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-alpine AS builder
WORKDIR /src

COPY ./Sources/Core ./Core
COPY ./Sources/CorrectScoreLuckyDraw ./CorrectScoreLuckyDraw
COPY ./Sources/RebateRetention ./RebateRetention
COPY ./Sources/AutoCorrectScoreLuckyDraw.Service/AutoCorrectScoreLuckyDraw.Service.csproj ./AutoCorrectScoreLuckyDraw.Service/AutoCorrectScoreLuckyDraw.Service.csproj

WORKDIR /src/AutoCorrectScoreLuckyDraw.Service
RUN dotnet nuget add source https://nuget.nexdev.net/api/v2
RUN dotnet restore -v n

COPY ./Sources/AutoCorrectScoreLuckyDraw.Service .

RUN dotnet publish -r linux-x64 --self-contained false -c Release -o /app

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine AS base

WORKDIR /app
COPY --from=builder /app .

ENV TZ="America/La_Paz"
ENV COMPlus_EnableDiagnostics=0

ENTRYPOINT ["dotnet", "AutoCorrectScoreLuckyDraw.Service.dll"]
