using System;
using System.Collections;
using System.Collections.Generic;
using _353503_Sebelev_Lab3.Entities;
using _353503_Sebelev_Lab3.Interfaces;

namespace _353503_Sebelev_Lab3.Collections;

public class Node<T>
{
    public T Data { get; set; }
    public Node<T> Next { get; set; }

    public Node(T data)
    {
        Data = data;
    }
}

public class MyCustomCollection<T> : ICustomCollection<T>, IEnumerable<T>
{
    private Node<T> _head;
    private Node<T> _tail;
    private int _count;
    private Node<T> _current;
    private int _cursor;

    public T this[int index]
    {
        get
        {
            if (index < 0 || index >= _count)
            {
                throw new IndexOutOfRangeException();
            }

            Node<T> current = _head;
            for (int i = 0; i < index; i++)
            {
                current = current.Next;
            }

            return current.Data;
        }

        set
        {
            if (index < 0 || index >= _count)
            {
                throw new IndexOutOfRangeException();
            }

            Node<T> current = _head;
            for (int i = 0; i < index; i++)
            {
                current = current.Next;
            }

            current.Data = value;
        }
    }

    public void Add(T item)
    {
        Node<T> node = new Node<T>(item);
        if (_head == null)
        {
            _head = node;
            _current = _head;
        }
        else
        {
            _tail.Next = node;
        }

        _tail = node;
        _count++;
    }

    public void Remove(T item)
    {
        Node<T> previous = null;
        Node<T> current = _head;

        while (current != null)
        {
            if (current.Data.Equals(item))
            {
                if (previous != null)
                {
                    previous.Next = current.Next;

                    if (current.Next == null)
                    {
                        _tail = previous;
                    }
                }
                else
                {
                    _head = _head.Next;

                    if (_head == null)
                    {
                        _tail = null;
                    }
                }

                _count--;
                return;
            }

            previous = current;
            current = current.Next;
        }

        throw new ItemNotFoundExeption("Item not found in the collection");
    }

    public int Count
    {
        get { return _count; }
    }

    public void Reset()
    {
        _cursor = -1;
        _current = _head;
    }

    public void Next()
    {
        if (_cursor < _count - 1)
        {
            _cursor++;
            _current = _current.Next;
        }
    }

    public T Current()
    {
        if (_cursor >= 0 && _cursor < _count)
        {
            return _current.Data;
        }

        throw new InvalidOperationException("Cursor is out of bounds");
    }

    public T RemoveCurrent()
    {
        if (_cursor >= 0 && _cursor < _count)
        {
            T item = _current.Data;
            Remove(item);
            return item;
        }

        throw new InvalidOperationException("Cursor is out of bounds");
    }

    public T GetLast()
    {
        if (_tail == null)
        {
            throw new InvalidOperationException("The collection is empty");
        }

        return _tail.Data;
    }
    
    public IEnumerator<T> GetEnumerator()
    {
        Node<T> current = _head;
        while (current != null)
        {
            yield return current.Data;
            current = current.Next;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}