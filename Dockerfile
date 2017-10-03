FROM microsoft/aspnetcore-build:1 AS build-image

# We copy .csproj first, as they don't change as much.
WORKDIR /app
COPY ./VagrantAtlas/*.csproj ./VagrantAtlas/
COPY ./VagrantAtlas.UnitTests/*.csproj ./VagrantAtlas.UnitTests/
COPY ./*.sln ./

# dotnet restore is run in its own layer
# (to create a cache of package references)
# NOTE:
#   Watch out if you use version ranges in package references:
#   they won't update as you might expect if you have a docker layer cached.
RUN dotnet restore

COPY ./ ./

ARG version=0.0.1-dev
RUN dotnet build \
      --configuration Release \
      /property:Version=${version} && \
    dotnet test ./VagrantAtlas.UnitTests/VagrantAtlas.UnitTests.csproj \
      --configuration Release \
      --no-build && \
    dotnet publish ./VagrantAtlas/VagrantAtlas.csproj \
      --configuration Release \
      --output /app/out


# Production container, reusing output from previous container (build)
FROM microsoft/aspnetcore:1

WORKDIR /atlas

COPY --from=build-image /app/out .

RUN groupadd atlas && \
    useradd -r -s /bin/false atlas -g atlas && \
    mkdir -p /atlas/data && \
    chown atlas:atlas /atlas/data && \
    find . -type d -exec chmod 0755 {} \; && \
    find . -type f -exec chmod 0644 {} \; && \
    chmod -R 775 /atlas/data

ENV ASPNETCORE_URLS=http://+:5000 \
    ATLAS_BOXES_PATH=/atlas/data/boxes.json \
    ATLAS_CLIENT_ID=vagrant \
    ATLAS_SECRET=vagrant

VOLUME /atlas/data
USER atlas
EXPOSE 5000

ENTRYPOINT ["dotnet", "VagrantAtlas.dll"]
