﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace CSharpNote.Data.DataStructure.Implement.Buffer
{
    public class Buffer<T> : IBuffer<T>, IEnumerable<T>, IEnumerable
    {
        private readonly T[] buffer;
        private int top;

        public Buffer(int capacity = 10)
        {
            if (capacity <= 1)
            {
                throw new ArgumentException("InvalidArgument");
            }

            buffer = new T[capacity];
            top = -1;
        }

        public void Write(T value)
        {
            if (IsFull) throw new InvalidOperationException("Full");
            buffer[++top] = value;
        }

        public T Read()
        {
            if (IsEmpty) throw new InvalidOperationException("Empty");
            return buffer[top--];
        }

        #region IEnumerator<T> Member

        public IEnumerator<T> GetEnumerator()
        {
            for (var index = top; index != -1; index--)
            {
                yield return buffer[index];
            }
        }

        #endregion

        #region IEnumerator Member

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        public void Write(T[] items)
        {
            foreach (var item in items)
            {
                Write(item);
            }
        }

        public void Write(IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                Write(item);
            }
        }

        #region property

        public int Capacity
        {
            get { return buffer.Length; }
        }

        public bool IsEmpty
        {
            get { return top == -1; }
        }

        public bool IsFull
        {
            get { return top == Capacity - 1; }
        }

        #endregion
    }
}