using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    // ItemData
    public string itemName;
    public int quantity;
    public Sprite itemSprite;
    public bool isFull;
    public string itemDescription;
    public int maxItemStack = 999;

    // VisibleData
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private Image itemImage;

    // Selected
    public GameObject selected;
    public bool isSelected;

    // Item Description
    public Image itemDescriptionImage;
    public TMP_Text itemDescriptionNameText;
    public TMP_Text itemDescriptionText;

    private InventoryManager inventoryManager;

    private void Awake()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription)
    {
        if(isFull)
        {
            return quantity;
        }

        this.itemName = itemName;
        this.itemSprite = itemSprite;
        this.itemDescription = itemDescription;

        itemImage.sprite = itemSprite;


        this.quantity += quantity;
        if (this.quantity >= maxItemStack)
        {
            quantityText.text = maxItemStack.ToString();
            quantityText.enabled = true;
            isFull = true;

            int extraItems = this.quantity - maxItemStack;
            this.quantity = maxItemStack;
            return extraItems;
        }

        quantityText.text = this.quantity.ToString();
        quantityText.enabled = true;

        return 0;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    public void OnLeftClick()
    {
        inventoryManager.DeselectAllSlots();
        selected.SetActive(true);
        isSelected = true;
        itemDescriptionImage.sprite = itemSprite;
        itemDescriptionNameText.text = itemName;
        itemDescriptionText.text = itemDescription;
    }

    public void OnRightClick()
    {

    }
}
