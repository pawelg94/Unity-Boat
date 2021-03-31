using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.usernameField.text);

            SendTCPData(_packet);
        }
    }

    public static void PlayerMovement(bool[] _inputs)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs.Length);
            foreach (bool _input in _inputs)
            {
                _packet.Write(_input);
            }

            //_packet.Write(GameManager.players[Client.instance.myId].transform.rotation);

            SendUDPData(_packet);
        }
    }

    public static void PlayerShootRB(Vector3 _facing)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerShootRB))
        {
            _packet.Write(_facing);

            SendTCPData(_packet);
        }
    }

    public static void PlayerShootLB(Vector3 _facing)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerShootLB))
        {
            _packet.Write(_facing);

            SendTCPData(_packet);
        }
    }

    public static void PlayerSendMessage(string _message)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerSendMessage))
        {
            _packet.Write(_message);

            SendTCPData(_packet);
        }
    }


    public static void PlayerJoinTeam(int _team)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerJoinTeam))
        {
            _packet.Write(_team);

            SendTCPData(_packet);
        }
    }

    public static void PlayerSetReady()
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerReadyStatus))
        {
            SendTCPData(_packet);
        }
    }
    #endregion
}
