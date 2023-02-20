using System;

namespace CodeBase.Exceptions
{
    public class DoubleSlotException : Exception
    {
        public readonly int firstSlot;
        public readonly int secondSlot;

        public DoubleSlotException()
        {
        }

        public DoubleSlotException(string message, int firstSlot, int secondSlot) : base(message)
        {
            this.firstSlot = firstSlot;
            this.secondSlot = secondSlot;
        }
    }
}
