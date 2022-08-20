using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    private float length, startPos;

    public GameObject cam;
    public float parallaxEffect;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        if (GetComponent<SpriteRenderer>() != null)
        {
            length = GetComponent<SpriteRenderer>().bounds.size.x;
        }
        else if (GetComponent<BoxCollider2D>() != null)
        {
            length = GetComponent<BoxCollider2D>().bounds.size.x;
        }
        else
        {
            length = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        float tempDistance = cam.transform.position.x * (1 - parallaxEffect);
        float distance = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (tempDistance > startPos + length - 5)
        {
            startPos += length;
        }
        else if (tempDistance < startPos - length + 5)
        {
            startPos -= length;
        }
    }
}
