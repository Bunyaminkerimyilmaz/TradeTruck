using UnityEngine;
using TMPro; 
using UnityEngine.UI;
using System.Collections; // Coroutine için gerekli

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    [Header("Saat Ayarları")]
    public float openingHour = 8f;   
    public float closingHour = 20f;  
    public float dayDuration = 120f; 
    
    [Header("Gün Sistemi")]
    public int currentDay = 1; 
    public int maxDays = 30;   
    
    [Header("Sesler (YENİ)")]
    public AudioSource mainAudioSource; // Genel ses kaynağı
    public AudioClip shutterSound;      // Kepenk açılma
    public AudioClip yawnSound;         // Esneme
    public AudioClip truckSound;        // Kamyon sesi

    [Header("Referanslar")]
    public Slider timeSlider;       
    public Light sunLight;          
    public TextMeshProUGUI statusText; 
    public TextMeshProUGUI dayText; 

    [Header("Durum")]
    public float currentTime;
    public bool isShopOpen = false; 
    public bool isDayEnded = false; 

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateDayUI(); 
        ResetDay(); 
    }

    void Update()
    {
        if (isShopOpen && !isDayEnded)
        {
            currentTime += Time.deltaTime / dayDuration * (closingHour - openingHour);

            if (timeSlider)
            {
                float progress = (currentTime - openingHour) / (closingHour - openingHour);
                timeSlider.value = progress;
            }

            RotateSun();

            if (currentTime >= closingHour)
            {
                CloseShopAutomatic();
            }
        }
    }

    // --- DÜKKAN AÇMA ---
    public void OpenShop()
    {
        if (isDayEnded) return; 

        isShopOpen = true;
        
        // SES: Kepenk sesi çal
        if(mainAudioSource && shutterSound) mainAudioSource.PlayOneShot(shutterSound);

        if(statusText) statusText.text = "Dükkan Açık - İyi Satışlar";
    }

    void CloseShopAutomatic()
    {
        isShopOpen = false;
        isDayEnded = true;
        Debug.Log("Mesai Bitti.");
        if(statusText) statusText.text = "Mesai Bitti! Yatağa git.";
    }

    // --- UYUMA SİSTEMİ (Sinematik) ---
    public void Sleep()
    {
        if (!isDayEnded)
        {
            if(statusText) statusText.text = "Daha akşam olmadı!";
            return;
        }

        StartCoroutine(SleepRoutine());
    }

    IEnumerator SleepRoutine()
    {
        // ... (Ekran kararma kodları vs.) ...
        yield return ScreenFader.Instance.FadeOut();
        // ... (Ses çalma kodları vs.) ...

        // Günü İlerlet
        AdvanceDay(1);

        // --- BURAYA EKLE: KAYDETME ---
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.SaveGame();
            Debug.Log("Uyurken oyun kaydedildi.");
        }
        // -----------------------------

        yield return ScreenFader.Instance.FadeIn();
    }

    // --- SEFERE ÇIKMA SİSTEMİ (Sinematik) ---
    public void SkipDays(int daysToSkip)
    {
        StartCoroutine(ExpeditionRoutine(daysToSkip));
    }

    IEnumerator ExpeditionRoutine(int days)
    {
        // 1. Ekranı Karart
        yield return ScreenFader.Instance.FadeOut();

        // 2. Ses Çal (Kamyon Sesi)
        if(mainAudioSource && truckSound) mainAudioSource.PlayOneShot(truckSound);

        // 3. Ekrana Bilgi Yaz (Karanlıkta)
        if(statusText) statusText.text = days + " Günlük yolculuk yapılıyor...";

        // 4. Kamyon sesi kadar veya sabit bir süre bekle
        yield return new WaitForSeconds(3f); // 3 Saniye yolculuk hissi

        // 5. Günleri Hesapla
        currentDay += days;
        if (currentDay > maxDays)
        {
            // Oyun Bitti Mantığı buraya
        }
        else
        {
            UpdateDayUI(); 
            ResetDay();    
            Debug.Log("Seferden dönüldü.");
        }

        // 6. Ekranı Aç
        yield return ScreenFader.Instance.FadeIn();
        
        if(statusText) statusText.text = "Mallarla geri döndün!";
    }

    // --- YARDIMCILAR ---
    public void AdvanceDay(int amount) // Manuel sadece gün atlamak için
    {
        currentDay += amount;
        UpdateDayUI(); 
        ResetDay();    
    }

    void ResetDay()
    {
        currentTime = openingHour;
        isShopOpen = false;
        isDayEnded = false;
        if (timeSlider) timeSlider.value = 0;
        if (statusText) statusText.text = "Telsize basıp dükkanı aç.";
        RotateSun();
    }
    
    void UpdateDayUI()
    {
        if (dayText != null) dayText.text = "GÜN: " + currentDay + " / " + maxDays;
    }

    void RotateSun()
    {
        if (sunLight == null) return;
        float progress = (currentTime - openingHour) / (closingHour - openingHour);
        float sunAngle = Mathf.Lerp(0f, 180f, progress); 
        sunLight.transform.rotation = Quaternion.Euler(sunAngle, -30f, 0f);
    }
}