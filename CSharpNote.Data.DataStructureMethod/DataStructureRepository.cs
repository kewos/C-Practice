﻿using System;
using System.Collections.Generic;
using System.Linq;
using CSharpNote.Common.Attributes;
using CSharpNote.Common.Extendsions;
using CSharpNote.Core.Implements;
using CSharpNote.Data.DataStructureMethod.SubClass.Buffer;
using CSharpNote.Data.DataStructureMethod.SubClass.HashTable;
using CSharpNote.Data.DataStructureMethod.SubClass.Queue;
using CSharpNote.Data.DataStructureMethod.SubClass.ORM;
using CSharpNote.Data.DataStructureMethod.SubClass.Tree;

namespace CSharpNote.Data.DataStructureMethod
{
    [MarkedRepositoryAttribue]
    public class DataStructureMethodRepository : AbstractMethodRepository
    {
        [MarkedItem]
        public void HashTable()
        {
            var hashTable = new HashTable<int, int>();
            var elements = Enumerable.Range(0, 1000);

            elements.ForEach(element => hashTable.Add(element, element));
            (hashTable.Count == 1000).ToConsole("Add 1000 Items, Count is 1000");

            (hashTable[999] == 999).ToConsole("index:999 value is 999:");

            elements.ForEach(element => hashTable.Remove(element));
            (hashTable.Count == 0).ToConsole("Remove 1000 Items, Count is 0:");
        }

        [MarkedItem]
        public void CircularQueue()
        {
            var circularQueue = new CircularQueue<int>(3).Enqueue(1).Enqueue(2).Enqueue(3);
            var assert = new List<int> { 1, 2, 3 };
            circularQueue.All((index, element) => element == assert[index]).ToConsole("elements is {1, 2, 3}:");
            (circularQueue.Count == 3).ToConsole("Count is 3:");

            "\n".ToConsole();
            var circularQueue1 = new CircularQueue<int>(3).Enqueue(1).Enqueue(2).Enqueue(3).Enqueue(4);
            var assert1 = new List<int> { 2, 3, 4 };
            circularQueue1.All((index, element) => element == assert1[index]).ToConsole("elements is {2, 3, 4}:");
            (circularQueue1.Count == 3).ToConsole("Count is 3:");

            "\n".ToConsole();
            var circularQueue2 = new CircularQueue<int>(3).Enqueue(2).Enqueue(3).Enqueue(4);
            (circularQueue2.Dequeue() == 2).ToConsole("element is 2:");
            var assert2 = new List<int> { 3, 4 };
            circularQueue2.All((index, element) => element == assert2[index]).ToConsole("elements is {3, 4}:");

            "\n".ToConsole();
            var circularQueue3 = new CircularQueue<int>(3).Enqueue(3).Enqueue(4);
            (circularQueue3.Peek() == 3).ToConsole("element is 3:");
            var assert3 = new List<int> { 3, 4 };
            circularQueue3.All((index, element) => element == assert3[index]).ToConsole("elements is {3, 4}:");

            "\n".ToConsole();
            var circularQueue4 = new CircularQueue<int>(3);
            var assert4 = new List<int> { };
            circularQueue4.All((index, element) => element == assert4[index]).ToConsole("elements is {}:");

            "\n".ToConsole();
            try
            {
                var circularQueue5 = new CircularQueue<int>(3);
                circularQueue5.Dequeue();
            }
            catch (Exception e)
            {
                (e is InvalidOperationException).ToConsole("throw InvalidOperationException:");
            }
        }

        [MarkedItem]
        public void Deque()
        {
            var deque = new Deque<int>(Enumerable.Range(5, 2)).EnqueueHead(1).EnqueueHead(2).EnqueueTail(3).EnqueueTail(4);
            var assert  = new List<int> { 2, 1, 5, 6, 3, 4 };
            deque.All((index, element) => element == assert[index]).ToConsole("elements is { 2, 1, 5, 6, 3, 4 } :");
            
            "\n".ToConsole();
            var deque1 = new Deque<int>(Enumerable.Range(5, 2)).EnqueueHead(1).EnqueueHead(2).EnqueueTail(3).EnqueueTail(4);
            (deque1.DequeueHead() == 2).ToConsole("DequeueHead Get Head 2");
            var assert1 = new List<int> { 1, 5, 6, 3, 4 };
            deque1.All((index, element) => element == assert1[index]).ToConsole("elements is { 1, 5, 6, 3, 4 } :");
            
            "\n".ToConsole();
            var deque2 = new Deque<int>(Enumerable.Range(5, 2)).EnqueueHead(1).EnqueueTail(3).EnqueueTail(4);
            (deque2.DequeueTail() == 4).ToConsole("DequeueTail Get Head 4");
            var assert2 = new List<int> { 1, 5, 6, 3 };
            deque2.All((index, element) => element == assert2[index]).ToConsole("elements is { 1, 5, 6, 3 } :");
        }

