using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public GameObject WaterObject;
    public InputField usernameField;
    public Image aimingcursor;
    public Image wallpaper;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void ConnectToServer()
    {
        Client.instance.ConnectToServer();        
    }

    public void DisableUI()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;
        wallpaper.enabled = false;
        aimingcursor.enabled = false;
        WaterObject.SetActive(true);
    }
}