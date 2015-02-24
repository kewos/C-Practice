﻿using System.Collections.Generic;
using System.Linq;
using CSharpNote.Common.Extendsions;
using CSharpNote.Core.Contracts;

namespace CSharpNote.Service.CSharpNoteService
{
    public class CSharperNoteService : ICSharperNoteService
    {
        private readonly IList<IMethodRepository> methodRepositories;

        #region constructor
        public CSharperNoteService(IEnumerable<IMethodRepository> methodRepositories)
        {
            this.methodRepositories = methodRepositories.ToList();
        }
        #endregion

        #region IRepositoryManager member
        public int Count
        {
            get
            {
                return methodRepositories.Count;
            }
        }

        public IEnumerable<string> GetRepositoryNames()
        {
            return methodRepositories.Select(repository => repository.RepositoryName);
        }

        public IMethodRepository this[int index]
        {
            get
            {
                index.AssertBetweenRange(0, Count - 1);

                return methodRepositories[index];
            }
        }
        #endregion
    }
}