using UnityEngine;


public class Bed : Targetable 
{
    public void Sleep()
    {
        
        TimeManager.Instance.Sleep();
    }
}
