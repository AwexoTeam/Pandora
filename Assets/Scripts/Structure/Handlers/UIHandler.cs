using GameDefinations;
using UnityEngine;

public enum UI_STATES
{
    Main_Inventory,
    Character_Info,
}

public class UIHandler : MonoBehaviour
{
    private EntityController entityController;
    private FileDatabase fileDatabase;
    private GameDatabase gameDatabase;
    public Sprite item_slot;
    public bool shitAd = true;
    public string currState = "";
    
    private void Start()
    {
        entityController = GetComponent<EntityController>();
        fileDatabase = GameObject.FindGameObjectWithTag("ModLoader").GetComponent<FileDatabase>();
        gameDatabase = GameObject.FindGameObjectWithTag("ModLoader").GetComponent<GameDatabase>();

        UI_STATES[] states = Utils.GetEnumArray<UI_STATES>();
        for (int i = 0; i < states.Length; i++)
        {
            gameDatabase.RegisterUIState(states[i].ToString().ToLower());
        }
    }

    private void OnGUI()
    {
        if (currState == "main_inventory")
        {
            if (shitAd)
            {
                
                entityController.inventory.AddItem(gameDatabase.Item_Database[1], 300);
                entityController.inventory.AddItem(gameDatabase.Item_Database[1], 1800);
                entityController.inventory.AddItem(gameDatabase.Item_Database[0], 1);
                shitAd = false;
            }
            DrawInventory();
        }
        else if (currState == "") { }
    }

    private void DrawInventory()
    {
        InventoryHandler inventoryHandler = entityController.inventory;
        
        float x_size = 0;
        x_size = inventoryHandler.inv_width * UI_CONSTS.INV_SLOT_OFFSET;
        x_size -= UI_CONSTS.INV_SLOT_OFFSET;
        
        float y_size = 0;
        y_size = (inventoryHandler.inv_height) * UI_CONSTS.INV_SLOT_OFFSET;
        y_size += UI_CONSTS.HOTKEY_BAR_OFFSET;
        y_size -= UI_CONSTS.INV_SLOT_OFFSET;
        
        float x_pos = Screen.width / 2 - x_size / 2;
        x_pos -= UI_CONSTS.INV_SLOT_SIZE.x / 2; 
        
        float y_pos = Screen.height / 2 - y_size / 2;
        y_pos -= UI_CONSTS.INV_SLOT_SIZE.y / 2;
        
        Vector2 startPos = new Vector2(x_pos, y_pos);

        for (int x = 0; x < inventoryHandler.inv_width; x++)
        {
            for (int y = 0; y < inventoryHandler.inv_height; y++)
            {
                float x_Offset = UI_CONSTS.INV_SLOT_OFFSET * x;
                float y_Offset = UI_CONSTS.INV_SLOT_OFFSET * y;

                Vector2 position = new Vector2(startPos.x + x_Offset, startPos.y + y_Offset);
                Rect rect = new Rect(position, UI_CONSTS.INV_SLOT_SIZE);

                GUI.DrawTexture(rect, item_slot.texture);

                if (inventoryHandler.inventory[x, y].item.isNull())
                {
                    GUI.DrawTexture(rect, item_slot.texture);
                }
                else
                {
                    string img_name = inventoryHandler.inventory[x, y].item.image_name;
                    if (img_name != string.Empty)
                    {
                        Sprite img;
                        bool sucess = fileDatabase.UI_Database.TryGetValue(img_name, out img);

                        if (sucess) { GUI.DrawTexture(rect, img.texture); }
                        else { GUI.DrawTexture(rect, item_slot.texture); }

                    }
                }

                ItemEntity currItem = inventoryHandler.inventory[x, y];
                if(currItem.isInitalized)
                {
                    GUI.Label(rect, currItem.count.ToString());
                }
            }
        }

        for (int x = 0; x < inventoryHandler.inv_width; x++)
        {
            float x_Offset = UI_CONSTS.INV_SLOT_OFFSET * x;
            float y_Offset = UI_CONSTS.INV_SLOT_OFFSET * inventoryHandler.inv_height + 1;

            Vector2 position = new Vector2(startPos.x + x_Offset, startPos.y + y_Offset + UI_CONSTS.HOTKEY_BAR_OFFSET);
            Rect rect = new Rect(position, UI_CONSTS.INV_SLOT_SIZE);

            GUI.DrawTexture(rect, item_slot.texture);
        }
    }
}