        [MarkedItem]
        public void CircularBuffer()
        { 
            var circularBuffer = new CircularBuffer<int>(3);
            circularBuffer.Write(Enumerable.Range(0, 3));
            var assert = new List<int> { 0, 1, 2 };
            (circularBuffer.Count() == 3).ToConsole("Count is 3:");
            circularBuffer.All((index, element) => assert[index] == element).ToConsole("elements is {0, 1, 2}");

            "\n".ToConsole();
            var circularBuffer1 = new CircularBuffer<int>(3);
            circularBuffer1.Write(Enumerable.Range(0, 4));
            var assert1 = new List<int> { 1, 2, 3 };
            (circularBuffer1.Count() == 3).ToConsole("Count is 3:");
            circularBuffer1.All((index, element) => assert1[index] == element).ToConsole("elements is { 1, 2, 3 }");

            "\n".ToConsole();
            var circularBuffer2 = new CircularBuffer<int>(3);
            circularBuffer2.Write(Enumerable.Range(0, 3));
            var assert2 = new List<int> { 1, 2 };
            (circularBuffer2.Read() == 0).ToConsole("Read is 0:");
            (circularBuffer2.Count() == 2).ToConsole("Count is 2:");
            circularBuffer2.All((index, element) => assert2[index] == element).ToConsole("elements is {1, 2}");
        }

        [MarkedItem]
        public void Buffer()
        {
            var buffer = new Buffer<int>(3);
            buffer.Write(Enumerable.Range(0, 3));
            var assert = new List<int> { 2, 1, 0 };
            (buffer.Count() == 3).ToConsole("Count is 3:");
            buffer.All((index, element) => assert[index] == element).ToConsole("elements is {2, 1, 0}");

            "\n".ToConsole();
            try
            {
                var buffer1 = new Buffer<int>(3);
                buffer1.Write(Enumerable.Range(0, 4));
            }
            catch (Exception e)
            {
                (e is InvalidOperationException).ToConsole("ThrowInvalidOperationException:");
            }

            "\n".ToConsole();
            try
            {
                var buffer2 = new Buffer<int>(3);
                buffer2.Read();
            }
            catch (Exception e)
            {
                (e is InvalidOperationException).ToConsole("ThrowInvalidOperationException:");
            }

            var buffer3 = new Buffer<int>(3);
            buffer3.Write(Enumerable.Range(0, 1));
            (buffer3.Read() == 0).ToConsole("Read Item = 0:");
            (buffer3.Count() == 0).ToConsole("Count = 0:");
        }

        [MarkedItem]
        public void ORM()
        {
            var db = new CustomerFactory().GetCustoms();
            var result = new List<Customer>();

            foreach (var row in db)
            {
                var customer = new Customer();
                foreach (var property in customer.GetType().GetProperties())
                {
                    Type[] typeArgs = { typeof(string), property.GetType() };
                    var converter = typeof(TypeConverterFactory).GetMethod("GetConverter").MakeGenericMethod(typeArgs).Invoke(null, null) as dynamic;
                    var value = converter.Convert(row.GetValueOrDefault(property.Name));
                    property.SetValue(customer, value, null);
                }
                result.Add(customer);
            }
        }

        [MarkedItem]
        public void BinaryTree()
        {
            var n7 = new TreeNode<int>(7);
            var n6 = new TreeNode<int>(6);
            var n5 = new TreeNode<int>(5);
            var n4 = new TreeNode<int>(4);
            var n3 = new TreeNode<int>(3, n7, null);
            var n2 = new TreeNode<int>(2, n5, n6);
            var n1 = new TreeNode<int>(1, n3, n4);
            var n0 = new TreeNode<int>(0, n1, n2);

            //foreach (var i in BinaryTree<int>.PreOrderWithStack(n0))
            //{
            //    i.ToConsole();
            //}

            foreach (var i in BinaryTree<int>.InOrderWithStack(n0))
            {
                i.ToConsole();
            }
        }
    }

    public class TypeConverterFactory
    {
        public static IConverter<TInput, TOutput> GetConverter<TInput, TOutput>()
        {
            Type[] typeArgs = { typeof(TInput) };

            if (typeof(TInput) == typeof(int))
            {
                return Activator.CreateInstance(typeof(IntegerConverter<>)
                    .MakeGenericType(typeArgs)) as dynamic;
            }
            if (typeof(TInput) == typeof(string))
            {
                return Activator.CreateInstance(typeof(StringConverter<>)
                    .MakeGenericType(typeArgs)) as dynamic;
            }
            return null;
        }
    }

    public interface IConverter<TInput, TOutput>
    {
        TOutput DefaultValue { get; }
        TOutput Convert(TInput input);
    }

    public class StringConverter<TInput> : IConverter<TInput, string>
    {
        
        public string DefaultValue
        {
            get { return ""; }
        }

        public string Convert(TInput input)
        {
            if (input == null)
                return DefaultValue;

            return input.ToString();
        }
    }

    public class IntegerConverter<TInput> : IConverter<TInput, int>
    {
        public int DefaultValue
        {
            get { return 0; }
        }

        public int Convert(TInput input)
        {
            if (input == null)
                return DefaultValue;

            return System.Convert.ToInt32(input);
        }
    }
}
