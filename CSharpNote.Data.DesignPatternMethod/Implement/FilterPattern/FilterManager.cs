﻿using System;
using System.Collections.Generic;

namespace CSharpNote.Data.DesignPattern.Implement.FilterPattern
{
    public class FilterManager<TItem>
    {
        private readonly Dictionary<Type, IFilter<TItem>> filterDic;

        public FilterManager()
        {
            filterDic = new Dictionary<Type, IFilter<TItem>>();
        }

        public void AddFilter<TFilter>()
            where TFilter : IFilter<TItem>, new()
        {
            var type = typeof (TFilter);

            if (!filterDic.ContainsKey(type))
            {
                filterDic[type] = new TFilter();
            }
        }

        public IEnumerable<TItem> ExecuteFilter<TFilter>(IEnumerable<TItem> source)
            where TFilter : IFilter<TItem>, new()
        {
            var type = typeof (TFilter);

            if (!filterDic.ContainsKey(type))
            {
                filterDic[type] = new TFilter();
            }

            return filterDic[type].Filter(source);
        }
    }
}