using System.CommandLine;
using Takeep.Core;

#region Keep Command

var keepName = new Option<string> ("--name", "The name of the item. The item will be known & shown by this name")
{
	Arity = ArgumentArity.ExactlyOne
};
keepName.AddAlias ("-n");

var keepContent = new Option<string> ("--text", "The text of the item. The main content you want to keep. Null to write text in notepad")
{
	Arity = ArgumentArity.ExactlyOne
};
keepContent.AddAlias ("-t");

var keepKeepsheet = new Option<string> ("--keepsheet", "The keepsheet which item will keep. Null to keep in default keepsheet")
{
	Arity = ArgumentArity.ExactlyOne
};
keepKeepsheet.AddAlias ("-k");

var keepCommand = new Command (
	"keep",
	$"Keeps (Adds) a text in default takesheet.{Environment.NewLine}Example: 	tkp keep -n test -t \"This is the test text\" (Recommanded for short & quick texts).{Environment.NewLine}Not specifying text (-t or --text) will open notepad, so you can write your text there easier (Recommanded for long or multiline texts).")
{
	keepName,
	keepContent,
	keepKeepsheet
};

keepCommand.SetHandler ((string name, string content, string keepsheet) =>
{
	try
	{
		TakeepXml.Keep (new Item { Name = name, Text = content }, keepsheet);
	}
	catch (Exception exception)
	{
		HandleException (exception);
	}
}, keepName, keepContent, keepKeepsheet);

#endregion

#region Take Command

var takeName = new Option<string> ("--name", "The name of the item")
{
	Arity = ArgumentArity.ExactlyOne
};
takeName.AddAlias ("-n");


var takeCopy = new Option<bool> ("--copy", "Copies the item's text to clipboard & doesn't shows it")
{
	Arity = ArgumentArity.ZeroOrOne
};
takeCopy.AddAlias ("-c");

var takeNotepad = new Option<bool> ("--open", "Opens the item's content in notepad")
{
	Arity = ArgumentArity.ZeroOrOne
};
takeNotepad.AddAlias ("-o");

var takeKeepsheet = new Option<string> ("--keepsheet", "The keepsheet which item will be looked for. Null to take from default keepsheet")
{
	Arity = ArgumentArity.ExactlyOne
};
takeKeepsheet.AddAlias ("-k");

var takeCommand = new Command (
	"take",
	$"Takes (shows) an Item's text. Take command is useful if you have the exact name of the item & you want to access to it's content.{Environment.NewLine}Examples:{Environment.NewLine}	tkp take -n test    (Writes the text in console, great for quick access){Environment.NewLine}	tkp take -n test -c (Copies the text to clipboard, useful if you don't want to text be shown on screen){Environment.NewLine}	tkp take -n test -o (Opens the text in notepad, great for long texts)")
{
	takeName,
	takeCopy,
	takeNotepad,
	takeKeepsheet
};

takeCommand.SetHandler ((string take, bool copy, bool notepad, string keepsheet) =>
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
		try
		{
			TakeepXml.Take (take, copy, notepad, keepsheet);
		}
		catch (Exception exeption)
		{
			HandleException (exeption);
		}
	}

}, takeName, takeCopy, takeNotepad, takeKeepsheet);

#endregion

#region Remove Command

var removeName = new Option<string> ("--name", "The name of the item");
removeName.AddAlias ("-n");
removeName.Arity = ArgumentArity.ExactlyOne;

var removeKeepsheet = new Option<string> ("--keepsheet", "The keepsheet which item will be looked for. Null to remove from default keepsheet")
{
	Arity = ArgumentArity.ExactlyOne
};
removeKeepsheet.AddAlias ("-k");

var removeCommand = new Command (
	"remove",
	$"Removes an item. Example:{Environment.NewLine}	tkp remove -n test")
{
	removeName,
	removeKeepsheet
};

removeCommand.SetHandler ((string name, string keepsheet) =>
{
	try
	{
		TakeepXml.Remove (name, keepsheet);
	}
	catch (Exception exception)
	{
		HandleException (exception);
	}

}, removeName, removeKeepsheet);

#endregion

#region List Command

var listName = new Option<string> ("--name");
listName.AddAlias ("-n");
listName.Arity = ArgumentArity.ZeroOrOne;

var listKeepsheet = new Option<string> ("--keepsheet");
listKeepsheet.AddAlias ("-k");
listKeepsheet.Arity = ArgumentArity.ExactlyOne;

var listCommand = new Command (
	"list",
	"Lists all items in your keepsheet")
{
	listName,
	listKeepsheet
};

listCommand.SetHandler ((string name, string keepsheet) =>
{
	try
	{
		TakeepXml.List (name, keepsheet);
	}
	catch (Exception exception)
	{
		HandleException (exception);
	}

}, listName, listKeepsheet);

#endregion

#region Edit Command

var editName = new Option<string> ("--name", "The name of the item");
editName.AddAlias ("-n");
editName.Arity = ArgumentArity.ExactlyOne;

var editContent = new Option<string> ("--text", "The text of the item.");
editContent.AddAlias ("-t");
editContent.Arity = ArgumentArity.ExactlyOne;

var editNotepad = new Option<bool> ("--open", "Opens the item's content in notepad")
{
	Arity = ArgumentArity.ZeroOrOne
};
editNotepad.AddAlias ("-o");


var editCommand = new Command (
	"edit",
	$"Edits an item's content. Examples:{Environment.NewLine}	tkp edit -n test -t \"This is not the test text\" (Edits the text of the item named test to \"This is not the test text\", great for quick & single-line edits){Environment.NewLine}	tkp edit -n test -o (Opens the text in notepad, great for long texts)")
{
	editName,
	editContent,
	editNotepad
};

editCommand.SetHandler ((string name, string content, bool notepad) =>
{
	try
	{
		TakeepXml.Edit (new Item { Name = name, Text = content }, notepad);
	}
	catch (Exception exception)
	{
		HandleException (exception);
	}

}, editName, editContent, editNotepad);

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
	Console.WriteLine ("Oops! Something wrong happened. The error message was copied to your clipboard. if you continue seeing this error, please tell us: https://github.com/matinmn87/takeep/issues");
	TakeepClipboard.Copy (exception.Message);
	Console.ForegroundColor = ConsoleColor.White;
}