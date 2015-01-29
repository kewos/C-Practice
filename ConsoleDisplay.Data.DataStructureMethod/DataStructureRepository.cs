﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleDisplay.Common;
using ConsoleDisplay.Core.Contracts;
using ConsoleDisplay.Core.Implements;
using ConsoleDisplay.Data.DataStructureMethod.SubClass;

namespace ConsoleDisplay.Data.DataStructureMethod
{
    public class DataStructureMethodRepository : AbstractMethodRepository
    {
        [DisplayMethod]
        public void HashTableImplement()
        {
            var hashTable = new HashTable<int, int>();
            var elements = Enumerable.Range(0, 1000);

            elements.ForEach(element => hashTable.Add(element, element));
            hashTable.Count.ToConsole("Add 1000 Items, Count:");

            hashTable[999].ToConsole("index:999, value:");

            elements.ForEach(element => hashTable.Remove(element));
            hashTable.Count.ToConsole("Remove 1000 Items, Count:");
        }
    }
}
