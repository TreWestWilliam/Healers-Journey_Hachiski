using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject characterPosition;
    void LateUpdate()
    {
        Vector3 position = characterPosition.transform.position;
        position.y += 5;
        position.z -= 5;
        transform.position = position;
    }
}
