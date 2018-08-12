FROM microsoft/dotnet:2.1-sdk AS builder

WORKDIR /app

COPY . .
RUN dotnet publish ./src -c Release -o dist

FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app

COPY --from=builder /app/src/Api/dist ./

ENTRYPOINT dotnet Products.Api.dll