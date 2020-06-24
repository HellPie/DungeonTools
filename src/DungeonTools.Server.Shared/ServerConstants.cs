using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("DungeonTools.Server")]

namespace DungeonTools.Server.Shared {
    public class ServerConstants {
        internal const string ControllerName = "api/encryption";
        internal const string ControllerKeysName = "keys";
        internal const string ControllerEncryptionName = "encrypt";
        internal const string ControllerDecryptionName = "decrypt";

        public const string LocalServer = "http://127.0.0.1:5000/";
        public const string RemoteServer = "http://dungeons.tools/";

        public const string KeysEndpoint = ControllerName + "/" + ControllerKeysName;
        public const string EncryptEndpoint = ControllerName + "/" + ControllerEncryptionName;
        public const string DecryptEndpoint = ControllerName + "/" + ControllerDecryptionName;
    }
}
