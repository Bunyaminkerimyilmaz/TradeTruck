using UnityEngine;


public class WalkieTalk : Targetable 
{
    public bool storeOpen = false; 

    public void StoreOpen()
    {
       
        if (TimeManager.Instance.isShopOpen || TimeManager.Instance.isDayEnded) return;

        storeOpen = true;
        
        
        TimeManager.Instance.OpenShop();
    }
}
