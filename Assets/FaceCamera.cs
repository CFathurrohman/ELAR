using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        // Buat button selalu menghadap ke kamera
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
    }
}
