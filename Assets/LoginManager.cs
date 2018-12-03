using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //UI 사용
using System.Text;      //변역
using UnityEngine.Networking;   //네트워크 사용

//회원가입 폼
public struct SignUpForm
{
    public string username;
    public string password;
    public string nickname;
}



public class LoginManager : MonoBehaviour {

    public Image signupPanel;
    public InputField usernameInputField;
    public InputField passwordInputField;
    public InputField confirmpasswordInputField;
    public InputField nicknameInputField;


    public void OnClickSignUpButton()
    {
        signupPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
        //signupPanel.GetComponent<RectTransform>().
    }
    public void OnClickConfirmButton()
    {
        string password = passwordInputField.text;
        string confirmPassword = confirmpasswordInputField.text;
        string username = usernameInputField.text;
        string nickname = nicknameInputField.text;

        if(string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword)
            || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(nickname))
        {
            return;
        }
        if (password.Equals(confirmPassword))
        {
            //Todo: 서버에 회원가입 정보 전송
            SignUpForm signUpForm = new SignUpForm();
            signUpForm.username = username;
            signUpForm.password = password;
            signUpForm.nickname = nickname;

            StartCoroutine(SignUp(signUpForm));
        }


    }
    public void OnClickCancel()
    {
        signupPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(900, 900);
    }

    IEnumerator SignUp(SignUpForm form)
    {
        string postData = JsonUtility.ToJson(form);
        byte[] sendData = Encoding.UTF8.GetBytes(postData);
        using (UnityWebRequest www = 
               UnityWebRequest.Put("http://localhost:3000/users/add", postData))
        {
            www.method = "POST";
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.Send();
            if(www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
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
