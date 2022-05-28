using System.CommandLine;
using Takeep.Core;


var keepName = new Option<string> ("--name", "Gets a name for the keep item");
var keepContent = new Option<string> ("--content", "Gets the content of the keep item");

keepName.AddAlias ("-n");
keepContent.AddAlias ("-c");

var keepCommand = new Command (
	"keep",
	"Keeps (Adds) a text in default takesheet")
{
	keepName,
	keepContent
};

keepCommand.SetHandler ((string name, string content) =>
{
	TakeepXml.Keep (new Item { Name = name, Content = content });

	Console.ForegroundColor = ConsoleColor.Green;
	Console.WriteLine ("✓ Successfully added the item!");
	Console.ForegroundColor = ConsoleColor.White;

}, keepName, keepContent);

var rootCommand = new RootCommand
{
	keepCommand
};

rootCommand.Invoke (args);
