using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;
    
    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
        
    }

    public void Add(T item) //Adding new items to the heap
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;

        SortUp(item);
        currentItemCount++;
    }

    public T Remove() //removing first item
    {
        T firstItem = items[0];
        currentItemCount--;

        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;

        SortDown(items[0]);

        return firstItem;
    }

    public void UpdateItem(T item) //if we want to change priory of an item, in case we have found new path with lower F cost
    {
        SortUp(item);
    }

    public int Count //getting the number of items in the Heap
    {
        get
        {
            return currentItemCount;
        }
    }


    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    void SortDown(T item) //decreasing the priority of an item in the heap
    {
        while(true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRigh = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if(childIndexLeft< currentItemCount)
            {
                swapIndex = childIndexLeft;

                if (childIndexRigh < currentItemCount)
                {
                    if(items[childIndexLeft].CompareTo(items[childIndexRigh])<0)
                    {
                        swapIndex = childIndexRigh;
                    }
                }

                if (item.CompareTo(items[swapIndex])<0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    void SortUp(T item) // Increasing the priorty of an item in the heap
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while(true)
        {
            T parentItem = items[parentIndex];

            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void Swap(T itemA, T itemB) //Function that swaps between 2 values from an array
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;

        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
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

