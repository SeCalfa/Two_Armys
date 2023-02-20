using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase.Structure.SingleSlot
{
    public class Slot : MonoBehaviour, IPointerDownHandler
    {
        private SlotsContainer slotsContainer;

        [SerializeField]
        private GameObject border;
        [SerializeField]
        private GameObject lockImage;
        [Space]
        [SerializeField]
        private Sprite x1;
        [SerializeField]
        private Sprite x2;

        public SlotState slotState { get; private set; } = SlotState.Empty;
        public bool isLock { get; private set; } = false;

        public void Construct(SlotsContainer slotsContainer)
        {
            this.slotsContainer = slotsContainer;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (isLock)
                return;

            if (slotsContainer.IsSlotContains(this))
                slotsContainer.SlotAction(SlotType.Active, this);
            else
                slotsContainer.SlotAction(SlotType.Free, this);
        }

        public void SetX1()
        {
            GetComponent<Image>().sprite = x1;
            slotState = SlotState.X1;
        }

        public Slot SetX2()
        {
            GetComponent<Image>().sprite = x2;
            slotState = SlotState.X2;

            return this;
        }

        public void SetLock()
        {
            lockImage.SetActive(true);
            isLock = true;
        }

        public void RemoveLock()
        {
            lockImage.SetActive(false);
            isLock = false;
        }

        public void ClearSlot()
        {
            GetComponent<Image>().sprite = null;
            slotState = SlotState.Empty;
        }

        public void BorderOn() =>
            border.SetActive(true);

        public void BorderOff() =>
            border.SetActive(false);
    }
}