using System;

namespace CodeBase.Exceptions
{
    public class UnknownCountSlotsException : Exception
    {
        public UnknownCountSlotsException()
        {
        }

        public UnknownCountSlotsException(string message) : base(message)
        {
        }
    }
}
