using System.Collections.Generic;

public class Board<T> where T : class
{
    protected List<T> _elements;
    public int Count => _elements.Count;

    public Board()
    {
        _elements = new List<T>();
    }

    public void Add(T element)
    {
        _elements.Add(element);
    }

    public T GetAt(int index)
    {
        if (index < 0 || index >= _elements.Count)
            return null;

        return _elements[index];
    }

    public T GetFirst()
    {
        return _elements[0];
    }
}


