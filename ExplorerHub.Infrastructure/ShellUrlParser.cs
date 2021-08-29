using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAPICodePack.Shell;

namespace ExplorerHub.Infrastructures
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
            //var so = ShellObject.FromParsingName("::{679F85CB-0220-4080-B29B-5540CC05AAB6}");
            // 快速访问
            //var quickVisit = .FromParsingName("");//KnownFolderHelper.FromKnownFolderId(Guid.Parse("{679F85CB-0220-4080-B29B-5540CC05AAB6}"));
            //folders[quickVisit.ToString()] = new[] {(ShellObject)quickVisit};
            KnownFolders = folders;
        }
    }
}
