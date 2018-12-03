using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //UI
using UnityEngine.SceneManagement;
using UnityEngine.Networking;   //네트워킹
using System.Text;  //언어변환

public struct LoginForm
{
    public string username;
    public string password;

}


public class UIManager : MonoBehaviour
{
    public static string username;
    public static string password;
    public Text userNickName;

    public Text serverMassage;


    public void OnGetNickNameButtonClicked()
    {
        LoginForm loginForm = new LoginForm();
        loginForm.username = username;
        loginForm.password = password;


        StartCoroutine(GetNickName(loginForm));

        StartCoroutine(GetUserInfo());
    }

    IEnumerator GetUserInfo()
    {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/users/info"))
        {
            string username = PlayerPrefs.GetString("username");
            if (!string.IsNullOrEmpty(username))
            {
                www.SetRequestHeader("Cookie", "username=" + username);

            }

            yield return www.Send();
            serverMassage.text = www.downloadHandler.text;
        }

    }


    IEnumerator GetNickName(LoginForm form)
    {
        string postData = JsonUtility.ToJson(form);
   //     byte[] sendData = Encoding.UTF8.GetBytes(postData);
        using (UnityWebRequest www =
               UnityWebRequest.Put("http://localhost:3000/users/SendNickname", postData))
        {
            www.method = "POST";
            www.SetRequestHeader("Context-Type", "application/json");
            yield return www.Send();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string resultStr = www.downloadHandler.text;
                var result = JsonUtility.FromJson<LoginResult>(resultStr);
                if (result.result == 2)   //resultStr.Equals("success"))
                {
                    userNickName.text = www.downloadHandler.text;
                }
                

                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    /*
    IEnumerator GetNickName(LoginForm form)
    {
        string postData = JsonUtility.ToJson(form);
        byte[] sendData = Encoding.UTF8.GetBytes(postData);
        using (UnityWebRequest www =
               UnityWebRequest.Put("http://localhost:3000/users/SendNickname", postData))
        {
            www.method = "POST";
            www.SetRequestHeader("Context-Type", "application/json");
            yield return www.Send();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string resultStr = www.downloadHandler.text;
                userNickName.text = resultStr;

                Debug.Log(www.downloadHandler.text);
            }
        }
    }
    */


    public void OnSignUpSceneClicked()
    {
        SceneManager.LoadScene("LoginScene_0");
    }


}
