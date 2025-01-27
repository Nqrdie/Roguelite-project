using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryMenu;
    public static bool menuActive;
    public ItemSlot[] itemSlot;
    private InputHandler input;

    private void Start()
    {
        input = FindObjectOfType<InputHandler>();
    }
    public void OpenInventory()
    {
        if (menuActive)
        {
            inventoryMenu.SetActive(false);
            menuActive = false;
            DeselectAllSlots();
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (!menuActive)
        {
            inventoryMenu.SetActive(true);
            menuActive = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void Update()
    {
        if(input.inventoryTriggered && menuActive)
        {
            inventoryMenu.SetActive(false);
            menuActive = false;
            DeselectAllSlots();
        }
        else if (input.inventoryTriggered && !menuActive)
        {
            inventoryMenu.SetActive(true);
            menuActive = true;
        }
    }

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        for(int i = 0; i < itemSlot.Length; i++)
        {
            if(!itemSlot[i].isFull && itemSlot[i].itemName == itemName || itemSlot[i].quantity == 0)
            {
                int leftOverItems = itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription);
                if(leftOverItems > 0)
                {
                    leftOverItems = AddItem(itemName, leftOverItems, itemSprite, itemDescription);
                }
                return leftOverItems;
            }
        }
        return quantity;
    }

    public void DeselectAllSlots()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selected.SetActive(false);
            itemSlot[i].isSelected = false;
        }
    }
}
