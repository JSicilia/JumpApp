using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TorchScript : MonoBehaviour
{

    Transform mainLight;
    Transform flickerLight;
    UnityEngine.Rendering.Universal.Light2D mainLightComponent;
    UnityEngine.Rendering.Universal.Light2D flickerLightComponent;


    // Start is called before the first frame update
    void Start()
    {
        mainLight = this.transform.GetChild(0);
        flickerLight = this.transform.GetChild(1);
        mainLightComponent = mainLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        flickerLightComponent = flickerLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        StartCoroutine(Timer());
    }

    IEnumerator Timer()
    {
        for (; ; ) //this is while(true)
        {
            float randomIntensity = Random.Range(0.5f, 2f);
            flickerLightComponent.intensity = randomIntensity;


            float randomTime = Random.Range(0f, 0.3f);
            yield return new WaitForSeconds(randomTime);
        }
    }
}