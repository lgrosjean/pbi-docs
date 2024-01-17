# pbi-docs

Command line utility program to generate Markdown documentation for PowerBI Semantic Model from local TMDL structure.

If no local TMDL structure is available, the tool allows the user to extract and serialize the TMDL structure to a local folder.

## Usage

### Generate documentation
```sh
Usage:
  pbi-docs [<tmdlFolderPath>] [command] [options]

Arguments:
  <tmdlFolderPath>  Directy containing the TMDL folder structure. [default: .]

Options:
  -d, --docsFolderPath <docsFolderPath>  Default directory where to save generated files. If not exist, will be created. [default: docs]
  --version                              Show version information
  -?, -h, --help                         Show help and usage information
```

### Serialize existing and published Semantic Model/Dataset

The application allow you also to extract distant TMDL structure from PowerBI Dataset.

> [!IMPORTANT]  
> To authentificate to PowerBI Service, you'll need to define the three following environment variables
> - `PBI_TENANT_ID`
> - `PBI_CLIENT_ID`
> - `PBI_CLIENT_SECRET`

```sh
Usage:
  pbi-docs [<tmdlFolderPath>] serialize [options]

Arguments:
  <tmdlFolderPath>  Directy containing the TMDL folder structure. [default: .]

Options:
  -w, --workspaceName <workspaceName>  Workspace containing Dataset to serialize
  -d, --datasetName <datasetName>      Dataset name to serialize
  -?, -h, --help                       Show help and usage information
````

## With Docker

Start by pulling the image locally
```sh
docker pull ghcr.io/lgrosjean/pbi-docs:latest
```

- Extract doc from TMDL structure

In a shell, if your folder containg your TMDL structure is located in at `example/Model` path and you want to export the documentation to `example/docs` folder:
```sh
docker run -it --rm -v $(pwd)/example:/App/example ghcr.io/lgrosjean/pbi-docs:latest example/Model -d example/docs
```

- Extract TMDL structure from distant Semantic Model/Dataset to an `output` local folder using environment variables `PBI_TENANT_ID`, `PBI_CLIENT_ID` and `PBI_CLIENT_SECRET`

```sh
docker run -it --rm -v $(pwd)/output:/App/output \
  -e PBI_TENANT_ID=$PBI_TENANT_ID \
  -e PBI_CLIENT_ID=$PBI_CLIENT_ID \
  -e PBI_CLIENT_SECRET=$PBI_CLIENT_SECRET \
  ghcr.io/lgrosjean/pbi-docs \
  output/ serialize -w "CORE - OPERATIONS [DEV]" -d "Core Dataset Activity Details lgrosjean [DEV]"
```
## Resources

- https://github.com/ap0llo/markdown-generator
- C# Command Line: https://learn.microsoft.com/en-us/dotnet/standard/commandline/
- C# Logging: https://learn.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line
- To containerize application: https://learn.microsoft.com/en-us/dotnet/core/docker/build-container?tabs=linux&pivots=dotnet-7-0