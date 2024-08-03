using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractNotif : MonoBehaviour
{
    [SerializeField] private GameObject collectNotif;
    [SerializeField] private Collider notifCollider;

    private void Awake()
    {
        collectNotif.SetActive(false);
        notifCollider = GetComponent<Collider>();
    }

    private void Update() {
        //SetCollectNotifVisible(false);
    }

    public void SetCollectNotifVisible(bool visible) {
        //Debug.Log(gameObject.name + " setting interact visible " +  visible);
        collectNotif.SetActive(visible);
    }

    public void checkNotification()
    {
        /*if(notifCollider != null)
        {
            Physics.Area
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collider Entered " + other.gameObject.name);
        if(other.transform.CompareTag("Player"))
        {
        SetCollectNotifVisible(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("Collider Exited " + other.gameObject.name);
        if(other.transform.CompareTag("Player"))
        {
            SetCollectNotifVisible(false);
        }
    }
}
