using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public Camera worldCam;
    public GameObject player;
    private float CameraHeight;
    private float CameraWidth;
    public string[] levels;
    public float[] levelPosition;
    public float[] levelChunk;
    public Text levelTag;
    private int levelIndex;
    public GameObject levelTagObject;

    // Start is called before the first frame update
    void Start()
    {

        float screenRatio = (float)Screen.width / (float)Screen.height;
        CameraHeight = worldCam.orthographicSize * 2;
        CameraWidth = CameraHeight * worldCam.aspect;
        float levelPos = worldCam.transform.position.y;
        
        if (levelPosition.Contains(levelPos))
        {
            levelTagObject.SetActive(true);
            levelIndex = System.Array.IndexOf(levelPosition, worldCam.transform.position.y);
            levelTag.text = levels[levelIndex];
            LeanTween.alphaText(levelTag.rectTransform, 1f, 2f).setEase(LeanTweenType.linear).setOnComplete(FadeLevelTagOut);
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y > (worldCam.transform.position.y + CameraHeight/2))
            {
            Debug.Log(player.transform.position.y);
            Debug.Log(worldCam.transform.position.y + CameraHeight / 2);
            worldCam.transform.position = new Vector3(worldCam.transform.position.x, worldCam.transform.position.y + CameraHeight, worldCam.transform.position.z);
            DisplayLevel();

        }

        if (player.transform.position.y < (worldCam.transform.position.y - CameraHeight/2))
        {
            worldCam.transform.position = new Vector3(worldCam.transform.position.x, worldCam.transform.position.y - CameraHeight, worldCam.transform.position.z);
            //LoadChunks();
            //worldCam.transform.position = new Vector3(worldCam.transform.position.x, worldCam.transform.position.y - CameraHeight, worldCam.transform.position.z);
        }
    }

    void FadeLevelTagOut()
    {
        LeanTween.alphaText(levelTag.rectTransform, 0f, 2f).setEase(LeanTweenType.linear).setOnComplete(LevelTagHide);
    }

    void LevelTagHide()
    {
        levelTagObject.SetActive(false);
    }

    void DisplayLevel()
    {
        if (levelPosition.Contains(worldCam.transform.position.y))
        {

            levelTagObject.SetActive(true);
            levelIndex = System.Array.IndexOf(levelPosition, worldCam.transform.position.y);
            levelTag.text = levels[levelIndex];
            LeanTween.alphaText(levelTag.rectTransform, 1f, 2f).setEase(LeanTweenType.linear).setOnComplete(FadeLevelTagOut);
        }
    }

    bool CameraCheck()
    {
        return true;
    }
}
