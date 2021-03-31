using UnityEngine;
using System.Collections;

//Controlls the water

public class WaterController : MonoBehaviour 
{
    public static WaterController current;

    public bool isMoving;

    //Wave height and speed
    public float scale = 0.1f;
    public float speed = 1.0f;
    //The width between the waves
    public float waveDistance = 1f;
    //Noise parameters
    public float noiseStrength = 1f;
    public float noiseWalk = 1f;

    //Read water state from server - synchronize 
    public static float waterStateFromServer;
    public float handleTime;
    public float updateWaterTime;
    public bool reseted = true;

    //Interpolate water variables
    //public float InterpolationTickRate = 0.08f;
    //public static float fromPos = 0;
    //public static float toPos = 0;
    //public static float lastTimePos;

    void Start()
    {
        current = this;
    }



    //Get the y coordinate from whatever wavetype we are using
    public float GetWaveYPos(Vector3 position, float timeSinceStart)
    {
        if (isMoving)
        {
            return WaveTypes.SinXWave(position, speed, scale, waveDistance, noiseStrength, noiseWalk, timeSinceStart);
        }
        else
        {
            return 0f;
        }
    }



    //Find the distance from a vertice to water
    //Make sure the position is in global coordinates
    //Positive if above water
    //Negative if below water
    public float DistanceToWater(Vector3 position, float timeSinceStart)
    {
        float waterHeight = GetWaveYPos(position, timeSinceStart);

        float distanceToWater = position.y - waterHeight;

        return distanceToWater;
    }

    //Interpolate Water state
    //public static void SetWaterState(float _waterstate)
    //{
    //    fromPos = toPos;
    //    toPos = _waterstate;
    //    lastTimePos = Time.time;
    //}



    void Update()
    {
        Shader.SetGlobalFloat("_WaterScale", scale);
        Shader.SetGlobalFloat("_WaterSpeed", speed);
        Shader.SetGlobalFloat("_WaterDistance", waveDistance);
        //Synchronize water
        //if (waterStateFromServer != 0)
        //{
        //    if (reseted)
        //    {
        //        handleTime = Time.time;
        //        reseted = false;
        //    }
        //    updateWaterTime = Time.time - handleTime;
        //    Shader.SetGlobalFloat("_WaterTime", waterStateFromServer + updateWaterTime);
        //}


        //---interpolate water state
        //waterStateFromServer = Mathf.Lerp(fromPos, toPos, (Time.time - lastTimePos) / InterpolationTickRate);

        Shader.SetGlobalFloat("_WaterTime", waterStateFromServer);

        //Debug.Log($"Current water state: {waterStateFromServer}");
        Shader.SetGlobalFloat("_WaterNoiseStrength", noiseStrength);
        Shader.SetGlobalFloat("_WaterNoiseWalk", noiseWalk);
    }
}
