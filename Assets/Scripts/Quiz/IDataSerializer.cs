public interface IDataSerializer<T>
{
    string FileExtension { get; }
    void Save(string path, T data);
    T Load(string path);
}