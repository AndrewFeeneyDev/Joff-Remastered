using UnityEngine;

public class ParallaxCursor : MonoBehaviour
{
    [SerializeField] private float parallaxSpeedX;
    [SerializeField] private float parallaxSpeedY;

    private Vector3 startPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector3 mouse = Input.mousePosition;

        float x = (mouse.x / Screen.width - 0.5f) * parallaxSpeedX;
        float y = (mouse.y / Screen.height - 0.5f) * parallaxSpeedY;

        transform.position = startPosition + new Vector3(x, y, 0f);
    }
}
