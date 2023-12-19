# pbi-docs

Utility to generate Markdown documentation for PowerBI Semantic Model from TMDL structure.

## Usage

```sh
Usage:
  pbi-docs [<tmdlFolderPath>] [options]

Arguments:
  <tmdlFolderPath>  Directy containing the TMDL folder structure. [default: .]

Options:
  -d, --docsFolderPath <docsFolderPath>  Default directory where to save generated files. If not exist, will be created. [default: docs]
  --version                              Show version information
  -?, -h, --help                         Show help and usage information
```

## With Docker

Start by pulling the image locally
```sh
docker pull ghcr.io/lgrosjean/pbi-docs:latest
```

Then, in a shell:
```sh
docker run -it --rm -v $(pwd)/example:/App/example pbi-docs example/Model -d example/docs
```

## Resources

- https://github.com/ap0llo/markdown-generator
- C# Command Line: https://learn.microsoft.com/en-us/dotnet/standard/commandline/
- C# Logging: https://learn.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line
- To containerize application: https://learn.microsoft.com/en-us/dotnet/core/docker/build-container?tabs=linux&pivots=dotnet-7-0