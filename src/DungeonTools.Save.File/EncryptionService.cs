namespace DungeonTools.Encryption {
    public class EncryptionService {
        public static readonly IEncryptionProvider Current;

        public static readonly IEncryptionProvider Local = new AesEncryptionProvider();
        public static readonly IEncryptionProvider Remote = new RemoteEncryptionProvider();


        static EncryptionService() {
#if USE_KEYS
            Current = Local;
#else
            Current = Remote;
#endif
        }
    }
}
