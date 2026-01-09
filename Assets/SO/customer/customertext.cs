using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Trade Data", menuName = "Trade Truck/Customer Trade Data")]
public class customertext : ScriptableObject
{
   
   [System.Serializable]
   public class TradeMission
   {
      [TextArea] public string dialogue; 
      public ItemData requiredItem;      
      public int requiredAmount;         
      public int cashReward;             
   }

   public List<TradeMission> missions; 

   
   public TradeMission GetRandomMission()
   {
      if (missions.Count == 0) return null;
      int index = Random.Range(0, missions.Count);
      return missions[index];
   }
}
