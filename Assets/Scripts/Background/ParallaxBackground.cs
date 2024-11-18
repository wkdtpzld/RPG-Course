using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public Camera cam;
    [SerializeField] private float parallaxEffect;
    private float xPosition;
    private float length;

    void Start()
    {
        length = GetComponent<SpriteRenderer>().bounds.size.x;
        xPosition = transform.position.x;
    }

    void Update()
    {
        float distanceToMoved = cam.transform.position.x * (1 - parallaxEffect);
        float distanceToMove = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(xPosition + distanceToMove, transform.position.y);

        if (distanceToMoved > xPosition + length)
        {
            xPosition = xPosition + length;
        }
        else if (distanceToMoved < xPosition - length)
        {
            xPosition = xPosition - length;
        }
    }
}
