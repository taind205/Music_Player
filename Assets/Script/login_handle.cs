using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class login_handle : MonoBehaviour
{
    public GameObject login_panel;
    public GameObject signUp_panel;
    public TMPro.TMP_InputField login_email_input;
    public TMPro.TMP_InputField login_password_input;
    public TMPro.TMP_InputField signup_username_input;
    public TMPro.TMP_InputField signup_fullname_input;
    public TMPro.TMP_InputField signup_email_input;
    public TMPro.TMP_InputField signup_phoneNumber_input;
    public TMPro.TMP_InputField signup_password_input;
    public Button login_showhide_pass_btn;
    public Button signup_showhide_pass_btn;
    public Dropdown gender_picker;
    public Button login_btn;
    public Button signUp_btn;
    public Button goTo_signUp_btn;
    public Button goTo_login_btn;
    public Sprite hidepass_icon;
    public Sprite showpass_icon;
    private GameObject alert_text_panel_parent;
    private int textDisplay_count=0;
    public TMPro.TextMeshProUGUI alert_text;


    public const string SIGNUP_LINK = "https://musicappapi-production-95cf.up.railway.app/pmdv/db/user/add";
    public const string LOGIN_LINK = "https://musicappapi-production-95cf.up.railway.app/pmdv/db/user/verify";

    // Start is called before the first frame update
    void Start()
    {
        goTo_signUp_btn.onClick.AddListener(delegate() {GoTo_SignUpPanel();});
        signUp_btn.onClick.AddListener(delegate {StartCoroutine(SignUp());});
        goTo_login_btn.onClick.AddListener(delegate{GoTo_LoginPanel();});
        login_btn.onClick.AddListener(delegate{StartCoroutine(Login());});
        login_showhide_pass_btn.onClick.AddListener(delegate{ShowHidePass_Login();});
        signup_showhide_pass_btn.onClick.AddListener(delegate{ShowHidePass_SignUp();});
        alert_text_panel_parent = alert_text.transform.parent.parent.gameObject;
        GoTo_LoginPanel();
    }

    public void MakeText(string text)
    {
        textDisplay_count++;
        StartCoroutine(Display_Text(text));
    }

     public IEnumerator Display_Text(string text)
    {
        alert_text_panel_parent.SetActive(true);
        alert_text.text = text;

        yield return new WaitForSeconds(3);

        textDisplay_count--;
        if(textDisplay_count==0)
            alert_text_panel_parent.SetActive(false);
    }

    public void GoTo_LoginPanel() {
        login_panel.SetActive(true);
        signUp_panel.SetActive(false);
    }

    public void GoTo_SignUpPanel() {
        login_panel.SetActive(false);
        signUp_panel.SetActive(true);
    }

    public IEnumerator SignUp()
    {
        string raw = "{\"name\":\""+signup_fullname_input.text
        +"\",\"username\":\""+signup_username_input.text
        +"\",\"password\":\""+signup_password_input.text
        +"\",\"gender\":\""+gender_picker.captionText.text
        +"\",\"email\":\""+signup_email_input.text
         +"\",\"phoneNumber\":\""+signup_phoneNumber_input.text
        +"\"}";

        Debug.Log(raw);

        using (UnityWebRequest www = UnityWebRequest.Post(SIGNUP_LINK, raw))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(raw);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            //www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            //www.SetRequestHeader("accept", "application/json; charset=UTF-8");
            www.SetRequestHeader("content-type","application/json; charset=UTF-8");
            yield return www.SendWebRequest();
            string json = www.downloadHandler.text;
            Debug.Log(json);
            General_Request general_Request = JsonUtility.FromJson<General_Request>(json);
            MakeText(general_Request.message);

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                www.uploadHandler.Dispose();
                Debug.Log("Sign Up complete?");
                ClearInput();
            }
        }
    }

    private void ClearInput()
    {
        signup_fullname_input.text="";
        signup_username_input.text="";
        signup_password_input.text="";
        gender_picker.captionText.text="";
        signup_email_input.text="";
        signup_phoneNumber_input.text="";
    }

    public IEnumerator Login()
    {
        string raw = "{\"email\":\""+login_email_input.text
         +"\",\"password\":\""+login_password_input.text
        +"\"}";
        Debug.Log(raw);

        using (UnityWebRequest www = UnityWebRequest.Post(LOGIN_LINK, raw))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(raw);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            //www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            //www.SetRequestHeader("accept", "application/json; charset=UTF-8");
            www.SetRequestHeader("Content-Type","application/json; charset=UTF-8");
            yield return www.SendWebRequest();
            string json = www.downloadHandler.text;
            Debug.Log(json);
            General_Request general_Request = JsonUtility.FromJson<General_Request>(json);
            login_password_input.text="";
            www.uploadHandler.Dispose();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
                MakeText(general_Request.message);
                //Debug.Log(www)
            }
            else
            {
                Debug.Log("Checking UserID...");
                string userid = general_Request.data.idUser;
                if(userid!=null && userid!="" && general_Request.status==200)
                {
                    music_flow.userID=userid;
                    music_flow.userName=general_Request.data.username;
                    Debug.Log("Login complete with user ID: "+userid);
                    SceneManager.LoadScene("Home_scene");
                }
                else MakeText(general_Request.message);
                //SceneManager.LoadScene("Home_scene");
            }
        }
    }

    private void ShowHidePass_SignUp()
    {
        if(signup_password_input.contentType==TMPro.TMP_InputField.ContentType.Standard)
            {
                signup_password_input.contentType=TMPro.TMP_InputField.ContentType.Password;
                signup_showhide_pass_btn.image.sprite=showpass_icon;
            }
        else{ 
            signup_password_input.contentType=TMPro.TMP_InputField.ContentType.Standard;
            signup_showhide_pass_btn.image.sprite=hidepass_icon;}

        signup_password_input.ForceLabelUpdate();
    }

    private void ShowHidePass_Login()
    {
        if(login_password_input.contentType==TMPro.TMP_InputField.ContentType.Standard)
            {
                login_password_input.contentType=TMPro.TMP_InputField.ContentType.Password;
                login_showhide_pass_btn.image.sprite=showpass_icon;
            }
        else {
            login_password_input.contentType=TMPro.TMP_InputField.ContentType.Standard;
            login_showhide_pass_btn.image.sprite=hidepass_icon;
        }

        login_password_input.ForceLabelUpdate();
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLogin()
    {
        SceneManager.LoadScene("Home_scene");
    }
}