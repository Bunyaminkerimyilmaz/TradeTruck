using UnityEngine;
using UnityEngine.AI;

public class CustomerMove : MonoBehaviour
{
    public Transform sellPoint;
    public Transform homePoint;
    public NavMeshAgent agent;
    public WalkieTalk walkieTalk;
    public SellPanel sellPanel;
    public Transform salesPersonPosition;
    public bool wentToHomePoint=false;

    void Start()
    {
        agent.updateRotation = false;
    }
    void Update()
    {  
       
        if (walkieTalk.storeOpen)
        {
            agent.SetDestination(sellPoint.position);
        }

        if (sellPanel.goHome)
        {
            agent.updateRotation = true;
            agent.SetDestination((homePoint.position));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "homepoint")
        {
            Debug.Log("evdeyim");
            Destroy(this.gameObject);
            sellPanel.goHome = false;
        }

        if (other.tag == "sellpoint")
        {
           
            Debug.Log("satıştayım");
            transform.LookAt(salesPersonPosition);
        }
    }

   
}
