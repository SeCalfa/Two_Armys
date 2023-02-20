using CodeBase.Exceptions;
using CodeBase.Structure.SingleSlot;
using System.Collections.Generic;

namespace CodeBase.Logic
{
    public class ButtonsActionDistributor
    {
        private readonly List<Slot> slots;
        
        public ButtonsActionDistributor(List<Slot> slots)
        {
            this.slots = slots;
        }

        public void UpdateButtonsAction()
        {
            if (slots.Count == 1)
            {
                if (slots[0].slotState == SlotState.Empty)
                    throw new SingleSlotEmptyException("Slot is empty");
                else
                    throw new SingleSlotNotEmptyException("Slot is not empty");
            }
            else if (slots.Count == 2)
            {
                int firstSlot = (int)slots[0].slotState;
                int secondSlot = (int)slots[1].slotState;

                throw new DoubleSlotException($"First slot: {firstSlot}. Second slot: {secondSlot}", firstSlot, secondSlot);
            }
            else if (slots.Count == 0)
                throw new EmptySlotsException("No one slots selected");
            else
                throw new UnknownCountSlotsException("Slots count more than 2");
        }
    }
}
