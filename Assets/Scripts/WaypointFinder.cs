using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointFinder : MonoBehaviour
{

    public List<Vector3> waypoints;
    public float Speed;
    private int blockPoint;


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
        blockPoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        MoveBlock();

    }

    void MoveBlock()
    {
        if (transform.position == waypoints[blockPoint])
        {
            blockPoint++;
            if (blockPoint >= waypoints.Count)
            {
                blockPoint = 0;
            }

        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[blockPoint], Speed * Time.deltaTime);
        }
    }

}
