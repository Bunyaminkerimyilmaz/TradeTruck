using UnityEngine;

public class RaycastAction : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private Camera cam;

    private Targetable currentTargetable;


    private CustomerContact currentCustomerContact;
    private WalkieTalk _walkieTalk;
    private Bed _bed;
    private OrderBook _orderBook;

    [Header("Referanslar")] public MouseLook mouseLook;
    public SellPanel sellPanel;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.collider.TryGetComponent(out Targetable targetable))
            {
                if (currentTargetable != targetable)
                {
                    if (currentTargetable != null) currentTargetable.ToggleHighlight(false);
                    currentTargetable = targetable;
                    currentTargetable.ToggleHighlight(true);
                }


                if (currentTargetable.TryGetComponent(out CustomerContact customerContact))
                {
                    currentCustomerContact = customerContact;
                    _walkieTalk = null;
                    _bed = null;
                    _orderBook = null;
                }

                else if (currentTargetable.TryGetComponent(out WalkieTalk walkieTalk))
                {
                    _walkieTalk = walkieTalk;
                    currentCustomerContact = null;
                    _bed = null;
                    _orderBook = null;
                }

                else if (currentTargetable.TryGetComponent(out Bed bed))
                {
                    _bed = bed;
                    currentCustomerContact = null;
                    _walkieTalk = null;
                    _orderBook = null;
                }

                else if (currentTargetable.TryGetComponent(out OrderBook orderBook))
                {
                    _orderBook = orderBook;
                    currentCustomerContact = null;
                    _walkieTalk = null;
                    _bed = null;
                }
            }
            else
            {
                ClearTargets();
            }
        }
        else
        {
            ClearTargets();
        }


        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentCustomerContact)
            {
                currentCustomerContact.Talk();

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                mouseLook.canLook = false;

                sellPanel.panelClosed = false;
                sellPanel.ShowRandomText();

                currentCustomerContact = null;
            }

            else if (_orderBook)
            {
                if (!TimeManager.Instance.isShopOpen)
                {
                    _orderBook.Interact();


                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    mouseLook.canLook = false;
                }
                else
                {
                    _orderBook.Interact();
                }
            }

            else if (_walkieTalk)
            {
                _walkieTalk.StoreOpen();
            }

            else if (_bed)
            {
                _bed.Sleep();
            }
        }
    }

    void ClearTargets()
    {
        if (currentTargetable != null)
        {
            currentTargetable.ToggleHighlight(false);
            currentTargetable = null;
        }

        currentCustomerContact = null;
        _walkieTalk = null;
        _bed = null;
        _orderBook = null;
    }
}