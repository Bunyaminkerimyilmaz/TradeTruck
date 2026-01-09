using UnityEngine;

public class CustomerContact : Targetable
{
  public GameObject sellPanel;
  public virtual void Talk()
  {
    sellPanel.SetActive(true);
  }
}
