using System.CommandLine;
using Takeep.Core;

#region Keep Command

var keepName = new Option<string> ("--name", "Gets the name of the item");
var keepContent = new Option<string> ("--text", "Gets the content of the keep item");

keepName.Arity = ArgumentArity.ExactlyOne;
keepContent.Arity = ArgumentArity.ExactlyOne;

keepName.AddAlias ("-n");
keepContent.AddAlias ("-t");

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
	if (take == null)
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.Write ("You must enter ");
		Console.ForegroundColor = ConsoleColor.White;
		Console.Write ("name");
		Console.ForegroundColor = ConsoleColor.White;
	}
	else
	{
		TakeepXml.Take (take);
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

#region Edit Command

var editName = new Option<string> ("--name", "Gets the name of the item");
var editContent = new Option<string> ("--text", "Gets the content of the keep item");

keepName.Arity = ArgumentArity.ExactlyOne;
editContent.Arity = ArgumentArity.ExactlyOne;

editName.AddAlias ("-n");
editContent.AddAlias ("-t");

var editCommand = new Command (
	"edit",
	"Edits an item's content (not name)")
{
	editName,
	editContent
};

editCommand.SetHandler ((string name, string content) =>
{
	try
	{
		TakeepXml.Edit (new Item { Name = name, Content = content });
	}
	catch (Exception exception)
	{
		HandleException (exception);
	}

}, editName, editContent);

#endregion

var rootCommand = new RootCommand ("takeep is the utility of never getting your texts lost! Takeep is a simple command-line tool that keeps your texts & lets you take them easily")
{
	keepCommand,
	takeCommand,
	removeCommand,
	listCommand,
	editCommand
};

rootCommand.Invoke (args);

void HandleException (Exception exception)
{
	Console.ForegroundColor = ConsoleColor.Red;
	Console.WriteLine ("Oops! Something wrong happened. if you continue seeing this error, please tell us: https://github.com/matinmn87/takeep");
	Console.ForegroundColor = ConsoleColor.White;
}