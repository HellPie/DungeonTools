namespace DungeonTools.Save.File {
    public static class EncryptionProviders {
        public static readonly IEncryptionProvider Current = new AesEncryptionProvider();
    }
}
