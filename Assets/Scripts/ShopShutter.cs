using UnityEngine;

public class ShopShutter : MonoBehaviour
{
    [Header("Ayarlar")]
    public float moveSpeed = 2f;   
    public Vector3 openOffset = new Vector3(0, 3f, 0);

    private Vector3 closedPosition; 
    private Vector3 openPosition;   

    void Start()
    {
        
        closedPosition = transform.position;
        
        
        openPosition = closedPosition + openOffset;
    }

    void Update()
    {
        
        if (TimeManager.Instance == null) return;

        
        Vector3 targetPosition = TimeManager.Instance.isShopOpen ? openPosition : closedPosition;

        
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
