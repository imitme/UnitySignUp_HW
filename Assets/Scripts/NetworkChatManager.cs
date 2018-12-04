using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;

public class NetworkChatManager : MonoBehaviour
{
    SocketIOComponent socket;

    public InputField messageInputField;
    public Text messageText;
    string nickname;

    void Start()
    {
        GameObject io = GameObject.Find("SocketIO");
        socket = io.GetComponent<SocketIOComponent>();

        nickname = "hong";  //TODO: 서버 닉네임 가져오기!
        socket.On("chat", UpdateMessage);
    }
    public void UpdateMessage(SocketIOEvent e )
    {
        //e에는 서버가 보내느 msg정보가 들어있다?
    //    var date = e.data;
      //  Debug.Log("Recived message...");

        string nick = e.data.GetField("nick").str;
        string msg = e.data.GetField("msg").str;

        messageText.text += string.Format(" {0} : {1}\n", nick, msg);
    }

    public void Send()
    {
        //자신의 메시지 화면에 표시
        string message = messageInputField.text;
        messageText.text += string.Format(" {0} : {1}\n", nickname, message);
        messageInputField.text = "";

        //자신이 입력한 메시지 서버에 전송
        JSONObject obj = new JSONObject();
        obj.AddField("nick", nickname);
        obj.AddField("msg", message);
        socket.Emit("message", obj);
       
    }
}