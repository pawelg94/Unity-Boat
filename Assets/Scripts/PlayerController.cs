using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public Transform camTransformRB;
    public Transform camTransformLB;
    public TMP_InputField chatInputMessage;
    public static GameObject GUI;
    public static bool chatFocusStatus;
    public int team1 = 1;
    public int team2 = 2;
    public float cooldown = 6f;
    public float nextRBFireTime;
    public float nextLBFireTime;

    private void Start()
    {
        HealthBar.cam = GameObject.FindGameObjectWithTag("hpBar").transform;
        HealthBar.leftbroadsideface = GameObject.FindGameObjectWithTag("lbCamLocal").transform;
        HealthBar.rightbroadsideface = GameObject.FindGameObjectWithTag("rbCamLocal").transform;
        GameManager.chatPanel = GameObject.FindGameObjectWithTag("ChatContent");
        EndlessWaterController.current.toFollowObj = GameObject.Find("LPlayer Test(Clone)");
        chatInputMessage.text = "";
        chatInputMessage.GetComponent<TMP_InputField>().DeactivateInputField();
        chatFocusStatus = false;
        GUI = GameObject.Find("LocalPlayerGUI");
        GUI.SetActive(false);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            chatFocusStatus = !chatFocusStatus;
            if (chatFocusStatus)
            {
                chatInputMessage.GetComponent<TMP_InputField>().ActivateInputField();
            }
            if (!chatFocusStatus)
            {
                if (chatInputMessage.text.Length + GameManager.players[Client.instance.myId].username.Length <= 46 && chatInputMessage.text.Length > 0)
                {
                    ClientSend.PlayerSendMessage(chatInputMessage.text);
                    chatInputMessage.GetComponent<TMP_InputField>().text = "";
                    chatInputMessage.GetComponent<TMP_InputField>().DeactivateInputField();
                    EventSystem.current.SetSelectedGameObject(null);
                }
                else
                {
                    chatInputMessage.GetComponent<TMP_InputField>().DeactivateInputField();
                    EventSystem.current.SetSelectedGameObject(null);
                }
                
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (CameraController.rightBroadSideStatus == true)
            {
                if (Time.time < nextRBFireTime)
                {
                    return;
                }
                nextRBFireTime = Time.time + cooldown;                
                ClientSend.PlayerShootRB(camTransformRB.forward);
                CooldownUIManager.instance.cooldownRBImage.fillAmount = 0f;
                CooldownUIManager.instance.textRBCD.text = "6";
                CooldownUIManager.instance.StartCoroutine(CooldownUIManager.instance.ActiveRBCooldown(5));
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (CameraController.leftBroadSideStatus == true)
            {
                if (Time.time < nextLBFireTime)
                {
                    return;
                }
                nextLBFireTime = Time.time + cooldown;                
                ClientSend.PlayerShootLB(camTransformLB.forward);
                CooldownUIManager.instance.cooldownLBImage.fillAmount = 0f;
                CooldownUIManager.instance.textLBCD.text = "6";
                CooldownUIManager.instance.StartCoroutine(CooldownUIManager.instance.ActiveLBCooldown(5));
            }
        }

    }
    private void FixedUpdate()
    {
        SendInputToServer();
    }


    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D)

        };

        ClientSend.PlayerMovement(_inputs);

    }

    public void JoinTeam1()
    {
        ClientSend.PlayerJoinTeam(team1);
    }

    public void JoinTeam2()
    {
        ClientSend.PlayerJoinTeam(team2);
    }

    public void SetReady()
    {
        ClientSend.PlayerSetReady();
    }

    public static void ActiveGUI(bool _condition)
    {
        GUI.SetActive(_condition);
    }
}
