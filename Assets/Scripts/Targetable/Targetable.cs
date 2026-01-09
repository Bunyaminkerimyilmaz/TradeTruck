using UnityEngine;

public abstract class Targetable : MonoBehaviour
{
   public GameObject infoObject;

   public void ToggleHighlight(bool status)
   {
      infoObject.SetActive(status);
   }
}
