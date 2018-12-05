using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;

public class TicTacToeNetwork : MonoBehaviour {

    SocketIOComponent socket;

    //start Panel
    public RectTransform startPanel;
    public Text startMessageText;
    public Button connectButton;
    public Button closeButton;

    private Vector2 startPanelPos;
    private PlayerType playerType;


    void Start () {
        GameObject so = GameObject.Find("SocketIO");
        socket = so.GetComponent<SocketIOComponent>();

        socket.On("joinRoom", JoinRoom);
        socket.On("createRoom", CreateRoom);
        socket.On("exitRoom", ExitRoom);
    }
    public void Connect()   //소켓접속 (수동)
    {
        socket.Connect();
    }
    public void Close()     //소켓접속 해제
    {
        socket.Close();
    }
    void ExitRoom(SocketIOEvent e)
    {
        Debug.Log("Exit Room.");
        socket.Close();
    }
    void JoinRoom(SocketIOEvent e)
    {
        Debug.Log("Join Room.");
        string roomID = e.data.GetField("room").str;

    }
    void CreateRoom(SocketIOEvent e)
    {
        Debug.Log("Create Room.");
        string roomID = e.data.GetField("room").str;
        playerType = PlayerType.PlayerOne;
    }



}
