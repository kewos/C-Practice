﻿namespace CSharpNote.Data.DesignPattern.Implement.FecadePattern
{
    public class SubClassA
    {
        public string DoSomeThing()
        {
            return GetType().Name;
        }
    }

    public class SubClassB
    {
        public string DoSomeThing()
        {
            return GetType().Name;
        }
    }
}