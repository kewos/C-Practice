﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpNote.Data.DesignPatternMethod.SubClass.IteratorPattern
{
    public interface IAggregate<TItem>
    {
        IIterator<TItem> GetIterator();
    }
}
    