using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, BulletManager> bullets = new Dictionary<int, BulletManager>();
    public static List<TextMeshProUGUI> messages = new List<TextMeshProUGUI>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public GameObject bulletPrefab;
    public static GameObject chatPanel = null;
    public TextMeshProUGUI messageObject;
    private TextMeshProUGUI Player1Team1;
    private TextMeshProUGUI PlayerReadyStatus;
    private GameObject networkLobby;
    private TextMeshProUGUI battleBeginsIn;
    private TextMeshProUGUI teamWinsText;

    public int teamMateMaterial = 1;
    public int enemyMaterial = 2;



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

    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
        }

        _player.GetComponent<PlayerManager>().Initialize(_id, _username);
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }

    public void SpawnBullet(int _id, Vector3 _position)
    {
        GameObject _bullet = Instantiate(bulletPrefab, _position, Quaternion.identity);
        _bullet.GetComponent<BulletManager>().Initialize(_id);
        bullets.Add(_id, _bullet.GetComponent<BulletManager>());
    }

    public void ShowMessage(string _message, int _fromPlayer)
    {
        if (messages.Count >= 15)
        {
            messages.Remove(messages[0]);
            Destroy(messages[0].GetComponent<TextMeshProUGUI>().gameObject);
        }
        TextMeshProUGUI _displayMessage = Instantiate(messageObject, chatPanel.transform);
        _displayMessage.GetComponent<TextMeshProUGUI>().text = players[_fromPlayer].username + ": " + _message;
        //Ustawienie 2 linii przy tekscie dłuższym niż 27 znakow
        if (_displayMessage.GetComponent<TextMeshProUGUI>().text.Length > 30)
        {
            _displayMessage.GetComponent<TextMeshProUGUI>().rectTransform.sizeDelta = new Vector2(120.5f, 24f);
        }
        messages.Add(_displayMessage);
    }

    public void PlayerJoinTeam(int _positionIndex, int _fromPlayer, int _team)
    {
        Player1Team1 = GameObject.Find($"Text_Player{_positionIndex + 1}Team{_team}_Username").GetComponent<TextMeshProUGUI>();
        players[_fromPlayer].team = _team;
        Player1Team1.text = players[_fromPlayer].username;

    }

    public void PlayerJoinTeam(int _positionIndex, int _fromPlayer, int _team, bool? _isReady)
    {
        Player1Team1 = GameObject.Find($"Text_Player{_positionIndex + 1}Team{_team}_Username").GetComponent<TextMeshProUGUI>();
        PlayerReadyStatus = GameObject.Find($"Text_Player{_positionIndex + 1}Team{_team}_Ready").GetComponent<TextMeshProUGUI>();
        Player1Team1.text = players[_fromPlayer].username;
        players[_fromPlayer].team = _team;
        if (_isReady == true)
        {
            PlayerReadyStatus.text = "Ready";
        }

    }

    public void PlayerLeaveTeam()
    {
        //Player1Team1 = GameObject.Find($"Text_Player{_positionIndex + 1}Team{_team}_Username").GetComponent<TextMeshProUGUI>();
        //Player1Team1.text = "Waiting for player ...";
        RefreshTeams();
        //BattleCancelled();

    }

    public void RefreshTeams()
    {

        for (int i = 1; i < 6; i++)
        {
            Player1Team1 = GameObject.Find($"Text_Player{i}Team1_Username").GetComponent<TextMeshProUGUI>();
            PlayerReadyStatus = GameObject.Find($"Text_Player{i}Team1_Ready").GetComponent<TextMeshProUGUI>();
            Player1Team1.text = "Waiting for player ...";
            PlayerReadyStatus.text = "";

        }

        for (int i = 1; i < 6; i++)
        {
            Player1Team1 = GameObject.Find($"Text_Player{i}Team2_Username").GetComponent<TextMeshProUGUI>();
            PlayerReadyStatus = GameObject.Find($"Text_Player{i}Team2_Ready").GetComponent<TextMeshProUGUI>();
            Player1Team1.text = "Waiting for player ...";
            PlayerReadyStatus.text = "";
        }


    }

    public void StartBattle()
    {
        
        networkLobby.SetActive(false);
        PlayerController.ActiveGUI(true);
        int myteam = players[Client.instance.myId].team;
        //myteam = players[Client.instance.myId].team;
        for (int i = 1; i <= players.Count; i++)
        {
            //int _id = i;
            //players.TryGetValue(_id, out PlayerManager _player);

            if (players[i] != players[Client.instance.myId])
            {
                if (players[i].team == myteam)
                {
                    players[i].SetMaterial(teamMateMaterial);
                }
                else if (players[i].team != myteam)
                {
                    players[i].SetMaterial(enemyMaterial);
                }                
                players[i].RegisterToCompass();
            }
            players[i].SetHealth(100f);
        }

    }

    public void EndBattle(int _teamWon)
    {
        networkLobby.SetActive(true);
        PlayerController.ActiveGUI(false);        
        RefreshTeams();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        battleBeginsIn.text = "";
        teamWinsText.text = $"Team{_teamWon} Wins";
        CompassIndicator.instance.players.Clear();
        CompassIndicator.instance.playersOnMap.Clear();
        CompassIndicator.instance.ClearCompass();
    }


    public void BattleBeginIn(int _time)
    {
        
        battleBeginsIn.text = $"Starting in: {_time}";
    }

    public void BattleCancelled()
    {
        
        battleBeginsIn.text = "";
    }


    public void Start()
    {
        battleBeginsIn = GameObject.Find("Text_BattleBeginIn").GetComponent<TextMeshProUGUI>();
        networkLobby = GameObject.Find("NetworkLobby");
        teamWinsText = GameObject.Find("Text_WinnerTeam").GetComponent<TextMeshProUGUI>();

    }
}