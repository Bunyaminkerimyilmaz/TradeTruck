using UnityEngine;
using System.Collections.Generic;
using System.IO; // Dosya işlemleri için kütüphane

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [Header("Eşya Veritabanı (Çok Önemli)")]
    // Oyundaki VAR OLABİLECEK TÜM eşyaları (ScriptableObject) buraya sürüklemelisin.
    // Sistem yükleme yaparken eşyayı buradan bulacak.
    public List<ItemData> allGameItems; 

    // Kayıt edilecek verilerin şablonu
    [System.Serializable]
    public class GameData
    {
        public int savedDay;
        public int savedMoney;
        public List<InventorySaveData> inventory = new List<InventorySaveData>();
    }

    [System.Serializable]
    public struct InventorySaveData
    {
        public string itemName; // Eşyanın ID'si gibi çalışacak
        public int count;
    }

    private string filePath;

    private void Awake()
    {
        Instance = this;
        // Dosya yolu: C:/Users/Kullanici/AppData/LocalLow/Sirket/Oyun/savegame.json
        filePath = Application.persistentDataPath + "/savegame.json";
    }

    private void Start()
    {
        // Eğer Ana Menüden "Devam Et" denildiyse yükle
        if (PlayerPrefs.GetInt("LoadGameRequest", 0) == 1)
        {
            LoadGame();
            PlayerPrefs.SetInt("LoadGameRequest", 0); // İsteği sıfırla
        }
    }

    // --- KAYDETME FONKSİYONU ---
    public void SaveGame()
    {
        GameData data = new GameData();

        // 1. Temel Veriler
        data.savedDay = TimeManager.Instance.currentDay;
        data.savedMoney = MoneyManager.Instance.currentMoney;

        // 2. Envanter Verileri
        foreach (var slot in InventoryManager.Instance.myInventory)
        {
            InventorySaveData itemData = new InventorySaveData();
            itemData.itemName = slot.itemData.itemName; // Eşyanın ismini al
            itemData.count = slot.count;
            data.inventory.Add(itemData);
        }

        // 3. Dosyaya Yazma (JSON)
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);

        Debug.Log("Oyun Kaydedildi: " + filePath);
    }

    // --- YÜKLEME FONKSİYONU ---
    public void LoadGame()
    {
        if (!File.Exists(filePath))
        {
            Debug.Log("Kayıt dosyası bulunamadı!");
            return;
        }

        // 1. Dosyayı Oku
        string json = File.ReadAllText(filePath);
        GameData data = JsonUtility.FromJson<GameData>(json);

        // 2. Verileri Yerine Koy
        TimeManager.Instance.currentDay = data.savedDay;
        // UI güncellemesi için küçük bir hile (0 gün ekle)
        TimeManager.Instance.AdvanceDay(0); 
        
        MoneyManager.Instance.currentMoney = data.savedMoney;
        MoneyManager.Instance.AddMoney(0); // UI güncellensin diye

        // 3. Envanteri Kur
        InventoryManager.Instance.myInventory.Clear(); // Önce boşalt

        foreach (var savedItem in data.inventory)
        {
            // İsime göre veritabanından orjinal objeyi bul
            ItemData originalItem = allGameItems.Find(x => x.itemName == savedItem.itemName);

            if (originalItem != null)
            {
                InventoryManager.Instance.AddItem(originalItem, savedItem.count);
            }
            else
            {
                Debug.LogWarning("Bulunamayan Eşya: " + savedItem.itemName);
            }
        }

        Debug.Log("Oyun Başarıyla Yüklendi.");
    }
    
    // Yardımcı: Kayıt dosyası var mı kontrolü (Ana Menü için lazım olacak)
    public static bool HasSaveFile()
    {
        string path = Application.persistentDataPath + "/savegame.json";
        return File.Exists(path);
    }
}
