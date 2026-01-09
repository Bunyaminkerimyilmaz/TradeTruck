using UnityEngine;
using TMPro; 

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance; 

    [Header("Ayarlar")]
    public int startMoney = 500; 
    public int currentMoney;

    [Header("UI Bağlantısı")]
    public TextMeshProUGUI moneyText; 

    void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
        currentMoney = startMoney;
        UpdateUI();
    }

    
    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateUI();
    }

   
    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            UpdateUI();
            return true; 
        }
        else
        {
            Debug.Log("Yetersiz Bakiye!");
            return false; 
        }
    }

    
    void UpdateUI()
    {
        if (moneyText != null)
        {
            moneyText.text = "$ " + currentMoney.ToString();
        }
    }
}
