using CodeBase.Exceptions;
using CodeBase.Logic;
using CodeBase.Structure.SingleSlot;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Structure
{
    public class SlotsContainer : MonoBehaviour
    {
        [SerializeField]
        private List<Slot> army1;
        [SerializeField]
        private List<Slot> army2;
        [SerializeField]
        private FunctionalButtons functionalButtons;

        private List<Slot> activeSlot;
        private ButtonsActionDistributor actionsDistributor;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            foreach (var slot in army1)
                slot.Construct(this);

            foreach (var slot in army2)
                slot.Construct(this);

            activeSlot = new List<Slot>();
            actionsDistributor = new ButtonsActionDistributor(activeSlot);

            functionalButtons.Construct(this, activeSlot);
        }

        public void SlotAction(SlotType slotType, Slot slot, int? insertIndex = null)
        {
            if(slotType == SlotType.Active)
            {
                activeSlot.Remove(slot);
                slot.BorderOff();
            }
            else if (slotType == SlotType.Free)
            {
                if (activeSlot.Count >= 2)
                {
                    activeSlot[0].BorderOff();
                    activeSlot.Remove(activeSlot[0]);
                }

                if (insertIndex == null)
                    activeSlot.Add(slot);
                else
                    activeSlot.Insert((int)insertIndex, slot);

                slot.BorderOn();
            }

            UpdateButtonsAction();
        }

        public void UpdateButtonsAction()
        {
            try
            {
                actionsDistributor.UpdateButtonsAction();
            }
            catch (DoubleSlotException e)
            {
                if (e.firstSlot == 0 && e.secondSlot == 0)
                {
                    functionalButtons.SwapInteract(false);
                    functionalButtons.DeleteInteract(false);
                    functionalButtons.Add1Interact(true);
                    functionalButtons.Add2Interact(true);
                }
                else if ((e.firstSlot != 0 && e.secondSlot != 0) || (e.firstSlot == 0 && e.secondSlot != 0) || (e.firstSlot != 0 && e.secondSlot == 0))
                {
                    functionalButtons.SwapInteract(true);
                    functionalButtons.DeleteInteract(true);
                    functionalButtons.Add1Interact(false);
                    functionalButtons.Add2Interact(false);
                }
                else
                    Debug.LogError("Unexpected combination");

                Debug.Log(e.Message);
            }
            catch (SingleSlotEmptyException e)
            {
                functionalButtons.SwapInteract(false);
                functionalButtons.DeleteInteract(false);
                functionalButtons.Add1Interact(true);
                functionalButtons.Add2Interact(true);

                Debug.Log(e.Message);
            }
            catch (SingleSlotNotEmptyException e)
            {
                functionalButtons.SwapInteract(false);
                functionalButtons.DeleteInteract(true);
                functionalButtons.Add1Interact(false);
                functionalButtons.Add2Interact(false);

                Debug.Log(e.Message);
            }
            catch (EmptySlotsException e)
            {
                functionalButtons.SwapInteract(false);
                functionalButtons.DeleteInteract(false);
                functionalButtons.Add1Interact(false);
                functionalButtons.Add2Interact(false);

                Debug.Log(e.Message);
            }
            catch (UnknownCountSlotsException e)
            {
                Debug.LogError(e.Message);
            }
        }

        public Dictionary<Slot, SecondSlotPosition> GetEmptyNearbySlot(Slot current)
        {
            int currentIndex;
            Dictionary<Slot, SecondSlotPosition> result = new Dictionary<Slot, SecondSlotPosition>();

            currentIndex = army1.FindIndex(c => c == current);
            if (currentIndex != -1)
            {
                if (currentIndex == 0)
                {
                    if (army1[1].slotState == SlotState.Empty)
                        result.Add(army1[1], SecondSlotPosition.Right);
                    else
                        return null;

                    return result;
                }

                if (currentIndex == army1.Count - 1)
                {
                    if (army1[army1.Count - 2].slotState == SlotState.Empty)
                        result.Add(army1[army1.Count - 2], SecondSlotPosition.Left);
                    else
                        return null;

                    return result;
                }

                if (army1[currentIndex + 1].slotState == SlotState.Empty)
                    result.Add(army1[currentIndex + 1], SecondSlotPosition.Right);
                else if (army1[currentIndex - 1].slotState == SlotState.Empty)
                    result.Add(army1[currentIndex - 1], SecondSlotPosition.Left);

                if (result.Count == 1)
                    return result;
                else
                    return null;
            }

            currentIndex = army2.FindIndex(c => c == current);
            if (currentIndex != -1)
            {
                if (currentIndex == 0)
                {
                    if (army2[1].slotState == SlotState.Empty)
                        result.Add(army2[1], SecondSlotPosition.Right);
                    else
                        return null;

                    return result;
                }

                if (currentIndex == army2.Count - 1)
                {
                    if (army2[army2.Count - 2].slotState == SlotState.Empty)
                        result.Add(army2[army2.Count - 2], SecondSlotPosition.Left);
                    else
                        return null;

                    return result;
                }

                if (army2[currentIndex + 1].slotState == SlotState.Empty)
                    result.Add(army2[currentIndex + 1], SecondSlotPosition.Right);
                else if (army2[currentIndex - 1].slotState == SlotState.Empty)
                    result.Add(army2[currentIndex - 1], SecondSlotPosition.Left);

                if (result.Count == 1)
                    return result;
                else
                    return null;
            }

            return null;
        }

        public bool GetIsSlotsNearby(Slot slot1, Slot slot2)
        {
            if(army1.Contains(slot1) && army1.Contains(slot2))
            {
                int s1_index = army1.FindIndex(s => s == slot1);
                int s2_index = army1.FindIndex(s => s == slot2);

                if (Mathf.Abs(s1_index - s2_index) == 1)
                    return true;
                else
                    return false;
            }

            if (army2.Contains(slot1) && army2.Contains(slot2))
            {
                int s1_index = army2.FindIndex(s => s == slot1);
                int s2_index = army2.FindIndex(s => s == slot2);

                if (Mathf.Abs(s1_index - s2_index) == 1)
                    return true;
                else
                    return false;
            }

            return false;
        }

        public int GetSlotIndex(Slot slot)
        {
            if (army1.Contains(slot))
                return army1.FindIndex(s => s == slot);
            else if (army2.Contains(slot))
                return army2.FindIndex(s => s == slot);
            else
            {
                Debug.LogError("Slot does not exist in any army");
                return -1;
            }
        }

        public Slot GetRightSlot_x2(Slot slot)
        {
            if (army1.Contains(slot))
                return army1[army1.FindIndex((s) => s == slot) + 1];
            else if (army2.Contains(slot))
                return army2[army2.FindIndex((s) => s == slot) + 1];
            else
            {
                Debug.LogError("Slot does not exist in any army");
                return null;
            }
        }

        public bool IsSlotContains(Slot slot) =>
            activeSlot.Contains(slot);
    }
}