using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class TicTacToeNetwork : MonoBehaviour {

    SocketIOComponent socket;

	void Start () {
        GameObject so = GameObject.Find("SocketIO");
        socket = so.GetComponent<SocketIOComponent>();

        socket.On("joinRoom", JoinRoom);
        socket.On("createRoom", CreateRoom);
    }

    void JoinRoom(SocketIOEvent e)
    {
        Debug.Log("Join Room.");
    }
    void CreateRoom(SocketIOEvent e)
    {
        Debug.Log("Create Room.");
    }

}
