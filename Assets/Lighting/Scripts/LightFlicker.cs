using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    // Start is called before the first frame update
    List<Light2D> lights = new List<Light2D>();
    private int nextUpdate = 1;
    private int currentFrameCount = 0;
    public float flickerLowerFrequency;
    public float flickerUpperFrequency;

    private bool innerLightOn = true;
    private bool outerLightOn = true;
    private bool isInnersTurn = false;
    private bool isOutersTurn = true;

    void Start()
    {
        GameObject[] flickers = GameObject.FindGameObjectsWithTag("Flicker");
        foreach(GameObject gameObject in flickers)
        {
            lights.Add(GameObjectToLight2D(gameObject));
        }
   }

    // Update is called once per frame
    void Update()
    {
        currentFrameCount++;
        if (currentFrameCount >= nextUpdate)
        {
           
            nextUpdate =((int)Random.Range(flickerLowerFrequency, flickerUpperFrequency));
            // Call your fonction
            UpdateEverySecond();
            currentFrameCount = 0;
        }
    }
    void UpdateEverySecond()
    {
        for (int i = 0; i < lights.Count; i++)
        {
            Light2D light = lights[i];

            //Turn inside light off
            if (innerLightOn && !outerLightOn && isInnersTurn && light.name == "Candle Light 2D Flicker Inner")
            {
                light.enabled = !light.enabled;
                innerLightOn = false;
                isOutersTurn = false;
                isInnersTurn = true;
                break;
            }

            //Turn inside light on 
            if (!innerLightOn && !outerLightOn && light.name == "Candle Light 2D Flicker Inner")
            {
                light.enabled = !light.enabled;
                innerLightOn = true;
                isInnersTurn = false;
                isOutersTurn = true;
                break;
            }

            //Turn outside light off
            if (outerLightOn && innerLightOn && light.name == "Candle Light 2D Flicker Outer")
            {
                light.enabled = !light.enabled;
                outerLightOn = false;
                isInnersTurn = true;
                isOutersTurn = false;
                break;
            }

            //Turn outside light on 
            if (innerLightOn && !outerLightOn && isOutersTurn && light.name == "Candle Light 2D Flicker Outer")
            {
                light.enabled = !light.enabled;
                outerLightOn = true;
                isOutersTurn = true;
                isInnersTurn = false;
                break;
            }
        }
    }

    public static Light2D GameObjectToLight2D(GameObject gameObject)
    {
        return gameObject.GetComponent<Light2D>();
    }
}
