﻿using System.CommandLine;
using Microsoft.AnalysisServices.Tabular;
using Grynwald.MarkdownGenerator;

namespace PbiDocs;

class Program
{
    static async Task Main(string[] args)
    {
        var rootCommand = new RootCommand(
            description: "Utility to generate Markdown documentation for PowerBI Semantic Model from TMDL structure."
        );

        var tmdlFolderPathArgument = new Argument<string>(
            name: "tmdlFolderPath",
            description: "Directy containing the TMDL folder structure.",
            getDefaultValue: () => "."
        );
        rootCommand.AddArgument(tmdlFolderPathArgument);

        var docsFolderPathOption = new Option<string>(
            name: "--docsFolderPath",
            description: "Default directory where to save generated files. If not exist, will be created.",
            getDefaultValue: () => "docs"
        );
        docsFolderPathOption.AddAlias("-d");

        rootCommand.AddOption(docsFolderPathOption);

        rootCommand.SetHandler(DoRootCommand, tmdlFolderPathArgument, docsFolderPathOption);

        rootCommand.Add(Serializer.GetCommand(tmdlFolderPathArgument));

        await rootCommand.InvokeAsync(args);
    }

    public static void DoRootCommand(string tmdlFolderPath, string docsFolderPath)
    {
        Console.WriteLine($"<tmdlFolderPath> argument = {tmdlFolderPath}");
        Console.WriteLine($"<docsFolderPath> option = {docsFolderPath}");

        Directory.CreateDirectory(docsFolderPath);

        var set = new DocumentSet<MdDocument>();

        var database = TmdlSerializer.DeserializeDatabaseFromFolder(tmdlFolderPath);
        var readmeMd = Program.CreateReadmeMd(set, database);

        var tablesBulletList = new MdBulletList();
        foreach (Table table in database.Model.Tables)
        {
            var tableMd = Program.CreateTableMd(set, table);
            tablesBulletList.Add(new MdListItem(
                set.GetLink(readmeMd, tableMd, table.Name))
            );
        }

        readmeMd.Root.Add(tablesBulletList);

        AddRelationshipsMd(readmeMd, database);

        set.Save(docsFolderPath, cleanOutputDirectory: true);
    }

    public static MdDocument CreateReadmeMd(DocumentSet<MdDocument> set, Database database)
    {
        var readmeMd = set.CreateMdDocument("README.md");
        readmeMd.Root.Add(new MdHeading(1, database.Name));
        readmeMd.Root.Add(new MdHeading(2, "Tables"));

        return readmeMd;
    }


    public static void AddRelationshipsMd(MdDocument readmeMd, Database database)
    {
        string mermaidStr = "";

        /// TODO: add a "CreateMermaidMd method from a relationship which returns a string
        foreach (SingleColumnRelationship relationship in database.Model.Relationships)
        {
            Table fromTable = relationship.FromTable;
            Table toTable = relationship.ToTable;

            Column fromColumn = relationship.FromColumn;
            Column toColumn = relationship.ToColumn;

            var fromCardinality = relationship.FromCardinality.ToString() == "One" ? "1" : "*";
            var toCardinality = relationship.ToCardinality.ToString() == "One" ? "1" : "*";

            mermaidStr += $"\n{fromTable.Name} \"{fromCardinality}\" --> \"{toCardinality}\" {toTable.Name}";
        }

        readmeMd.Root.Add(new MdHeading(2, "Relationships"));
        readmeMd.Root.Add(new MdParagraph(
            new MdRawMarkdownSpan("```mermaid\nclassDiagram\n"),
            new MdRawMarkdownSpan(mermaidStr),
            new MdRawMarkdownSpan("\n```")
            )
        );
    }


    public static MdDocument CreateTableMd(DocumentSet<MdDocument> set, Table table)
    {
        var tableName = table.Name.Replace("/", "\\");
        var tableMd = new MdDocument(new MdHeading(1, tableName));

        tableMd.Root.Add(new MdParagraph(table.Description ?? ""));

        Program.AddColumnsMd(tableMd, table);
        Program.AddMeasuresMd(tableMd, table);

        set.Add($"tables/{tableName}.md", tableMd);

        Console.WriteLine($"Documentation for table '{tableName}' created!");

        return tableMd;
    }

    public static void AddColumnsMd(MdDocument document, Table table)
    {
        if (table.Columns.Count >= 1)
        {

            document.Root.Add(new MdHeading("Columns", 2));

            var columnTableHeader = new MdTableRow("Column", "Description", "Type");
            var columnTableRows = new List<MdTableRow>();

            // TODO: create a "AddColumnMd" method using a column and returning a row
            foreach (var column in table.Columns)
            {
                var columnName = column.Name;
                var columnDataType = column.DataType.ToString();
                var columnDescription = column.Description;
                columnTableRows.Add(new MdTableRow(columnName, columnDescription, columnDataType));
            }
            document.Root.Add(new MdTable(columnTableHeader, columnTableRows));
        }
    }
    public static void AddMeasuresMd(MdDocument document, Table table)
    {
        if (table.Measures.Count >= 1)
        {

            document.Root.Add(new MdHeading("Measures", 2));

            var measureTableHeader = new MdTableRow("Measure", "Description", "Expression");
            var measureTableRows = new List<MdTableRow>();

            foreach (var measure in table.Measures)
            {
                var measureName = measure.Name;
                var measureExpression = measure.Expression;
                var measureDescription = measure.Description;
                measureTableRows.Add(new MdTableRow(measureName, measureDescription, measureExpression));
            }
            document.Root.Add(new MdTable(measureTableHeader, measureTableRows));
        }
    }

}
