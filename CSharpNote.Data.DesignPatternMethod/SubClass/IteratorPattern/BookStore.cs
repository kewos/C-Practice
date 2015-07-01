﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpNote.Data.DesignPatternMethod.SubClass.IteratorPattern
{
    public class BookStore : IAggregate<Book>
    {
        private readonly List<Book> books;

        public BookStore()
        {
            books = new List<Book>();
        }

        public void RegistBook(Book book)
        {
            books.Add(book);
        }

        public void RegistBook(IEnumerable<Book> books)
        {
            this.books.AddRange(books);
        }

        public IIterator<Book> GetIterator()
        {
            return new Iterator<Book>(books);
        }
    }
}
