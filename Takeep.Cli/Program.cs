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

var takeOption = new Option<string> ("take", "Takes (finds) an Item by its name");

var rootCommand = new RootCommand
{
	keepCommand,
	takeOption
};

rootCommand.SetHandler ((string take) =>
{
	Item item = TakeepXml.Take (take);

	if (item == null)
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine ($"■ No items found with name \"{take}\"");
		Console.ForegroundColor = ConsoleColor.White;
	}
	else
	{
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine ($"■ This is the content of {item.Name}:");
		Console.ForegroundColor = ConsoleColor.White;

		Console.WriteLine (item.Content);
	}

}, takeOption);

rootCommand.Invoke (args);
