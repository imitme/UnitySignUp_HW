using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UIGameManager : MonoBehaviour {

    public Text serverMessage;
    public InputField scoreInputField;
    public Text scoreText;

    public void OnClickAddScore()
    {
        string scoreStr = scoreInputField.text;
        if (!string.IsNullOrEmpty(scoreStr))
        {
            StartCoroutine(AddScore(scoreStr));
        }
        
    }
    public void OnClickGetScore()
    {
        StartCoroutine(GetScore());
    }
    IEnumerator AddScore(string score)
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/users/addscore/" + score))
        {
            string sId = PlayerPrefs.GetString("sId");
            if (!string.IsNullOrEmpty(sId))
            {
                www.SetRequestHeader("Cookie", sId);
            }
            yield return www.Send();
            string resultStr = www.downloadHandler.text;

            
        }
    }
    IEnumerator GetScore()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/users/score/" ))
        {
            string sId = PlayerPrefs.GetString("sId");
            if (!string.IsNullOrEmpty(sId))
            {
                www.SetRequestHeader("Cookie", sId);
            }
            yield return www.Send();
            string resultStr = www.downloadHandler.text;

            if (!string.IsNullOrEmpty(resultStr))
            {
                scoreText.text = resultStr;
            }
        }
    }



    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
