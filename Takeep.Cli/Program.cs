using System.CommandLine;
using Takeep.Core;

#region Keep Command

var keepName = new Option<string> ("--name", "Gets the name of the item");
var keepContent = new Option<string> ("--content", "Gets the content of the keep item");

keepName.Arity = ArgumentArity.ExactlyOne;
keepContent.Arity = ArgumentArity.ExactlyOne;

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
	try
	{
		TakeepXml.Keep (new Item { Name = name, Content = content });

		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine ("✓ Successfully added the item!");
		Console.ForegroundColor = ConsoleColor.White;
	}
	catch (Exception exception)
	{
		HandleException (exception);
	}
}, keepName, keepContent);

#endregion

#region Take Command

var takeName = new Option<string> ("--name", "Gets the name of the item");
takeName.Arity = ArgumentArity.ExactlyOne;

takeName.AddAlias ("-n");

var takeCommand = new Command (
	"take",
	"Takes (finds) an Item")
{
	takeName
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

#endregion

#region Remove Command

var removeName = new Option<string> ("--name", "Gets the name of the item");

removeName.AddAlias ("-n");
removeName.Arity = ArgumentArity.ExactlyOne;

var removeCommand = new Command (
	"remove",
	"Removes an item")
{
	removeName
};

removeCommand.SetHandler ((string name) =>
{
	try
	{
		TakeepXml.Remove (name);

		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine ("✓ Successfully removed the item!");
		Console.ForegroundColor = ConsoleColor.White;
	}
	catch (Exception exception)
	{
		HandleException (exception);
	}

}, removeName);

#endregion

#region List Command

var listCommand = new Command ("list", "Lists all items in your keepsheet");

listCommand.SetHandler (() =>
{
	Console.ForegroundColor = ConsoleColor.Yellow;
	Console.WriteLine ("■ The list of items of default keepsheet:");
	Console.ForegroundColor = ConsoleColor.White;
	TakeepXml.List ();
});

#endregion

var rootCommand = new RootCommand
{
	keepCommand,
	takeCommand,
	removeCommand,
	listCommand
};


rootCommand.Invoke (args);

void HandleException (Exception exception)
{
	Console.ForegroundColor = ConsoleColor.Red;
	Console.WriteLine ("Oops! Something wrong happened. if you continue seeing this error, please tell us: https://github.com/matinmn87/takeep");
	Console.ForegroundColor = ConsoleColor.White;
}