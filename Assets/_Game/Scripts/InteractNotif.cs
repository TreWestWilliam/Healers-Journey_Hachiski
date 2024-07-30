using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractNotif : MonoBehaviour
{
    
    [SerializeField] private GameObject collectNotif;

    private void Update() {
        SetCollectNotifVisible(false);
    }

    public void SetCollectNotifVisible(bool visible) {
        collectNotif.SetActive(visible);
    }
}
