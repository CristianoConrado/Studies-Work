using System;
namespace StudiesWork.DataModifiers
{
   public sealed class Heap<T> where T : IHeapItem<T>
   {
      private readonly T[] items;
      private int currentItemCount = 0;
      public Heap(int maxHeapSize) => items = new T[maxHeapSize];
      public void Add(T item)
      {
         item.HeapIndex = currentItemCount;
         items[currentItemCount] = item;
         SortUp(item);
         currentItemCount++;
      }
      public T RemoveFirst()
      {
         T firstItem = items[0];
         currentItemCount--;
         items[0] = items[currentItemCount];
         items[0].HeapIndex = 0;
         SortDown(items[0]);
         return firstItem;
      }
      public void UpdateItem(T item) => SortUp(item);
      public int Count => currentItemCount;
      public bool Contains(T item) => Equals(items[item.HeapIndex], item);
      private void SortDown(T item)
      {
			int childLeft;
			int childRight;
			int swapIndex;
			while (true)
         {
				childLeft = item.HeapIndex * 2 + 1;
				childRight = item.HeapIndex * 2 + 2;
            if(childLeft < currentItemCount)
            {
               swapIndex = childLeft;
               if (childRight < currentItemCount)
						if (items[childLeft].CompareTo(items[childRight]) < 0)
							swapIndex = childRight;
               if (item.CompareTo(items[swapIndex]) < 0)
                  Swap(item, items[swapIndex]);
               else
                  return;
            }
            else
               return;
         }
      }
      private void SortUp(T item)
      {
         T parentItem;
         int parentIndex;
         while (true)
         {
            parentIndex = (item.HeapIndex - 1) / 2;
            parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
               Swap(item, parentItem);
            else
               break;
         }
      }
      private void Swap(T itemA, T itemB)
      {
         items[itemA.HeapIndex] = itemB;
         items[itemB.HeapIndex] = itemA;
         (itemB.HeapIndex, itemA.HeapIndex) = (itemA.HeapIndex, itemB.HeapIndex);
      }
   };
   public interface IHeapItem<T> : IComparable<T>
   {
      public int HeapIndex { get; set; }
   };
};
