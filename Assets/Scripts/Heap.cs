using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>{

    T[] items;
    int currentItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T RemoveFirst()
    {
        T first = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return first;
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    void SortDown(T item)
    {
        while (true) //could use a recursive method, probably less optimized ?
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;
            if(childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;
                if (childIndexRight < currentItemCount)
                {
                    if (items[childIndexLeft].CompareTo(items[childIndexRight])<0)
                    {
                        swapIndex = childIndexRight;
                    }
                }
                if (item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else { return; } //already at the right place
            }
            else { return; } //already at the right place
        }
    }

    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2; //using the array version of a heap
        while (true) //could use a recursive method, probably less optimized ?
        {
            T parentItem = items[parentIndex];
            //classical Heap sorting, swaping item with parent when needed
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }
            parentIndex = (item.HeapIndex - 1 / 2);
        }
    }

    void Swap(T item1, T item2)
    {
        items[item1.HeapIndex] = item2;
        items[item2.HeapIndex] = item1;
        int tmpItem1Hi = item1.HeapIndex; //temporary item1 heap index
        item1.HeapIndex = item2.HeapIndex;
        item2.HeapIndex = tmpItem1Hi;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}