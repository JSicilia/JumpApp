using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlocks : MonoBehaviour
{
    private Vector3 startPosition;
    public float frequency;
    public float magnitude;
    public float offset;
    public bool vertical;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.collider.transform.SetParent(null);
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (vertical)
        {
            transform.position = startPosition + transform.up * Mathf.Sin(Time.time * frequency + offset) * magnitude;
        } else
        {
            transform.position = startPosition + transform.right * Mathf.Sin(Time.time * frequency + offset) * magnitude;
        }
        
    }
}
