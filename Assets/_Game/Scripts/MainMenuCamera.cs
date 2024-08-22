using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    [SerializeField] private float rotationAmount;
    [SerializeField] private float cameraTimer;
    [SerializeField] private Vector3[] positions;
    [SerializeField] private Vector3[] rotations;

    private float timer = 0;
    private int cameraIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        swapCamera(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= cameraTimer)
        {
            timer = 0;
            cameraIndex = (cameraIndex + 1) % positions.Length;
            swapCamera(cameraIndex);
        }
        Vector3 rotation = this.gameObject.transform.rotation.eulerAngles;
        rotation.y += rotationAmount * Time.deltaTime;
        this.gameObject.transform.rotation = Quaternion.Euler(rotation);

        timer += Time.deltaTime;
    }

    void swapCamera(int index)
    {
        this.gameObject.transform.position = positions[index];

        Vector3 rotation = rotations[index];
        rotation.y -= rotationAmount * (cameraTimer / 2) * rotationAmount;
        this.gameObject.transform.rotation = Quaternion.Euler(rotation);
    }
}
