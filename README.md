# takeep ![GitHub last commit](https://img.shields.io/github/last-commit/matinmn87/takeep?color=7676ec&logo=github&style=flat-square) ![GitHub commit activity](https://img.shields.io/github/commit-activity/w/matinmn87/takeep?color=7676ec&logo=git&logoColor=ffffff&style=flat-square) ![GitHub release (latest SemVer)](https://img.shields.io/github/v/release/matinmn87/takeep?color=%237676ec&label=latest%20release&style=flat-square)
The utility 🔧 of never getting your texts 📃 lost! Takeep is a simple command-line 🧑‍💻 tool that keeps 🗃️ your texts & lets you take them easily 🙂.

# How takeep works?
Takeep is as simple as you can imagine! Add your text to takeep, give it a name, done! Now, you can access your text with 'take' command.

![takeep commands' overview](/files/takeepCommandsPreview.gif)

```
> tkp keep -n myToken -c 239840984:sdofum0JDokcdfvjiSiodo
√ Successfully added the item!
> tkp take -n myToken
■ This is the content of myToken:
239840984:sdofum0JDokcdfvjiSiodo
> tkp list
■ The list of items of default keepsheet:
        myToken
> tkp edit -n myToken -c sdofum0JDokcdfvjiSiodo
√ Successfully edited the item!
> tkp take -n myToken
■ This is the content of myToken:
sdofum0JDokcdfvjiSiodo
```

# Project files
takeep is written in C# with Visual Studio. Currently, takeep has 2 main projects:
- Takeep.Cli
- Takeep.Core

**Takeep.Cli** is the presentation project. It uses System.CommandLine to handle arguments for app call.
**Takeep.Core** is the core project. It contains methods & functionality of takeep commands.
