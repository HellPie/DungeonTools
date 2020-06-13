namespace DungeonTools.Encryption {
    public class EncryptionServices {
        public static readonly IEncryptionService Default;

        public static readonly IEncryptionService LocalKeys = new AesEncryptionService();
        public static readonly IEncryptionService RemoteApi = new ApiEncryptionService();


        static EncryptionServices() {
#if USE_KEYS
            Default = LocalKeys;
#else
            Default = RemoteApi;
#endif
        }
    }
}
