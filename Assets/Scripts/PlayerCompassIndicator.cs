using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCompassIndicator : MonoBehaviour
{
    public static PlayerCompassIndicator instance;
    public float scale;
    public Color colorRed = new Color(1f, 0f, 0f, 1f);
    public Color colorGreen = new Color(1f, 0f, 0f, 1f);
    public int myTeam;

    // Start is called before the first frame update
    //public void Initialize()
    //{
    //    CompassIndicator.instance.RegisterPlayer(this);
    //}

}
