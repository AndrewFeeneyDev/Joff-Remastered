using Unity.VisualScripting;
using UnityEngine;

public class ParallaxCamera : MonoBehaviour
{
    [SerializeField] private Transform mainCamera;
    [SerializeField] private float parallaxSpeedX;
    [SerializeField] private float parallaxSpeedY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = FindAnyObjectByType<Camera>().transform;

        if (mainCamera == null)
        {
            Debug.Log($"{gameObject.name} missing Main Camera reference");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        transform.position = new Vector2(mainCamera.position.x * parallaxSpeedX, mainCamera.position.y * parallaxSpeedY);
    }
}
