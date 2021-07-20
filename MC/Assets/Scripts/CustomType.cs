using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleStack<T>
{
    T[] data;
    int count=0;

    private SimpleStack() { }
    public SimpleStack(int capacity)
    {
        data = new T[capacity];
    }

    public T[] Data
    {
        private get { return data; }

        set
        {
            data = value;
            count = data.Length;
        }
    }

    public void Add(T _data)
    {
        data[count++] = _data;
    }
    public T Pop
    {
        get
        {
            return data[--count];
        }
    }
    public T this[int idx]
    {
        get
        {
            return data[idx];
        }
    }

    public bool IsWorking
    {
        get
        {
            return (count > 0);
        }
    }
    public int Count
    {
        get
        {
            return count;
        }
    }

    public void Clear()
    {
        count = 0;
    }
}

public class SimpleQueue<T>
{
    T[] data;

    int firstIdx = 0;
    int lastIdx = -1;

    int count = 0;
    int capacity;

    private SimpleQueue() { }
    public SimpleQueue(int _capacity)
    {
        data = new T[_capacity];
        capacity = _capacity;
    }

    public void Add(T _data)
    {
        lastIdx = NextIdx(lastIdx);

        data[lastIdx] = _data;

        ++count;
    }
    public T Pop
    {
        get
        {
            --count;

            T output = data[firstIdx];

            firstIdx = NextIdx(firstIdx);

            return output;
        }
    }
    int NextIdx(int curIdx)
    {
        if (curIdx == (capacity - 1))
            curIdx = 0;
        else
            curIdx += 1;

        return curIdx;
    }

    public bool IsEmpty
    {
        get
        {
            return (count == 0);
        }
    }
}

public struct WaitFrames
{
    int skipCount;
    int curCount;

    public WaitFrames(int count)
    {
        skipCount = count;
        curCount = 0;
    }

    public bool OnTime
    {
        get
        {
            ++curCount;
            if(curCount >= skipCount)
            {
                curCount = 0;
                return true;
            }
            return false;
        }
    }
}
