using System;
using System.Collections.Generic;
using System.Linq;
using ExplorerHub.ViewModels;
using Microsoft.WindowsAPICodePack.Shell;

namespace ExplorerHub.Infrastructure
{
    public class ShellUrlParser : IShellUrlParser
    {
        public IReadOnlyDictionary<string, ShellObject[]> KnownFolders { get; }

        public ShellObject Default { get; } = (ShellObject) Microsoft.WindowsAPICodePack.Shell.KnownFolders.Computer;

        public ShellUrlParser()
        {
            var folders = Microsoft.WindowsAPICodePack.Shell.KnownFolders.All
                .GroupBy(folder => folder.ToString(), folder => folder as ShellObject, StringComparer.CurrentCultureIgnoreCase)
                .ToDictionary(folder => folder.Key,
                    folder => folder.ToArray(),
                    StringComparer.CurrentCultureIgnoreCase);

            // 兼容Win10: 快速访问
            try
            {
                var so = ShellObject.FromParsingName("shell:::{679F85CB-0220-4080-B29B-5540CC05AAB6}");
                var key = so.GetDisplayName(DisplayNameType.Default);

                if (!folders.ContainsKey(key))
                {
                    folders.Add(key, new[] { so });
                }
            }
            catch (ShellException)
            {
                
            }

            KnownFolders = folders;
        }
    }
}
