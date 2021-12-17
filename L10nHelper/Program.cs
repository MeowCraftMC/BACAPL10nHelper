// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

if (args.Length != 2)
{
    Console.WriteLine("Usage: dotnet L10nHelper.dll <L10n file> <Path>.");
    return;
}

var lines = File.ReadAllLines(args[0]);
var str = lines
    .Where(line => !line.StartsWith("#"))
    .Aggregate(string.Empty, (current, line) => current + line);
var translations = JsonConvert.DeserializeObject<Dictionary<string, string>>(str);

var pathToFiles = new List<string>();
GetPaths(args[1]);

foreach (var file in pathToFiles)
{
    var fileContent = File.ReadAllText(file);

    fileContent = translations
        .Aggregate(fileContent, (current, translation) => 
            current.Replace($"\"{translation.Key}\"", $"\"{translation.Value}\""));

    File.WriteAllText(file, fileContent);

    Console.WriteLine(file);
}

void GetPaths(string dirPath)
{
    // if (!Directory.Exists(dirPath))
    // {
    //     return;
    // }
    
    pathToFiles.AddRange(Directory.GetFiles(dirPath));

    foreach (var dir in Directory.GetDirectories(dirPath))
    {
        GetPaths(dir);
    }
}
