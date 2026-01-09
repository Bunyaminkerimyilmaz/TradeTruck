using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderSlotUI : MonoBehaviour
{
    [Header("UI Bileşenleri")]
    public Image iconImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI durationText;
    public Button addButton; 

    private OrderManager.ShopItem _shopItem;
    private OrderManager _manager;

    
    public void Setup(OrderManager.ShopItem item, OrderManager manager)
    {
        _shopItem = item;
        _manager = manager;

        
        if (iconImage != null) iconImage.sprite = item.itemData.icon; 
        nameText.text = item.itemData.itemName;
        priceText.text = "$ " + item.price;
        
        if (item.travelDays == 0)
            durationText.text = "Şehir İçi (0 Gün)";
        else
            durationText.text = "Konum: " + item.travelDays + " Gün Mesafe";

        
        addButton.onClick.RemoveAllListeners();
        addButton.onClick.AddListener(() => _manager.AddToCart(_shopItem));
    }
}
