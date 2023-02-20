using CodeBase.Structure.SingleSlot;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Structure
{
    public class FunctionalButtons : MonoBehaviour
    {
        private SlotsContainer slotsContainer;
        private List<Slot> activeSlot;

        [SerializeField]
        private Button swap;
        [SerializeField]
        private Button delete;
        [SerializeField]
        private Button addx1;
        [SerializeField]
        private Button addx2;

        public void Construct(SlotsContainer slotsContainer, List<Slot> activeSlot)
        {
            this.slotsContainer = slotsContainer;
            this.activeSlot = activeSlot;
        }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            swap.onClick.AddListener(Swap);
            delete.onClick.AddListener(Delete);
            addx1.onClick.AddListener(Addx1);
            addx2.onClick.AddListener(Addx2);
        }

        private void Swap()
        {
            // Za pomoca tej listy(i z dodawaniem nowych warunkow "if"(takich jak w 49 linijce)) teoretycznie mozna rozbudowac mechanike i dodac mozliwosc
            // dodawania 3 i wiecej slotow. Dla przycisku Add x3 rowniez trzeba bedzie rozbudowac enum SlotState
            // :)
            List<Slot> slotsMemory;

            if ((activeSlot[0].slotState == SlotState.X1 && activeSlot[1].slotState == SlotState.X1) ||
                (activeSlot[0].slotState == SlotState.X2 && activeSlot[1].slotState == SlotState.X2))
            {
                Debug.LogWarning("Both selected slots are same");
                return;
            }

            if ((activeSlot[0].slotState == SlotState.Empty && activeSlot[1].slotState == SlotState.X1) ||
                (activeSlot[0].slotState == SlotState.X1 && activeSlot[1].slotState == SlotState.Empty))
            {
                if (activeSlot[0].slotState == SlotState.Empty)
                {
                    activeSlot[0].SetX1();
                    activeSlot[1].ClearSlot();
                }
                else if (activeSlot[0].slotState == SlotState.X1)
                {
                    activeSlot[0].ClearSlot();
                    activeSlot[1].SetX1();
                }
                else
                    Debug.LogError("Unexpected action");

                slotsContainer.UpdateButtonsAction();
                return;
            }

            if ((activeSlot[0].slotState == SlotState.Empty && activeSlot[1].slotState == SlotState.X2) ||
                (activeSlot[0].slotState == SlotState.X2 && activeSlot[1].slotState == SlotState.Empty))
            {
                if (activeSlot[0].slotState == SlotState.Empty)
                {
                    slotsMemory = new List<Slot>() { activeSlot[1], slotsContainer.GetRightSlot_x2(activeSlot[1]) };

                    activeSlot[1].ClearSlot();
                    slotsContainer.GetRightSlot_x2(activeSlot[1]).ClearSlot();
                    slotsContainer.GetRightSlot_x2(activeSlot[1]).RemoveLock();

                    Dictionary<Slot, SecondSlotPosition> nearbySlot = slotsContainer.GetEmptyNearbySlot(activeSlot[0]);
                    if (nearbySlot != null)
                    {
                        if (nearbySlot.ElementAt(0).Value == SecondSlotPosition.Right)
                        {
                            Slot active = activeSlot[0].SetX2();
                            nearbySlot.ElementAt(0).Key.SetX2();
                            nearbySlot.ElementAt(0).Key.SetLock();

                            OffAllActiveSlots();

                            slotsContainer.SlotAction(SlotType.Free, active);
                        }
                        else
                        {
                            Slot active = nearbySlot.ElementAt(0).Key.SetX2();
                            activeSlot[0].SetX2();
                            activeSlot[0].SetLock();

                            OffAllActiveSlots();

                            slotsContainer.SlotAction(SlotType.Free, active);
                        }

                        foreach (var s in slotsMemory)
                        {
                            if (s.slotState != SlotState.Empty)
                                continue;

                            slotsContainer.SlotAction(SlotType.Free, s);
                            break;
                        }
                    }
                    else
                    {
                        activeSlot[1].SetX2();
                        slotsContainer.GetRightSlot_x2(activeSlot[1]).SetX2();
                        slotsContainer.GetRightSlot_x2(activeSlot[1]).SetLock();

                        Debug.LogWarning("Impossible to swap");
                    }
                }
                else if (activeSlot[0].slotState == SlotState.X2)
                {
                    slotsMemory = new List<Slot>() { activeSlot[0], slotsContainer.GetRightSlot_x2(activeSlot[0]) };

                    activeSlot[0].ClearSlot();
                    slotsContainer.GetRightSlot_x2(activeSlot[0]).ClearSlot();
                    slotsContainer.GetRightSlot_x2(activeSlot[0]).RemoveLock();

                    Dictionary<Slot, SecondSlotPosition> nearbySlot = slotsContainer.GetEmptyNearbySlot(activeSlot[1]);
                    if (nearbySlot != null)
                    {
                        if (nearbySlot.ElementAt(0).Value == SecondSlotPosition.Right)
                        {
                            Slot active = activeSlot[1].SetX2();
                            nearbySlot.ElementAt(0).Key.SetX2();
                            nearbySlot.ElementAt(0).Key.SetLock();

                            OffAllActiveSlots();

                            slotsContainer.SlotAction(SlotType.Free, active);
                        }
                        else
                        {
                            Slot active = nearbySlot.ElementAt(0).Key.SetX2();
                            activeSlot[1].SetX2();
                            activeSlot[1].SetLock();

                            OffAllActiveSlots();

                            slotsContainer.SlotAction(SlotType.Free, active);
                        }

                        foreach (var s in slotsMemory)
                        {
                            if (s.slotState != SlotState.Empty)
                                continue;

                            slotsContainer.SlotAction(SlotType.Free, s);
                            break;
                        }
                    }
                    else
                    {
                        activeSlot[0].SetX2();
                        slotsContainer.GetRightSlot_x2(activeSlot[0]).SetX2();
                        slotsContainer.GetRightSlot_x2(activeSlot[0]).SetLock();

                        Debug.LogWarning("Impossible to swap");
                    }
                }
                else
                    Debug.LogError("Unexpected action");

                slotsContainer.UpdateButtonsAction();
                return;
            }

            if ((activeSlot[0].slotState == SlotState.X1 && activeSlot[1].slotState == SlotState.X2) ||
                (activeSlot[0].slotState == SlotState.X2 && activeSlot[1].slotState == SlotState.X1))
            {
                if (activeSlot[0].slotState == SlotState.X1)
                {
                    slotsMemory = new List<Slot>() { activeSlot[1], slotsContainer.GetRightSlot_x2(activeSlot[1]) };

                    activeSlot[0].ClearSlot();
                    activeSlot[1].ClearSlot();
                    slotsContainer.GetRightSlot_x2(activeSlot[1]).ClearSlot();
                    slotsContainer.GetRightSlot_x2(activeSlot[1]).RemoveLock();

                    Dictionary<Slot, SecondSlotPosition> nearbySlot = slotsContainer.GetEmptyNearbySlot(activeSlot[0]);
                    if (nearbySlot != null)
                    {
                        if (nearbySlot.ElementAt(0).Value == SecondSlotPosition.Right)
                        {
                            Slot active = activeSlot[0].SetX2();
                            nearbySlot.ElementAt(0).Key.SetX2();
                            nearbySlot.ElementAt(0).Key.SetLock();

                            OffAllActiveSlots();

                            slotsContainer.SlotAction(SlotType.Free, active);
                        }
                        else
                        {
                            Slot active = nearbySlot.ElementAt(0).Key.SetX2();
                            activeSlot[0].SetX2();
                            activeSlot[0].SetLock();

                            OffAllActiveSlots();

                            slotsContainer.SlotAction(SlotType.Free, active);
                        }

                        foreach (var s in slotsMemory)
                        {
                            if (s.slotState != SlotState.Empty)
                                continue;

                            slotsContainer.SlotAction(SlotType.Free, s);
                            s.SetX1();
                            break;
                        }
                    }
                    else
                    {
                        activeSlot[0].SetX1();
                        activeSlot[1].SetX2();
                        slotsContainer.GetRightSlot_x2(activeSlot[1]).SetX2();
                        slotsContainer.GetRightSlot_x2(activeSlot[1]).SetLock();

                        Debug.LogWarning("Impossible to swap");
                    }
                }
                else if (activeSlot[0].slotState == SlotState.X2)
                {
                    slotsMemory = new List<Slot>() { activeSlot[0], slotsContainer.GetRightSlot_x2(activeSlot[0]) };

                    activeSlot[1].ClearSlot();
                    activeSlot[0].ClearSlot();
                    slotsContainer.GetRightSlot_x2(activeSlot[0]).ClearSlot();
                    slotsContainer.GetRightSlot_x2(activeSlot[0]).RemoveLock();

                    Dictionary<Slot, SecondSlotPosition> nearbySlot = slotsContainer.GetEmptyNearbySlot(activeSlot[1]);
                    if (nearbySlot != null)
                    {
                        if (nearbySlot.ElementAt(0).Value == SecondSlotPosition.Right)
                        {
                            Slot active = activeSlot[1].SetX2();
                            nearbySlot.ElementAt(0).Key.SetX2();
                            nearbySlot.ElementAt(0).Key.SetLock();

                            OffAllActiveSlots();

                            slotsContainer.SlotAction(SlotType.Free, active);
                        }
                        else
                        {
                            Slot active = nearbySlot.ElementAt(0).Key.SetX2();
                            activeSlot[1].SetX2();
                            activeSlot[1].SetLock();

                            OffAllActiveSlots();

                            slotsContainer.SlotAction(SlotType.Free, active);
                        }

                        foreach (var s in slotsMemory)
                        {
                            if (s.slotState != SlotState.Empty)
                                continue;

                            slotsContainer.SlotAction(SlotType.Free, s);
                            s.SetX1();
                            break;
                        }
                    }
                    else
                    {
                        activeSlot[1].SetX1();
                        activeSlot[0].SetX2();
                        slotsContainer.GetRightSlot_x2(activeSlot[0]).SetX2();
                        slotsContainer.GetRightSlot_x2(activeSlot[0]).SetLock();

                        Debug.LogWarning("Impossible to swap");
                    }
                }
                else
                    Debug.LogError("Unexpected action");

                slotsContainer.UpdateButtonsAction();
                return;
            }
        }

        private void Delete()
        {
            if (activeSlot.Count == 1)
            {
                if (activeSlot[0].slotState == SlotState.X1)
                {
                    activeSlot[0].ClearSlot();
                }
                else if (activeSlot[0].slotState == SlotState.X2)
                {
                    activeSlot[0].ClearSlot();
                    slotsContainer.GetRightSlot_x2(activeSlot[0]).ClearSlot();
                    slotsContainer.GetRightSlot_x2(activeSlot[0]).RemoveLock();
                }
            }
            else if (activeSlot.Count == 2)
            {
                if (activeSlot[0].slotState == SlotState.X2)
                {
                    slotsContainer.GetRightSlot_x2(activeSlot[0]).ClearSlot();
                    slotsContainer.GetRightSlot_x2(activeSlot[0]).RemoveLock();
                }

                if (activeSlot[1].slotState == SlotState.X2)
                {
                    slotsContainer.GetRightSlot_x2(activeSlot[1]).ClearSlot();
                    slotsContainer.GetRightSlot_x2(activeSlot[1]).RemoveLock();
                }

                activeSlot[0].ClearSlot();
                activeSlot[1].ClearSlot();
            }
            else
                Debug.LogError("Active slots more than 2");

            slotsContainer.UpdateButtonsAction();
        }

        private void Addx1()
        {
            if (activeSlot.Count == 1)
            {
                activeSlot[0].SetX1();
            }
            else if (activeSlot.Count == 2)
            {
                activeSlot[0].SetX1();
                activeSlot[1].SetX1();
            }
            else
                Debug.LogError("Active slots more than 2");

            slotsContainer.UpdateButtonsAction();
        }

        private void Addx2()
        {
            if (activeSlot.Count == 1)
            {
                Dictionary<Slot, SecondSlotPosition> nearbySlot = slotsContainer.GetEmptyNearbySlot(activeSlot[0]);
                if (nearbySlot != null)
                {
                    activeSlot[0].SetX2();
                    nearbySlot.ElementAt(0).Key.SetX2();

                    if(nearbySlot.ElementAt(0).Value == SecondSlotPosition.Right)
                    {
                        nearbySlot.ElementAt(0).Key.SetLock();
                    }
                    else
                    {
                        activeSlot[0].SetLock();
                        slotsContainer.SlotAction(SlotType.Active, activeSlot[0]);
                        slotsContainer.SlotAction(SlotType.Free, nearbySlot.ElementAt(0).Key);
                    }
                }
                else
                    Debug.LogWarning("Impossible to addx2");
            }
            else if (activeSlot.Count == 2)
            {
                if (slotsContainer.GetIsSlotsNearby(activeSlot[0], activeSlot[1]))
                {
                    int s1_index = slotsContainer.GetSlotIndex(activeSlot[0]);
                    int s2_index = slotsContainer.GetSlotIndex(activeSlot[1]);

                    activeSlot[0].SetX2();
                    activeSlot[1].SetX2();

                    if(s1_index < s2_index)
                    {
                        activeSlot[1].SetLock();
                        slotsContainer.SlotAction(SlotType.Active, activeSlot[1]);
                    }
                    else
                    {
                        activeSlot[0].SetLock();
                        slotsContainer.SlotAction(SlotType.Active, activeSlot[0]);
                    }
                }
                else
                {
                    Dictionary<Slot, SecondSlotPosition> nearbySlot1;
                    Dictionary<Slot, SecondSlotPosition> nearbySlot2;

                    nearbySlot1 = slotsContainer.GetEmptyNearbySlot(activeSlot[0]);

                    if (nearbySlot1 != null)
                    {
                        activeSlot[0].SetX2();
                        nearbySlot1.ElementAt(0).Key.SetX2();

                        if (nearbySlot1.ElementAt(0).Value == SecondSlotPosition.Right)
                        {
                            nearbySlot1.ElementAt(0).Key.SetLock();
                        }
                        else
                        {
                            activeSlot[0].SetLock();
                            slotsContainer.SlotAction(SlotType.Active, activeSlot[0]);
                            slotsContainer.SlotAction(SlotType.Free, nearbySlot1.ElementAt(0).Key, 0);
                        }
                    }
                    else
                        Debug.LogWarning("Impossible to addx2 in slot 1");

                    nearbySlot2 = slotsContainer.GetEmptyNearbySlot(activeSlot[1]);

                    if (nearbySlot2 != null)
                    {
                        activeSlot[1].SetX2();
                        nearbySlot2.ElementAt(0).Key.SetX2();

                        if (nearbySlot2.ElementAt(0).Value == SecondSlotPosition.Right)
                        {
                            nearbySlot2.ElementAt(0).Key.SetLock();
                        }
                        else
                        {
                            activeSlot[1].SetLock();
                            slotsContainer.SlotAction(SlotType.Active, activeSlot[1]);
                            slotsContainer.SlotAction(SlotType.Free, nearbySlot2.ElementAt(0).Key, 1);
                        }
                    }
                    else
                        Debug.LogWarning("Impossible to addx2 in slot 2");
                }
            }
            else
                Debug.LogError("Active slots more than 2");

            slotsContainer.UpdateButtonsAction();
        }

        private void OffAllActiveSlots()
        {
            slotsContainer.SlotAction(SlotType.Active, activeSlot[1]);
            slotsContainer.SlotAction(SlotType.Active, activeSlot[0]);
        }

        public void SwapInteract(bool isInteractable) =>
            swap.interactable = isInteractable;

        public void DeleteInteract(bool isInteractable) =>
            delete.interactable = isInteractable;

        public void Add1Interact(bool isInteractable) =>
            addx1.interactable = isInteractable;

        public void Add2Interact(bool isInteractable) =>
            addx2.interactable = isInteractable;
    }
}