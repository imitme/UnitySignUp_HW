using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;


public class NetworkManager : MonoBehaviour {

    SocketIOComponent socket;
	
	void Start () {
        GameObject io = GameObject.Find("SocketIO");
        socket = io.GetComponent<SocketIOComponent>();
	}
	
}
