#if !USE_KEYS

// ReSharper disable CommentTypo

// This class allows the library to compile without errors. Encryption keys are not published with the code as they are
// Mojang's property and it would put the legality of the project at risk.
// To get encryption and decryption to work you will have to manually configure Algorithm to be an AesManaged instance
// with the valid Key and IV.
// Please store those in a file named "AesEncryptionService.Keys.cs" surrounding it with an "#ifdef USE_KEYS"
// macro block so that this file will not clash with it during compile time.
// Using a different file in a partial class ending in ".Keys.cs" triggers a .gitignore rule so encryption
// keys are never committed with the project's sources.

// The creation of a file named "AesEncryptionService.Keys.cs" containing only an Algorithm property with a set IV
// and Key is the only exception to this project's license (AGPL-3.0).

using System.Security.Cryptography;

namespace DungeonTools.Encryption {
    internal partial class AesEncryptionService {
        private static readonly SymmetricAlgorithm Algorithm = new AesManaged {
            Mode = CipherMode.ECB,
            Padding = PaddingMode.None,
        };
    }
}

#endif
