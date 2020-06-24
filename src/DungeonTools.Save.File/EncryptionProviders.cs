namespace DungeonTools.Save.File {
    public static class EncryptionProviders {
        public static readonly IEncryptionProvider Current;

        public static readonly IEncryptionProvider Local = new AesEncryptionProvider();
        public static readonly IEncryptionProvider Remote = new RemoteEncryptionProvider();

        static EncryptionProviders() {
#if USE_KEYS
            Current = Local;
#else
            Current = Remote;
#endif
        }
    }
}
