using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SellPanel : MonoBehaviour
{
    [Header("Butonlar")]
    public Button sellButton;
    public Button rejectButton;
    public Button increasePriceButton; 
    public Button decreasePriceButton;

    [Header("UI Elemanları")]
    public TMP_InputField priceInputField; 
    public TextMeshProUGUI dialogueTextField;
    public TextMeshProUGUI feedbackText;

    [Header("Veri")]
    public customertext CustomertextSO;
    
    public MouseLook mouseLook;
    
    [System.Serializable]
    public struct ItemUIBinding
    {
        public string label;
        public ItemData itemType;
        public TextMeshProUGUI uiText;
    }
    public List<ItemUIBinding> inventorySlots;

    
    [Header("Durum Kontrolü")]
    public bool panelClosed = true; 
    public bool goHome = false;     
    

    private customertext.TradeMission currentMission;
    private int currentOfferPrice; 

    void Start()
    {
        sellButton.onClick.AddListener(OnSellClicked);
        rejectButton.onClick.AddListener(OnRejectClicked);

        if(increasePriceButton) increasePriceButton.onClick.AddListener(() => ChangePrice(10));
        if(decreasePriceButton) decreasePriceButton.onClick.AddListener(() => ChangePrice(-10));
        
        if(priceInputField) priceInputField.onValueChanged.AddListener(delegate { OnInputValueChanged(); });
    }

    public void ShowRandomText()
    {
        gameObject.SetActive(true);
        panelClosed = false; 
        
        if(feedbackText) feedbackText.text = ""; 

        currentMission = CustomertextSO.GetRandomMission();

        if (currentMission != null)
        {
            dialogueTextField.text = currentMission.dialogue;
            currentOfferPrice = currentMission.cashReward;
            if(priceInputField) priceInputField.text = currentOfferPrice.ToString();
        }

        UpdateInventoryUI();
    }

    public void OnInputValueChanged()
    {
        if (priceInputField != null && int.TryParse(priceInputField.text, out int result))
        {
            currentOfferPrice = result;
        }
    }

    public void ChangePrice(int amount)
    {
        currentOfferPrice += amount;
        if (currentOfferPrice < 0) currentOfferPrice = 0; 
        if(priceInputField) priceInputField.text = currentOfferPrice.ToString();
    }

    public void OnSellClicked()
    {
        if (currentMission == null || InventoryManager.Instance == null) return;

        if (!InventoryManager.Instance.HasEnoughItem(currentMission.requiredItem, currentMission.requiredAmount))
        {
            if(feedbackText) feedbackText.text = "Insufficient Material!";
            return;
        }

        bool tradeAccepted = CalculateTradeChance();

        if (tradeAccepted)
        {
            InventoryManager.Instance.RemoveItem(currentMission.requiredItem, currentMission.requiredAmount);
            if (MoneyManager.Instance != null)
            {
                MoneyManager.Instance.AddMoney(currentOfferPrice);
            }
            Debug.Log($"Satış Tamam! {currentOfferPrice}$ kazanıldı.");
            ClosePanel();
        }
        else
        {
            if(feedbackText) feedbackText.text = "Customer: 'This price is too high! 'I'm not buying it!'";
            Invoke("ClosePanel", 0.7f); 
        }
    }

    bool CalculateTradeChance()
    {
        int basePrice = currentMission.cashReward;
        if (currentOfferPrice <= basePrice) return true;
        if (currentOfferPrice > basePrice * 2.0f) return false;

        float increaseRatio = (float)(currentOfferPrice - basePrice) / basePrice; 
        float successChance = 1.0f - increaseRatio; 
        return Random.value <= successChance;
    }

    void UpdateInventoryUI()
    {
        if (InventoryManager.Instance == null) return;
        foreach (var slot in inventorySlots)
        {
            if (slot.uiText != null && slot.itemType != null)
            {
                int count = InventoryManager.Instance.GetItemCount(slot.itemType);
                slot.uiText.text = slot.itemType.itemName + ": " + count;
            }
        }
    }

    public void OnRejectClicked()
    {
        ClosePanel();
    }

    void ClosePanel()
    {
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mouseLook.canLook = true;
        
        panelClosed = true; 
        goHome = true;      

        if(feedbackText) feedbackText.text = "";
    }
}
