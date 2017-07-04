using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class UniqueQueue<T>
{
    private readonly Queue<T> queue = new Queue<T>();
    private HashSet<T> alreadyAdded = new HashSet<T>();

    public virtual void Enqueue(T item)
    {
        if (alreadyAdded.Add(item)) { queue.Enqueue(item); }
    }
    public int Count { get { return queue.Count; } }

    public virtual T Dequeue()
    {
        T item = queue.Dequeue();
        return item;
    }
    public virtual T Peek()
    {
        T item = queue.Peek();
        return item;
    }
    public virtual void Clear()
    {
        alreadyAdded.Clear();
        queue.Clear();
    }
    public virtual void ClearHash()
    {
        alreadyAdded.Clear();
    }

}
