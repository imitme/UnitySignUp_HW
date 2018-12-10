using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   //UI 사용
using System.Text;      //변역
using UnityEngine.Networking;   //네트워크 사용
using UnityEngine.SceneManagement;

//회원가입 폼
public struct SignUpForm_0
{
    public string username;
    public string password;
    public string nickname;
}
public struct SignInForm
{
    public string username;
    public string password;
}

public struct LoginResult
{
    public int result;
}

public enum ResponseType
{
  INVALID_USERNAME =0,
  INVALID_PASSWORD,
  SUCCESS
}


public class LoginManager_0 : MonoBehaviour {


    public GameObject signupPanelObj;
    public GameObject LoginPanelObj;
    public Image signupPanel;
    public InputField usernameInputField;
    public InputField passwordInputField;
    public InputField confirmpasswordInputField;
    public InputField nicknameInputField;

    public InputField loginUsernameInputField;
    public InputField loginPasswordInputField;

    public Button loginButton;

    // Use this for initialization
    void Start()
    {
        /* 로그인 체크 확인하는 곳!
        StartCoroutine( CheckLoginUser());
        */
        signupPanelObj.SetActive(false);   //signupPanel.GetComponent<GameObject>().SetActive(false);
        LoginPanelObj.SetActive(false);
        loginButton.interactable = false;   //loginButton.enabled = false;

    }

    IEnumerator CheckLoginUser()
    {
        using( UnityWebRequest www = UnityWebRequest.Get("http://localhost:3000/users/checkLoginUser"))
        {
            yield return www.Send();
            SceneManager.LoadScene("LoginScene_Lobby");
        }
    }

    public void OnClickSignUpButton()
    {
        //signupPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);    //signupPanelObj.GetComponent<GameObject>().SetActive(true);
        signupPanelObj.SetActive(true);
    }
    public void OnClickOpenSignInButton()
    {
        //signupPanelObj.GetComponent<GameObject>().SetActive(true);
        LoginPanelObj.SetActive(true);   //LoginPanelObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
    }

    public void OnClickSignInButton()
    {
        string username = loginUsernameInputField.text;
        string password = loginPasswordInputField.text;

        if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))//둘다 널이면 함수 빠져 나오기!
        {
            return;
        }

        SignInForm signInForm = new SignInForm();
        signInForm.username = username;
        signInForm.password = password;

        loginButton.interactable = false;

        StartCoroutine(SignIn(signInForm));

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
            SignUpForm_0 signUpForm = new SignUpForm_0();
            signUpForm.username = username;
            signUpForm.password = password;
            signUpForm.nickname = nickname;

            StartCoroutine(SignUp(signUpForm));
        }


    }
    public void OnClickCancel()
    {
        //signupPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(900, 900);
        //signupPanel.GetComponent<GameObject>().SetActive(false);
        signupPanelObj.SetActive(false);
        LoginPanelObj.SetActive(false);

    }

    IEnumerator SignUp(SignUpForm_0 form)
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
                //회원가입 성공하면, SignUp 패널 닫기
                string result = www.downloadHandler.text;
                if(result.Equals("success"))
                {
                    signupPanelObj.SetActive(false); //signupPanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(560, 0);
                }
            }
        }
    }
    IEnumerator SignIn(SignInForm form)
    {
        string postData = JsonUtility.ToJson(form);
        //byte[] sendData = Encoding.UTF8.GetBytes(postData);
        using (UnityWebRequest www =
               UnityWebRequest.Put("http://localhost:3000/users/signin", postData))
        {
            www.method = "POST";
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.Send();

            loginButton.interactable = true;

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string cookie = www.GetResponseHeader("set-cookie");  //Dictionary<string, string> headers = www.GetResponseHeaders();
                int lastIdx = cookie.IndexOf(';');
                string sId = cookie.Substring(0, lastIdx);
                string username = cookie.Substring(9, lastIdx-9);  //string username = cookie.Substring(9,lastIdx-9); //문자열 필요부분(유저네임) 추출
                

                string resultStr = www.downloadHandler.text;
                var result = JsonUtility.FromJson<LoginResult>(resultStr); 
                
                if (result.result ==2)   //resultStr.Equals("success"))
                {
                    if (!string.IsNullOrEmpty(username))
                    {
                        PlayerPrefs.SetString("username", username);
                    }
                    if (!string.IsNullOrEmpty(sId))
                    {
                        PlayerPrefs.SetString("sId", sId);
                    }

                    UIManager.username = form.username;
                    UIManager.password = form.password;
                    //SceneManager.LoadScene("LoginScene_Lobby");
                    //SceneManager.LoadScene("Game");
                    SceneManager.LoadScene("TicTacToe");

                }
                Debug.Log(www.downloadHandler.text);
            }
        }
    }



    public void UpdateLoginInputFiled()
    {
        if(!string.IsNullOrEmpty(loginUsernameInputField.text) 
            && !string.IsNullOrEmpty(loginPasswordInputField.text))
        {
            //loginButton.enabled = true;
            loginButton.interactable = true;
        }
        else
        {
            //loginButton.enabled = false;
            loginButton.interactable = false;
        }
    }

    
	
}
