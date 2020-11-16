FROM mcr.microsoft.com/dotnet/core/sdk:3.1
ADD ./ /app
ENV ASPNETCORE_URLS=http://*:${PORT}
WORKDIR /app
ENTRYPOINT [ "dotnet", "GEP.dll" ]