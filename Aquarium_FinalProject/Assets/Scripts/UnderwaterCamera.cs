using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderwaterCamera : MonoBehaviour
{
    float underwaterDens = 0.15f;
    Color underwaterColor = new Vector4(0.1f, 0.3f, 0.4f, 1.0f);

    private bool oldFog;
    private float oldDens;
    private Color oldColor;
    private FogMode oldMode;
    private bool underwater = false;

    // sounds
    AudioSource underwaterNoise;
    AudioSource birdNoise;
    AudioSource sheepNoise;
    AudioSource waterNoise;

    // Use this for initialization
    void Start()
    {
        // get references to sounds
        underwaterNoise = GameObject.Find("underwater").GetComponent<AudioSource>();
        birdNoise = GameObject.Find("birds").GetComponent<AudioSource>();
        sheepNoise = GameObject.Find("sheep").GetComponent<AudioSource>();
        waterNoise = GameObject.Find("water").GetComponent<AudioSource>();
    }

    void Update()
    {
        // if it's underwater...
        if (gameObject.transform.position.y < 9.5f)
        {
            if (!underwater)
            { // turn on underwater effect only once
                oldFog = RenderSettings.fog;
                oldMode = RenderSettings.fogMode;
                oldDens = RenderSettings.fogDensity;
                oldColor = RenderSettings.fogColor;
                RenderSettings.fog = true;
                RenderSettings.fogMode = FogMode.Exponential;
                RenderSettings.fogDensity = underwaterDens;
                RenderSettings.fogColor = underwaterColor;
                underwater = true;
                underwaterNoise.enabled = true;
                birdNoise.enabled = false;
                sheepNoise.enabled = false;
                waterNoise.enabled = false;
            }
        }
        else // but if it's not underwater...
        {
            if (underwater)
            { // turn off underwater effect, if any
                RenderSettings.fog = oldFog;
                RenderSettings.fogMode = oldMode;
                RenderSettings.fogDensity = oldDens;
                RenderSettings.fogColor = oldColor;
                underwater = false;
                underwaterNoise.enabled = false;
                birdNoise.enabled = true;
                sheepNoise.enabled = true;
                waterNoise.enabled = true;
            }
        }
    }
}
