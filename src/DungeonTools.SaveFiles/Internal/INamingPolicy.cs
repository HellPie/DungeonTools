namespace DungeonTools.SaveFiles.Internal {
    public interface INamingPolicy<T> {
        public T ConvertName(string name);
        public string ConvertValue(T value);
    }
}
