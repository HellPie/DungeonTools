using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CommandLine;
using CommandLine.Text;

namespace DungeonTools.Cli {
    [Verb("raw", HelpText = "Command for manual operation on saves and profiles.")]
    internal class RawOptions {

        [Value(0, HelpText = "Input files. Can be a list of JSON files, DAT files or both.", MetaValue = "FILES", Required = true, Min = 1)]
        public IEnumerable<string> Input { get; }

        [Option('n', "overwrite", HelpText = "Whether to overwrite existing files or not.", Required = false, Default = true)]
        public bool Overwrite { get; }

        // ReSharper disable StringLiteralTypo
        [Usage(ApplicationAlias = "dtools")]
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        public static IEnumerable<Example> Examples => new[] {
            new Example("Convert an encrypted DAT file into a JSON file", new RawOptions(new[] {"savefile.dat"})),
            new Example("Convert a list of both encrypted DAT files and JSON files without overwriting existing their counterpart files on output.", new RawOptions(new[] {"savefile.dat", "savefile2.dat"}, false)),
            new Example("Convert a list of JSON files into encrypted DAT files", new RawOptions(new[] {"459357397BF844C9A723EEB46902EC78.json", "savefile.json", "F8C65EB5184743609ACAE871CCBC6F1F.json"})),
            new Example("Convert a list of both encrypted DAT files and JSON files into the counterpart files", new RawOptions(new[] { "459357397BF844C9A723EEB46902EC78.dat", "F8C65EB5184743609ACAE871CCBC6F1F.json", "savefile.dat"})),
        };
        // ReSharper restore StringLiteralTypo

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public RawOptions(IEnumerable<string> input, bool overwrite = true) {
            Input = input;
            Overwrite = overwrite;
        }
    }
}
