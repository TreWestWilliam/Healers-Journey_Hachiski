using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject characterPosition;
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -5);

    // Runs after character movement finishes
    void LateUpdate()
    {
        Vector3 position = characterPosition.transform.position + offset;
        transform.position = position;
    }
}
