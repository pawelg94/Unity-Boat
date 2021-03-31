using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myid = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myid;
        ClientSend.WelcomeReceived();
        UIManager.instance.DisableUI();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    //Get water state from server
    public static void GetWaterState(Packet _packet)
    {
        float _waterState = _packet.ReadFloat();
        //int _myid = _packet.ReadInt();  <<< if water send to specific player || Now its SendUDPDataToAll

        WaterController.waterStateFromServer = _waterState;
        //Debug.Log("Time: " + System.DateTime.Now.Ticks);
        //WaterController.SetWaterState(_waterState);

    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            //_player.transform.position = _position;
            _player.SetPosition(_position);


        }
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        if (GameManager.players.TryGetValue(_id, out PlayerManager _player))
        {
            //_player.transform.rotation = _rotation;
            _player.SetRotation(_rotation);

            //if (_id != Client.instance.myId)
            //{
            //    _player.SetRotation(_rotation);
            //}
            //else
            //{
            //    _player.transform.rotation = _rotation;
            //}

        }
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }

    public static void PlayerHealth(Packet _packet)
    {
        int _id = _packet.ReadInt();
        float _health = _packet.ReadFloat();

        GameManager.players[_id].SetHealth(_health);
    }

    public static void PlayerRespawned(Packet _packet)
    {
        int _id = _packet.ReadInt();

        GameManager.players[_id].Respawn();
    }

    public static void SpawnBullet(Packet _packet)
    {
        int _bulletId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        int _shootFromPlayer = _packet.ReadInt();

        GameManager.instance.SpawnBullet(_bulletId, _position);
    }

    public static void BulletPosition(Packet _packet)
    {
        int _bulletId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        if (GameManager.bullets.TryGetValue(_bulletId, out BulletManager _bullet))
        {
            //_bullet.transform.position = _position;
            _bullet.SetPosition(_position);
        }
    }

    public static void BulletExplosion(Packet _packet)
    {
        int _bulletId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        if (GameManager.bullets.TryGetValue(_bulletId, out BulletManager _bullet))
        {
            _bullet.Explode(_position);
        }
    }

    public static void PlayerMessage(Packet _packet)
    {
        string _message = _packet.ReadString();
        int _fromPlayer = _packet.ReadInt();

        GameManager.instance.ShowMessage(_message, _fromPlayer);
        Debug.Log($"odebrana wiadomość: {_message}");
    }

    public static void PlayerJoinTeam(Packet _packet)
    {
        int _teamPositionIndex = _packet.ReadInt();
        int _fromPlayer = _packet.ReadInt();
        int _team = _packet.ReadInt();
        try
        {
            bool? _isReady = _packet.ReadBool();
            if (_isReady != null)
            {
                GameManager.instance.PlayerJoinTeam(_teamPositionIndex, _fromPlayer, _team, _isReady);
            }
        }
        catch (System.Exception)
        {
            GameManager.instance.PlayerJoinTeam(_teamPositionIndex, _fromPlayer, _team);
        }
               
    }


    public static void PlayerLeaveTeam(Packet _packet)
    {
        //int _teamPositionIndex = _packet.ReadInt();
        //int _fromPlayer = _packet.ReadInt();
        //int _team = _packet.ReadInt();

        GameManager.instance.PlayerLeaveTeam();
    }


    public static void StartBattle(Packet _packet)
    {
        GameManager.instance.StartBattle();
    }

    public static void EndBattle(Packet _packet)
    {               
        int _teamWon = _packet.ReadInt();
        Debug.Log($"endbattle received: team{_teamWon} wins");
        GameManager.instance.EndBattle(_teamWon);
        
    }

    public static void BattleBeginIn(Packet _packet)
    {
        int _time = _packet.ReadInt();
        GameManager.instance.BattleBeginIn(_time);

    }

    public static void BattleCanceled(Packet _packet)
    {
        //int _time = _packet.ReadInt();
        GameManager.instance.BattleCancelled();

    }

}
