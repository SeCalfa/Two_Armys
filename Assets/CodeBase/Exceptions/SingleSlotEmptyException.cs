using System;

namespace CodeBase.Exceptions
{
    public class SingleSlotEmptyException : Exception
    {
        public SingleSlotEmptyException()
        {
        }

        public SingleSlotEmptyException(string message) : base(message)
        {
        }
    }
}
