using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;

public class TicTacToeNetwork : MonoBehaviour {
    //Socket.io
    SocketIOComponent socket;

    //start Panel
    public RectTransform startPanel;
    public Text startMessageText;
    public Button connectButton;
    public Button closeButton;

    TicTacToeManager gameManager;
    Vector2 startPanelPos;
    PlayerType playerType;

    string myRoomID;

    public void Connect()
    {
        socket.Connect();
        startMessageText.text = "상대를 기다리는 중...";
        connectButton.gameObject.SetActive(false);

    }

    public void Close()
    {
        socket.Close();
    }

    void Start () {
        GameObject so = GameObject.Find("SocketIO");
        socket = so.GetComponent<SocketIOComponent>();

        gameManager = GetComponent<TicTacToeManager>();

        socket.On("createRoom", CreateRoom);
        socket.On("joinRoom", JoinRoom);
        
        
        socket.On("startGame", StartGame);
        socket.On("doOpponent", DoOpponent);
        socket.On("exitRoom", ExitRoom);

        startPanel.gameObject.SetActive(true);    //test로 임시 주석 처리!

        closeButton.interactable = false;
    }

    void StartGame(SocketIOEvent e)
    {
        Debug.Log("Start Room.");
        startPanel.gameObject.SetActive(false);
        closeButton.interactable = true;

        //게임매니저에게 방을 만든사람인지 아닌지에대하 정보 같이 전달
        gameManager.StartGame(playerType);
    }

    void ExitRoom(SocketIOEvent e)
    {
        Debug.Log("Exit Room.");
        socket.Close();
    }

    void JoinRoom(SocketIOEvent e)
    {
        Debug.Log("Join Room.");
        string roomID = e.data.GetField("room").str;    //확인만 하고 있다. 보관도 함께!
        if (!string.IsNullOrEmpty(roomID))   //보관코드
        {
            myRoomID = roomID;
        }
        playerType = PlayerType.PlayerTwo;
        /*
        if (string.IsNullOrEmpty(roomID))
        {
            myRoomID = roomID;
        }*/
    }

    void CreateRoom(SocketIOEvent e)
    {
        Debug.Log("Create Room.");
        string roomID = e.data.GetField("room").str;    //확인만 하고 있다. 보관도 함께!
        if (!string.IsNullOrEmpty(roomID))   //보관코드
        {
            myRoomID = roomID;
        }
        playerType = PlayerType.PlayerOne;
        
    }

    

    //플레이어 게임 정보 서버로 전송
    public void DoPlayer(int index)
    {
        JSONObject playInfo = new JSONObject();
        playInfo.AddField("position", index);
        playInfo.AddField("room", myRoomID);    //해당 룸 사용자에게 메세지보내기 위해서 만듬.

        socket.Emit("doPlayer", playInfo);
    }
    
    void DoOpponent(SocketIOEvent e)
    {
        int cellIndex = -1;
        e.data.GetField(ref cellIndex, "position");//인덱스로 드로우 마커 만들어 상대동작 확인?가능
        gameManager.DrawMark(cellIndex, Player.Opponent); //상대방 턴 반영!
    }
}
