using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject characterObject;
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -5);

    // Runs after character movement finishes
    void LateUpdate()
    {
        Vector3 position = characterObject.transform.position + offset;
        transform.position = position;
        transform.LookAt(characterObject.transform.position);
    }
}
