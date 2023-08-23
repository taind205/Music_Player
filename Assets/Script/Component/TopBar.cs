using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TopBar : MonoBehaviour
{
    public TMPro.TMP_InputField search_input;
    public TMPro.TextMeshProUGUI username_txt;
    public music_flow music_Flow;
    public Button search_btn;
    public Button setting_btn;
    public Button logout_btn;
    public GameObject settingOption_panel;

    private void Show_SettingPanel()
    {
        StartCoroutine(TurnOnPanel());
    }

    private void Logout()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Login_Scene");
    }

    // Start is called before the first frame update
    void Start()
    {
        search_btn.onClick.AddListener(delegate {music_Flow.GoTo_SearchPage(search_input.text, true);});
        logout_btn.onClick.AddListener(delegate {Logout();});
        setting_btn.onClick.AddListener(delegate {Show_SettingPanel();});
        username_txt.text=music_flow.userName;
        //search_input.onSubmit.AddListener();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            settingOption_panel.SetActive(false);
        }
    }

    IEnumerator TurnOnPanel()
    {
        yield return new WaitForSeconds(0.1f);
        settingOption_panel.SetActive(true);
    }
}
