﻿using System.Collections.Generic;

namespace CSharpNote.Data.DesignPattern.Implement.VistorPattern
{
    public class BuffetDinner
    {
        private readonly IList<IFood> elements;

        public BuffetDinner()
        {
            elements = new List<IFood>();
        }

        public void Attach(IFood element)
        {
            elements.Add(element);
        }

        public void Detach(IFood element)
        {
            elements.Remove(element);
        }

        public void Accept(IVisitor visitor)
        {
            foreach (var food in elements)
            {
                food.Accept(visitor);
            }
        }
    }
}