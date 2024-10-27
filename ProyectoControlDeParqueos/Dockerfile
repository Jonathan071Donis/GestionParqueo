FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

ENV TZ=America/Guatemala
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

#EXPOSE 443

COPY ["ProyectoControlDeParqueos.csproj", "./"]
RUN dotnet restore "./ProyectoControlDeParqueos.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ProyectoControlDeParqueos.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProyectoControlDeParqueos.csproj" -c Release -o /app/publish

FROM build AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY nuevoCerti.pfx /https/nuevoCerti.pfx

ENTRYPOINT ["dotnet", "ProyectoControlDeParqueos.dll"]