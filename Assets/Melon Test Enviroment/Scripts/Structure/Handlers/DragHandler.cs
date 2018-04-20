using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject draggedItem;
    Vector3 startPos;
    Transform startParent;
    CanvasGroup canvasGroup;

    private void Start() { canvasGroup = this.GetComponentInParent<CanvasGroup>(); }

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggedItem = this.gameObject;
        startPos = this.transform.position;
        startParent = this.transform.parent;

        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        draggedItem = null;

        canvasGroup.blocksRaycasts = true;
        transform.position = startPos;

        bool succ = canvasGroup.IsRaycastLocationValid(eventData.position, GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>());
        
        if (succ)
        {
            UI_INV_SLOT this_slot = this.gameObject.GetComponentInParent<UI_INV_SLOT>();
            UI_INV_SLOT dragged_slot = eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<UI_INV_SLOT>();

            if(this_slot == null) { return; }
            if(dragged_slot == null) { return; }

            GameObject inv = dragged_slot.gameObject.transform.parent.gameObject;
            
            InventoryHandler invHandler = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryHandler>();
            
            inventory_init _Init = inv.GetComponent<inventory_init>();
            Vector2 right_pos = new Vector2(-1,-1);
            foreach (var entry in _Init.slots)
            {
                GameObject key = entry.Value;
                UI_INV_SLOT slot = key.GetComponentInParent<UI_INV_SLOT>();
                Vector2Int pos = slot.pos;

                if(pos == dragged_slot.pos)
                {
                    right_pos = pos;
                    
                    invHandler.SwitchItemSlot(this_slot, slot);
                    break;
                }
            }
        }
        
    }
}
