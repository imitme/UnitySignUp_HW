using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using UnityEngine.UI;



public class NetworkManager : MonoBehaviour {

    public Text resultText;
    SocketIOComponent socket;
	
	void Start () {
        

        GameObject io = GameObject.Find("SocketIO");
        socket = io.GetComponent<SocketIOComponent>();

        socket.On("hello", Hello); //메세지 등록
    }

    void Hello(SocketIOEvent e )
    {
        Debug.Log("Hello");
        resultText.text = "Hello";
    }
    public void Hi()
    {
        socket.Emit("hi");
    }
}
