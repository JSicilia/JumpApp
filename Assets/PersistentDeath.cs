using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentDeath : MonoBehaviour
{
    ParticleSystem particle;
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();
    public GameObject BloodSplat;
    public Transform splatHolder;
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(particle, other, collisionEvents);
        //Debug.Log("blood collided");
        int count = collisionEvents.Count;

        for (int i = 0; i < count; i++)
        {
            Instantiate(BloodSplat, collisionEvents[i].intersection, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)), splatHolder);
        }
    }
}
