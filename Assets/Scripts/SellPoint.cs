using System;
using UnityEngine;

public class SellPoint : MonoBehaviour
{
    public bool sellPointEmpty = true;
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Customer")
        {
            sellPointEmpty = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Customer")
        {
            sellPointEmpty = false;
        }
    }
}
