using UnityEngine;

public class OrderBook : Targetable 
{
    public OrderManager orderManager;
    public AudioSource audioSource; // YENİ: Ses kaynağı
    public AudioClip pageSound;     // YENİ: Sayfa sesi

    public void Interact()
    {
        if (TimeManager.Instance.isShopOpen)
        {
            if(TimeManager.Instance.statusText) 
                TimeManager.Instance.statusText.text = "Dükkan açıkken gidemezsin!";
            return;
        }

        if (orderManager != null)
        {
            // SESİ ÇAL
            if (audioSource && pageSound) audioSource.PlayOneShot(pageSound);

            orderManager.ToggleOrderPanel(true);
        }
    }
}