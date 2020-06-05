using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CommandLine;
using CommandLine.Text;

namespace DungeonTools.Cli {
    [Verb("raw", HelpText = "Helper commands for manual operation on saves and profiles.")]
    internal class RawOptions {
        [DisallowNull] private readonly string _input;
        private readonly bool _encrypt;

        [Value(0, HelpText = "Input save file. Can be either a JSON file or a DAT save file.", MetaValue = "FILE", Required = true)]
        [DisallowNull]
        public string Input => _input;

        [Option('e', "encrypt", Default = false, HelpText = "Whether the file should be encrypted or decrypted.", Required = false)]
        public bool Encrypt => _encrypt;

        // ReSharper disable StringLiteralTypo
        [Usage(ApplicationAlias = "dtools")]
        public static IEnumerable<Example> Examples => new[] {
            new Example("Convert a DAT global save file into JSON", new RawOptions("savefile.dat")),
            new Example("Convert a JSON global save file back into a DAT file", new RawOptions("savefile.json", true)),
            new Example("Convert a DAT profile file into JSON", new RawOptions("459357397BF844C9A723EEB46902EC78.dat")),
            new Example("Convert a JSON profile file back into a DAT file", new RawOptions("459357397BF844C9A723EEB46902EC78.json", true)),
        };
        // ReSharper restore StringLiteralTypo

        public RawOptions([DisallowNull] string input, bool encrypt = false) {
            _input = input;
            _encrypt = encrypt;
        }
    }
}
