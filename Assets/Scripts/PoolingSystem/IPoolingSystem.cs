public interface IPoolingSystem<T> where T : IPoolable
{
    T Get();
    void Return(T obj);
}
