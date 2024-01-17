
using System.CommandLine;
using Microsoft.AnalysisServices.Tabular;

namespace PbiDocs;

public class Serializer
{

    public static Command GetCommand(Argument<string> tmdlFolderPathArgument)
    {
        var workspaceNameOption = new Option<string>(
                 name: "--workspaceName",
                 description: "Workspace containing Dataset to serialize"
             );
        workspaceNameOption.AddAlias("-w");

        var datasetNameOption = new Option<string>(
            name: "--datasetName",
            description: "Dataset name to serialize"
        );
        datasetNameOption.AddAlias("-d");

        var serializeCommand = new Command("serialize", "Serialize cloud PowerBI Dataset to local")
        {
            workspaceNameOption,
            datasetNameOption
        };

        serializeCommand.SetHandler(SerializeToFolder, workspaceNameOption, datasetNameOption, tmdlFolderPathArgument);

        return serializeCommand;
    }

    public static void SerializeToFolder(string workspaceName, string datasetName, string tmdlFolderPath)
    {
        Console.WriteLine($"Hello World: {workspaceName} {datasetName}");

        string srcWorkspaceConnection = $"powerbi://api.powerbi.com/v1.0/myorg/{workspaceName}";
        string? tenantId = Environment.GetEnvironmentVariable("PBI_TENANT_ID");
        string? appId = Environment.GetEnvironmentVariable("PBI_CLIENT_ID");
        string? appSecret = Environment.GetEnvironmentVariable("PBI_CLIENT_SECRET");

        string workspaceXmla = $"DataSource={srcWorkspaceConnection};User ID=app:{appId}@{tenantId};Password={appSecret};";

        var server = new Server();
        server.Connect(workspaceXmla);

        var database = server.Databases.GetByName(datasetName);

        var destinationFolder = $"{tmdlFolderPath}{Path.DirectorySeparatorChar}{database.Name}";

        TmdlSerializer.SerializeModelToFolder(database.Model, destinationFolder);

        Console.WriteLine($"TMDL structure from {datasetName} inside {workspaceName} extracted to {destinationFolder}");
    }
}
