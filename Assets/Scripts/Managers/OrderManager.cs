using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    [System.Serializable]
    public class ShopItem
    {
        public ItemData itemData;
        public int price;
        public int travelDays;
    }

    public class CartItem
    {
        public ShopItem shopItem;
        public int quantity;
    }

    [Header("Dükkan Verileri")] public List<ShopItem> shopItems;

    private List<CartItem> shoppingCart = new List<CartItem>();

    [Header("UI Referansları")] public GameObject orderPanel;
    public Transform contentParent;
    public GameObject slotPrefab;

    [Header("Özet UI")] public TextMeshProUGUI totalCostText;
    public TextMeshProUGUI totalDaysText;
    public TextMeshProUGUI cartSummaryText;

    [Header("Oyuncu Kontrolü")] public MouseLook mouseLook; // Inspector'dan Player'ı sürüklemeyi unutma!

    private void Awake()
    {
        Instance = this;
    }

    // AÇMA KAPAMA MANTIĞINI BURADA MERKEZİLEŞTİRDİK
    public void ToggleOrderPanel(bool isOpen)
    {
        orderPanel.SetActive(isOpen);

        if (isOpen)
        {
            GenerateShop();
            ClearCart();

            // Mouse GÖRÜNSÜN, Karakter DÖNMESİN
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (mouseLook != null) mouseLook.canLook = false;
        }
        else
        {
            // Mouse KİLİTLENSİN, Karakter DÖNSÜN
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (mouseLook != null) mouseLook.canLook = true;
        }
    }

    void GenerateShop()
    {
        foreach (Transform child in contentParent) Destroy(child.gameObject);

        foreach (var item in shopItems)
        {
            GameObject newSlot = Instantiate(slotPrefab, contentParent);
            newSlot.GetComponent<OrderSlotUI>().Setup(item, this);
        }
    }

    public void AddToCart(ShopItem itemToAdd)
    {
        CartItem existingItem = shoppingCart.Find(x => x.shopItem == itemToAdd);

        if (existingItem != null)
        {
            existingItem.quantity++;
        }
        else
        {
            CartItem newItem = new CartItem();
            newItem.shopItem = itemToAdd;
            newItem.quantity = 1;
            shoppingCart.Add(newItem);
        }

        UpdateCartUI();
    }

    void UpdateCartUI()
    {
        int totalMoney = 0;
        int totalTripDays = 0;
        List<ShopItem> visitedLocations = new List<ShopItem>();
        string summary = "Sepet: ";

        foreach (var cartItem in shoppingCart)
        {
            totalMoney += cartItem.shopItem.price * cartItem.quantity;

            if (!visitedLocations.Contains(cartItem.shopItem))
            {
                totalTripDays += cartItem.shopItem.travelDays;
                visitedLocations.Add(cartItem.shopItem);
            }

            summary += $"{cartItem.shopItem.itemData.itemName} (x{cartItem.quantity}) ";
        }

        if (totalCostText) totalCostText.text = $"Toplam: ${totalMoney}";
        if (totalDaysText) totalDaysText.text = $"Süre: {totalTripDays} Gün";
        if (cartSummaryText) cartSummaryText.text = summary;
    }

    void ClearCart()
    {
        shoppingCart.Clear();
        UpdateCartUI();
    }

    public void ConfirmTrip()
    {
        if (shoppingCart.Count == 0) return;

        int totalCost = 0;
        int totalDays = 0;
        List<ShopItem> visitedLocations = new List<ShopItem>();

        foreach (var cartItem in shoppingCart)
        {
            totalCost += cartItem.shopItem.price * cartItem.quantity;
            if (!visitedLocations.Contains(cartItem.shopItem))
            {
                totalDays += cartItem.shopItem.travelDays;
                visitedLocations.Add(cartItem.shopItem);
            }
        }

        if (MoneyManager.Instance.SpendMoney(totalCost))
        {
            foreach (var cartItem in shoppingCart)
            {
                InventoryManager.Instance.AddItem(cartItem.shopItem.itemData, cartItem.quantity);
            }


            ToggleOrderPanel(false);

            TimeManager.Instance.SkipDays(totalDays);

            Debug.Log($"Sefer başladı! {totalDays} gün sürecek.");
        }
        else
        {
            Debug.Log("Para Yetersiz!");
            if (cartSummaryText) cartSummaryText.text = "YETERSİZ BAKİYE!";
        }
    }
}