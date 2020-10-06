using System;

namespace PhotosCategorier.DataStructure
{
    public class CircularQueue<T>
    {
        public int Capacity { get => Queue.Length; }

        public int Count
        {
            get => (RearIndex + Capacity - FrontIndex) % Capacity;
        }

        private readonly T[] Queue;

        private int FrontIndex = 0, RearIndex = 0;
        public CircularQueue() : this(4)
        {

        }

        public CircularQueue(int capatcity)
        {
            Queue = new T[capatcity];
        }

        public bool IsFull
        {
            get => (RearIndex + 1) % Capacity == FrontIndex;
        }

        public bool IsEmpty
        {
            get => RearIndex == FrontIndex;
        }

        public bool Enqueue(T value)
        {
            lock (this)
            {
                if (IsFull)
                {
                    return false;
                }
                Queue[RearIndex] = value;
                RearIndex = (RearIndex + 1) % Capacity;
                return true;
            }
        }

        public T Dequeue()
        {
            lock (this)
            {
                if (IsEmpty)
                {
                    throw new IndexOutOfRangeException();
                }
                T value = Queue[FrontIndex];
                FrontIndex = (FrontIndex + 1) % Capacity;
                return value;
            }
        }

        public T Peek()
        {
            if (IsEmpty)
            {
                throw new IndexOutOfRangeException();
            }
            return Queue[FrontIndex];
        }

        public void Clear()
        {
            FrontIndex = RearIndex = 0;
        }
    }
}