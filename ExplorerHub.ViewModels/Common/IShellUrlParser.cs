using System;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Shell;

namespace ExplorerHub.ViewModels
{
    public interface IShellUrlParser
    {
        IReadOnlyDictionary<string, ShellObject[]> KnownFolders { get; }

        ShellObject Default { get; }
    }

    public static class ShellParserExtensions
    {
        public static bool TryParse(this IShellUrlParser folderManager,
            string parsingName, out ShellObject shellObject)
        {
            var path = parsingName;
            if (folderManager.KnownFolders.TryGetValue(path, out var folders))
            {
                shellObject = folders[0];
                return true;
            }

            try
            {
                shellObject = ShellObject.FromParsingName(parsingName);
                return true;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                shellObject = null;
                return false;
            }
        }
    }
}