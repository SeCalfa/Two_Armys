using System;

namespace CodeBase.Exceptions
{
    public class SingleSlotNotEmptyException : Exception
    {
        public SingleSlotNotEmptyException()
        {
        }

        public SingleSlotNotEmptyException(string message) : base(message)
        {
        }
    }
}
