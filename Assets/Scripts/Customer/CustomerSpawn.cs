using System;
using UnityEngine;
using UnityEngine.AI;

using UnityEngine;

public class CustomerSpawn : MonoBehaviour
{
    [Header("Spawn Ayarları")]
    public GameObject objectToSpawn;
    public Transform spawnPoint;
    public SellPoint sellPointsc;
    public float timer = 0f; 
    public float spawnDelay = 10f;

    [Header("Referanslar (Müşteriye Aktarılacaklar)")]
    public SellPanel sellPanels;
    public GameObject sellPanel;
    public Transform sellPoint;
    public Transform homePoint;
    public WalkieTalk walkieTalk;
    public Transform salesPersonPosition;

    void Update()
    {
        if (TimeManager.Instance == null || !TimeManager.Instance.isShopOpen)
        {
            return; 
        }

        timer += Time.deltaTime;

        if (sellPointsc.sellPointEmpty && timer >= spawnDelay)
        {
            Debug.Log("Müşteri doğdu");
            Spawn();
            timer = 0f;
        }
    }

    public void Spawn()
    {
        GameObject go = Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);

        CustomerMove cm = go.GetComponent<CustomerMove>();
        CustomerContact cc = go.GetComponent<CustomerContact>();

        // Mevcut atamaların aynen kalıyor
        if (cm != null)
        {
            cm.sellPoint = sellPoint;
            cm.homePoint = homePoint;
            cm.agent = go.GetComponent<UnityEngine.AI.NavMeshAgent>();
            cm.walkieTalk = walkieTalk;
            cm.sellPanel = sellPanels;
            cm.salesPersonPosition = salesPersonPosition;
        }

        if (cc != null)
        {
            cc.sellPanel = sellPanel;
        }
    }
}