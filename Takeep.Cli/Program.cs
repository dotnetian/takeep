using System.CommandLine;
using Takeep.Core;

var keepName = new Option<string> ("--name", "Gets the name of the item");
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

var takeName = new Option<string> ("--name", "Gets the name of the item");

takeName.AddAlias ("-n");

var takeCommand = new Command (
	"take",
	"Takes (finds) an Item by its name")
{
	takeName
};

var rootCommand = new RootCommand ("takeep is a tool to help you organize your texts")
{
	keepCommand,
	takeCommand
};

takeCommand.SetHandler ((string take) =>
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

}, takeName);

rootCommand.Invoke (args);
