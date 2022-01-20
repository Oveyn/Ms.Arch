#docker build -t oveyn/ms-arch-hw01-api -f Dockerfile ..

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS publish
ARG VERSION_APP
WORKDIR /src
COPY ["src/Ms.Arch.Hw01.Api/Ms.Arch.Hw01.Api.csproj", "Ms.Arch.Hw01.Api/"]

RUN dotnet restore "./Ms.Arch.Hw01.Api/Ms.Arch.Hw01.Api.csproj" --runtime alpine-x64
COPY ["src/Ms.Arch.Hw01.Api/.", "Ms.Arch.Hw01.Api/."]
RUN dotnet publish "./Ms.Arch.Hw01.Api/Ms.Arch.Hw01.Api.csproj" -c Release -o /app/publish \
  --no-restore \
  --runtime alpine-x64 \
  --self-contained true \
  /p:PublishTrimmed=true \
  /p:PublishSingleFile=true \
  /p:Version="${VERSION_APP}"

FROM mcr.microsoft.com/dotnet/runtime-deps:5.0-alpine AS final

RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app

# upgrade musl to remove potential vulnerability
RUN apk upgrade musl

USER dotnetuser
WORKDIR /app
EXPOSE 5000
COPY --from=publish /app/publish .

ENTRYPOINT ["./Ms.Arch.Hw01.Api", "--urls", "http://0.0.0.0:5000"]
