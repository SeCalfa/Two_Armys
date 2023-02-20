using System;

namespace CodeBase.Exceptions
{
    public class EmptySlotsException : Exception
    {
        public EmptySlotsException()
        {
        }

        public EmptySlotsException(string message) : base(message)
        {
        }
    }
}
