using System.Collections.Generic;
using System.IO;
using Microsoft.WindowsAPICodePack.Shell;

namespace ExplorerHub
{
    public interface IKnownFolderManager
    {
        IReadOnlyDictionary<string, ShellObject[]> Folders { get; }

        ShellObject Default { get; }
    }

    public static class FolderManagerExtensions
    {
        public static bool TryParse(this IKnownFolderManager folderManager,
            string parsingName, out ShellObject shellObject)
        {
            var path = parsingName;
            if (Path.IsPathRooted(path) && Directory.Exists(path))
            {
                shellObject = ShellObject.FromParsingName(path);
                return true;
            }
            
            if (folderManager.Folders.TryGetValue(path, out var folders))
            {
                shellObject = folders[0];
                return true;
            }

            shellObject = null;
            return false;
        }
    }
}