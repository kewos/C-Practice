﻿using System;

namespace CSharpNote.Data.DesignPatternMethod.SubClass.ChainResponsibilityPattern
{
    public abstract class AbstractHandler : IHandler
    {
        private readonly IHandler nextHandler;

        protected AbstractHandler(IHandler handler)
        {
            nextHandler = handler;
        }

        public void Execute(IHandlerCommand handlerCommand)
        {
            if (handlerCommand == null)
                throw new ArgumentNullException("IHandlerCommandIsNull");

            if ((handlerCommand.CommandType == GetType()))
                DoSometing();
            else
                NextProcess(handlerCommand);
        }

        private void NextProcess(IHandlerCommand handlerCommand)
        {
            if (!HasNextProcess)
                throw new ArgumentNullException("NoFindMatchHandler");

            nextHandler.Execute(handlerCommand);
        }

        private bool HasNextProcess
        {
            get { return (nextHandler != null); }
        }

        protected abstract void DoSometing();
    }
}