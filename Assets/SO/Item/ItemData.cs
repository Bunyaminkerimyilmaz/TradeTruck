using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "Trade Truck/Item")]
public class ItemData : ScriptableObject
{
    [Header("Eşya Bilgileri")]
    public string itemName;        
    public Sprite icon;            
    public float basePrice;        
    public bool isStrategic;       

    [TextArea]
    public string description;     
}
