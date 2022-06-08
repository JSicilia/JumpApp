using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int Completions;
    public bool SpriteFlip;
    public float[] SavedPosition;

    public PlayerData (Player player, Vector3 position)
    {
        Completions = player.Completions;
        SpriteFlip = player.GetComponent<SpriteRenderer>().flipX;
        SavedPosition = new float[3];
        SavedPosition[0] = position.x;
        SavedPosition[1] = position.y;
        SavedPosition[2] = position.z;
    }
}
