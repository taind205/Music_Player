using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class home_anim_script : MonoBehaviour
{
    public RectTransform MenuBar_Trf;
    public RectTransform PlaylistBar_Trf;
    public RectTransform PlayBar_Trf;
    public RectTransform Content_Panel_Trf;
    public Transform PlayBar_BtnToggle_Icon_Trf;
    public Transform PlaylistBar_BtnToggle_Icon_Trf;
    public GameObject PlaylistBar_BtnToggle;
    public UnityEngine.UI.Image playlist_btn_icon;
    private int preWidth;
    private int preHeight;
    
    GameObject obj;
    SideBar_Anim menubar_anim;
    SideBar_Anim playlistbar_anim;
    SideBar_Anim playbar_anim;

    // Start is called before the first frame update
    void Start()
    {
        //obj = this.gameObject;
        //menubar_anim = new SideBar_Anim(MenuBar_Trf, MenuBar_Sidepoint,4);
        //playlistbar_anim = new SideBar_Anim(PlaylistBar_Trf, PlaylistBar_Sidepoint,2);
        menubar_anim = MenuBar_Trf.gameObject.AddComponent<SideBar_Anim>();
        playlistbar_anim = PlaylistBar_Trf.gameObject.AddComponent<SideBar_Anim>(); // = new SideBar_Anim(PlaylistBar_Trf, PlaylistBar_Sidepoint,2);
        playbar_anim = PlayBar_Trf.gameObject.AddComponent<SideBar_Anim>();
        menubar_anim.SetProp(MenuBar_Trf,4,false);
        playlistbar_anim.SetProp(PlaylistBar_Trf,2,false);
        playbar_anim.SetProp(PlayBar_Trf,3,true);
        preHeight=540;
        preWidth=960;
        StartCoroutine(PreventScreenResolution());
        
    }

    IEnumerator PreventScreenResolution()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);
            if(Screen.width<Screen.height+Screen.height/3) // Not allow < 4:3 ratio
                {
                    Screen.SetResolution(preWidth,preHeight,false);
                    Debug.Log("Back to old res");
                }
            else
                {
                    
                    preHeight=Screen.height;
                    preWidth=Screen.width;
                }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Toggle_MenuBar()
    {
        menubar_anim.ToggleSideBar();
    }

    public void Toggle_PlaylistBar()
    {
        StartCoroutine(Toggle_PlaylistBar_Eff());
    }

    public IEnumerator Toggle_PlaylistBar_Eff()
    {
        playlistbar_anim.ToggleSideBar();
        yield return new WaitForSeconds(SideBar_Anim.sidebar_anim_time*1.8f);

        if(playlistbar_anim.isOnScreen)
        {
            PlaylistBar_BtnToggle_Icon_Trf.localScale = new Vector3(1,1,1);
            playlist_btn_icon.color = new Color(1f,1f,1f,1f);
        }
        else 
        {
            PlaylistBar_BtnToggle_Icon_Trf.localScale = new Vector3(1,-1,1);
            playlist_btn_icon.color = new Color(1f,1f,1f,0.3f);
        }
    }

    public void Toggle_PlayBar()
    {
        StartCoroutine(Toggle_PlayBar_Eff());
    }

    public IEnumerator Toggle_PlayBar_Eff()
    {
        if(playbar_anim.isOnScreen) //Playbar is going to down
        {
            PlaylistBar_BtnToggle.SetActive(true);
            PlaylistBar_Trf.offsetMin = new Vector2(PlaylistBar_Trf.offsetMin.x, 0);
            Content_Panel_Trf.offsetMin = new Vector2(Content_Panel_Trf.offsetMin.x, 0);
        }
        else //Playbar is going to up
        {
            PlaylistBar_BtnToggle.SetActive(false);
            
        }
        playbar_anim.ToggleSideBar();
        
        yield return new WaitForSeconds(SideBar_Anim.sidebar_anim_time*1.8f);

        if(!playbar_anim.isOnScreen)
        {
            PlayBar_BtnToggle_Icon_Trf.localScale = new Vector3(1,-1,1); //Playbar is downed 
        }
        else
        {
            PlayBar_BtnToggle_Icon_Trf.localScale = new Vector3(1,1,1); //Playbar is upped
            Content_Panel_Trf.offsetMin = new Vector2(Content_Panel_Trf.offsetMin.x, 240);            
            PlaylistBar_Trf.offsetMin = new Vector2(PlaylistBar_Trf.offsetMin.x, 240);
        }
    }

}
