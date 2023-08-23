using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Playlist_Page : MonoBehaviour
{
    public GameObject song_instance;
    public GameObject song_list;
    public GameObject playlist_instance;
    public GameObject playlist_instance_layout;
    public GameObject ATP_panel_playlist_instance;
    public GameObject ATP_panel_playlist_instance_layout;
    public GameObject All_Playlist_Panel;
    public GameObject Playlist_Panel;
    public GameObject dimBackGround_Panel;
    public GameObject addToPlaylist_Panel;
    public GameObject createPlaylist_Panel;
    public GameObject confirm_Panel;
    public music_flow Music_Flow;
    public TMPro.TextMeshProUGUI playlist_name;
    public List<GameObject> all_song_displayed = new List<GameObject>();
    public List<GameObject> all_loaded_playlist_gameobj = new List<GameObject>();
    public List<Playlist> all_loaded_playlist = new List<Playlist>();
    public List<GameObject> all_playlist_onATPPanel = new List<GameObject>();
    public TMPro.TextMeshProUGUI num_song_select;
    public Button btn_Unselect;
    public Button btn_Playnext;
    public Button btn_RemoveFromPlaylist;
    public Button CreateNewPlaylist_btn;
    public TMPro.TMP_InputField playlist_name_input;
    public Button AddToPlaylist_btn;
    public Button ATP_CreateNewPlaylist_btn;
    public Button createNewPlaylistPanel_Create_btn;
    public Button createNewPlaylistPanel_Cancel_btn;
    public GameObject disable_btn_panel;
    public Button back_btn;
    private bool interlaced_bc_control_song=false; //var use to control bg color of song instance displayed.
    private bool interlaced_bc_control_playlist=false; //var use to control bg color of playlist instance displayed.
    private bool interlaced_bc_control_atpPanel; //var use to control bg color of playlist instance displayed in AddToPlaylist panel.
    private Dictionary<string,GameObject> selected_icon = new Dictionary<string, GameObject>();
    private string selected_playlist_id;
    public string current_displayed_playlist_id="0";

    private Dictionary<string,Button> selected_song = new Dictionary<string, Button>(); 
    //Note: disabledcolor for each button will be set the same as first normalColor

    public void Display_AllPlaylist(bool callAPI)
    {
        back_btn.gameObject.SetActive(false);
        Playlist_Panel.SetActive(false);
        Debug.Log("PLP Set to false");
        All_Playlist_Panel.SetActive(true);
        if(callAPI)
        {
            Music_Flow.GoTo_AllPlaylistPage(true);
        }
    }

    private GameObject GetPlaylist_GameObj(string idPlaylist)
    {
        foreach(GameObject playlist_gameobj in all_loaded_playlist_gameobj)
            {
                if(idPlaylist==playlist_gameobj.name)
                    return playlist_gameobj;
            }
        
        return null;
    }

    private Playlist GetPlaylistByID(string idPlaylist)
    {
        foreach(Playlist playlist in all_loaded_playlist)
            {
                if(playlist.data.idPlaylist==idPlaylist)
                    return playlist;
            }
        
        return null;
    }

    public void Load_or_Update_Playlist(Playlist playlistToLoad, bool overwrite = true)
    {
        if(playlistToLoad.data.idPlaylist==null)
            return;
        foreach(Playlist loaded_playlist in all_loaded_playlist)
            if(loaded_playlist.data.idPlaylist==playlistToLoad.data.idPlaylist)
                {
                    if(overwrite)
                        {
                            loaded_playlist.data = playlistToLoad.data;
                            if(All_Playlist_Panel.activeSelf)
                                UpdatePlaylistInfo(GetPlaylist_GameObj(playlistToLoad.data.idPlaylist),loaded_playlist);
                        }

                    return; // 
                }

        Init_Playlist_Instance(playlistToLoad);
    }

    void Init_Playlist_Instance(Playlist playlist)
    {
        GameObject new_playlist_displayed = Instantiate(playlist_instance,playlist_instance_layout.transform);
        new_playlist_displayed.name = playlist.data.idPlaylist;
        all_loaded_playlist_gameobj.Add(new_playlist_displayed);
        all_loaded_playlist.Add(playlist);
        
        Button[] playlist_btn = new_playlist_displayed.GetComponentsInChildren<Button>();
        if(interlaced_bc_control_playlist)
        {
            //Changes the button's Normal color to the new color.
            interlaced_bc_control_playlist=false;
            ColorBlock cb = playlist_btn[0].colors;
            cb.normalColor = new Color(0,0,0,0); // disabledcolor will be set the same as first normalColor 
            cb.disabledColor = cb.normalColor; 
            playlist_btn[0].colors = cb;
        }
        else interlaced_bc_control_playlist=true;
        string playlist_id = playlist.data.idPlaylist;
        playlist_btn[0].onClick.AddListener(delegate() {GoTo_Playlist(playlist_id);} );
        playlist_btn[1].onClick.AddListener(delegate() {Play_Playlist(playlist_id);} );
        playlist_btn[2].onClick.AddListener(delegate() {DeletePlaylist(playlist_id);} );

        UpdatePlaylistInfo(new_playlist_displayed, playlist);

        Debug.Log("Add click listener on playlistbar for playlist: "+ playlist.data.idPlaylist);
        
        
        //song_rtrf.SetParent(suggest_songs_parent);
        new_playlist_displayed.SetActive(true);
        //song_rtrf.Translate(song_rtrf.rect.width*(1.2f*suggest_song_count), 0f, 0f);
        //song_rtrf.position=(Vector3.right*song_rtrf.rect.width*(1.2f*suggest_song_count));

        return;
    }

    private void UpdatePlaylistInfo(GameObject playlist_displayed, Playlist playlist)
    {
        TMPro.TextMeshProUGUI[] playlist_info = playlist_displayed.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        playlist_info[0].text = playlist.data.name;
        int playlist_num_of_song = playlist.GetNumOfSong();
        if(playlist_num_of_song>2)
            playlist_info[1].text = playlist_num_of_song.ToString()+" songs";
        else
        {
            playlist_info[1].text = playlist_num_of_song.ToString()+" song";
        }
        playlist_info[2].text = playlist.data.dateCreate;
    }

    public void Display_Playlist(Playlist playlist)
    {
        back_btn.gameObject.SetActive(true);
        disable_btn_panel.SetActive(true);
        current_displayed_playlist_id=playlist.data.idPlaylist;
        All_Playlist_Panel.SetActive(false);
        Playlist_Panel.SetActive(true);
        foreach(GameObject gameObject in all_song_displayed)
        {
            Destroy(gameObject);
        }
        playlist_name.text = playlist.data.name;
        num_song_select.text = "";
        selected_song.Clear();
        selected_icon.Clear();
        interlaced_bc_control_song=false;

        foreach(Song song in playlist.GetListSong())
        {
        StartCoroutine(Init_Song_Instance(song));
        }
    }

    IEnumerator Init_Song_Instance(Song song)
    {
        GameObject new_song = Instantiate(song_instance,song_list.transform);
        all_song_displayed.Add(new_song);

        TMPro.TextMeshProUGUI[] song_info = new_song.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        Image[] images = new_song.GetComponentsInChildren<Image>();
        Image song_img = images[2];
        Image select_icon = images[1];
        select_icon.gameObject.SetActive(false);
        selected_icon.TryAdd(song.data.id,select_icon.gameObject);
        
        Button song_btn = new_song.GetComponent<Button>();
        if(interlaced_bc_control_song)
        {
            //Changes the button's Normal color to the new color.
            interlaced_bc_control_song=false;
            ColorBlock cb = song_btn.colors;
            cb.normalColor = new Color(0,0,0,0); // disabledcolor will be set the same as first normalColor 
            cb.disabledColor = cb.normalColor; 
            song_btn.colors = cb;
        }
        else interlaced_bc_control_song=true;
        song_btn.onClick.AddListener(delegate() {SelectSong(song.data.id, song_btn);} );


        Debug.Log("Add click listener on playlistbar for song: "+song.data.id);
        
        song_info[0].text = song.data.title;
        song_info[1].text = song.data.artistsNames;
        song_info[2].text = song.genres;
        song_info[3].text = song.GetDate();
        song_info[4].text = song.GetLength();
        //song_rtrf.SetParent(suggest_songs_parent);
        new_song.SetActive(true);
        //song_rtrf.Translate(song_rtrf.rect.width*(1.2f*suggest_song_count), 0f, 0f);
        //song_rtrf.position=(Vector3.right*song_rtrf.rect.width*(1.2f*suggest_song_count));

        Texture2D tex=null;
        
        while(true)
        {
            yield return new WaitForSeconds(0.4f);
            tex = song.img;
            if(tex!=null)
            {
                song_img.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                Debug.Log("Load img complete, playlist");
                break;
            }
        }
    }

    void SelectSong(string song_id, Button btn)
    {
        Debug.Log(song_id);
        Debug.Log(btn);
        
        if(selected_song.ContainsKey(song_id)) 
        {
            Set_NormalBtn(song_id);
            selected_song.Remove(song_id);
        }
        else
        {
            selected_song.TryAdd(song_id,btn);
            Set_HighlightBtn(song_id);
        }
        if(selected_song.Count==0) {
            num_song_select.text="";
            disable_btn_panel.SetActive(true);
        }
        else 
        {
            num_song_select.text=selected_song.Count.ToString()+" song(s) selected";
            disable_btn_panel.SetActive(false);
        }

        foreach(string i in selected_song.Keys)
        {
            Debug.Log(i);  
        }
    }

    void Set_HighlightBtn(string id)
    {
        //Changes the button's Normal color to the new color.
        ColorBlock cb = selected_song[id].colors;
        cb.normalColor = cb.selectedColor;
        selected_song[id].colors = cb;
        selected_icon[id].SetActive(true);
    }

    void Set_NormalBtn(string id)
    {
        //Changes the button's Normal color to the new color.
        ColorBlock cb = selected_song[id].colors;
        cb.normalColor = cb.disabledColor; // disabledcolor will be set the same as first normalColor 
        selected_song[id].colors = cb;
        selected_icon[id].SetActive(false);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }

    public void UnSelectAll()
    {
        foreach(string i in selected_song.Keys)
        {
            Set_NormalBtn(i);
        }
        disable_btn_panel.SetActive(true);
        num_song_select.text="";
        selected_song.Clear();
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }

    public void Play_SelectedSong()
    {
        List<string> song_ids = new List<string>();
        foreach(string i in selected_song.Keys)
            song_ids.Add(i);
        Music_Flow.AddSong_And_PlayTempPlaylist(song_ids);
    }

    public void Open_AddToPlaylist_Panel()
    {
        dimBackGround_Panel.SetActive(true);
        addToPlaylist_Panel.SetActive(true);
        interlaced_bc_control_atpPanel=false;

        foreach(GameObject gameObject in all_playlist_onATPPanel)
            Destroy(gameObject);
        foreach(Playlist playlist in Music_Flow.allPlaylistloaded)
        {
            Init_Playlist_Instance_ATP_Panel(playlist);
        }
    }

    public void OpenCreatePL_Panel()
    {
        playlist_name_input.text="";
        dimBackGround_Panel.SetActive(true);
        createPlaylist_Panel.SetActive(true);
    }

    public void Create_And_AddToPlaylist_SelectedSong()
    {
        Playlist playlist = new Playlist();
        //playlist.data.idPlaylist;
        playlist.data.dateCreate=System.DateTime.Now.ToString("dd/MM/yyyy");
        playlist.data.name = playlist_name_input.text;
        List<string> selectedSong_IDs = new List<string>();
        foreach(string song_id in selected_song.Keys)
            selectedSong_IDs.Add(song_id);
        Music_Flow.CreateNewPlaylist(playlist,false,false,selectedSong_IDs);
        //AddSelectedSong_ToSelectedPlaylist(playlist.data.idPlaylist);
        createPlaylist_Panel.SetActive(false);
        addToPlaylist_Panel.SetActive(false);
        dimBackGround_Panel.SetActive(false);
        UnSelectAll();
    }

    public void CancelCreate_Playlist()
    {
        dimBackGround_Panel.SetActive(false);
        createPlaylist_Panel.SetActive(false);
    }

    void Init_Playlist_Instance_ATP_Panel(Playlist playlist)
    {
       
        GameObject new_playlist_displayed = Instantiate(ATP_panel_playlist_instance,ATP_panel_playlist_instance_layout.transform);
        all_playlist_onATPPanel.Add(new_playlist_displayed);
        TMPro.TextMeshProUGUI playlist_name = new_playlist_displayed.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        
        Button playlist_btn = new_playlist_displayed.GetComponent<Button>();
        Image playlist_bg = new_playlist_displayed.GetComponent<Image>();
        if(interlaced_bc_control_atpPanel)
        {
            interlaced_bc_control_atpPanel=false;
            playlist_bg.color= new Color(0.37647f,0.37647f,0.37647f,1);
        }
        else interlaced_bc_control_atpPanel=true;

        string playlist_id = playlist.data.idPlaylist;
        playlist_btn.onClick.AddListener(delegate() {AddSelectedSong_ToSelectedPlaylist(playlist_id);} );


        Debug.Log("Add click listener on playlistbar for playlist: "+ playlist_id);
        
        playlist_name.text = playlist.data.name;
        //song_rtrf.SetParent(suggest_songs_parent);
        new_playlist_displayed.SetActive(true);
        //song_rtrf.Translate(song_rtrf.rect.width*(1.2f*suggest_song_count), 0f, 0f);
        //song_rtrf.position=(Vector3.right*song_rtrf.rect.width*(1.2f*suggest_song_count));

        return;
    }

    public void AddSelectedSong_ToSelectedPlaylist(string playlist_id)
    {
        Debug.Log("Add "+selected_song.Count+" song to playlist "+playlist_id);
        foreach(string song_id in selected_song.Keys)
            Music_Flow.AddSong_ToPlaylist(song_id,playlist_id,true);
        
        addToPlaylist_Panel.SetActive(false);
        dimBackGround_Panel.SetActive(false);
        UnSelectAll();
    }

    public void RemoveSelectedSong_FromCurrentDisplayPlaylist()
    {
        Debug.Log("Remove Selected Song from current display playlist: "+current_displayed_playlist_id);
        List<string> song_ids = new List<string>();
        foreach(string id in selected_song.Keys)
            song_ids.Add(id);
        Music_Flow.RemoveSongs_FromPlaylist(song_ids,current_displayed_playlist_id);
        UnSelectAll();
        //Music_Flow.GoTo_PlaylistPage(current_displayed_playlist_id);
    }

    void Play_Playlist(string id){
        Debug.Log("Play playlist: "+id);
        Music_Flow.SetCurrentPlaylist(id);
        Music_Flow.Play_Current_Playlist();
    }

    void GoTo_Playlist(string id){
        Debug.Log("Play playlist: "+id);
        Music_Flow.GoTo_PlaylistPage(id);
    }

    void DeletePlaylist(string id){
        selected_playlist_id=id;
        dimBackGround_Panel.SetActive(true);
        confirm_Panel.SetActive(true);
    }

    public void Confirm_DeletePL(bool confirm)
    {
        if(confirm) 
        {
            Debug.Log("Confirm delete");
            foreach(GameObject gameObject in all_loaded_playlist_gameobj)
                Destroy(gameObject);
            all_loaded_playlist_gameobj.Clear();
            all_loaded_playlist.Clear();
            Music_Flow.DeletePlaylist(selected_playlist_id);
        }
        else Debug.Log("Cancel");
        dimBackGround_Panel.SetActive(false);
        confirm_Panel.SetActive(false);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        num_song_select.text="";
        btn_Unselect.onClick.AddListener(delegate() {UnSelectAll();} );
        btn_Playnext.onClick.AddListener(delegate() {Play_SelectedSong();} );
        btn_RemoveFromPlaylist.onClick.AddListener(delegate() {RemoveSelectedSong_FromCurrentDisplayPlaylist();} );
        // Playlist_Panel.SetActive(false);
        // Debug.Log("PLP Set to false");
        // listOf_Playlist_Panel.SetActive(false);

        CreateNewPlaylist_btn.onClick.AddListener(delegate{OpenCreatePL_Panel();});
        AddToPlaylist_btn.onClick.AddListener(delegate{Open_AddToPlaylist_Panel();});
        ATP_CreateNewPlaylist_btn.onClick.AddListener(delegate{OpenCreatePL_Panel();});
        createNewPlaylistPanel_Create_btn.onClick.AddListener(delegate{Create_And_AddToPlaylist_SelectedSong();});
        createNewPlaylistPanel_Cancel_btn.onClick.AddListener(delegate{CancelCreate_Playlist();});
        back_btn.onClick.AddListener(delegate{Display_AllPlaylist(true);});
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

// public void Display_ListOf_Playlist(List<Playlist> listof_pl)
    // {
    //     listOf_Playlist_Panel.SetActive(true);
    //     Playlist_Panel.SetActive(false);
    //     foreach(GameObject gameObject in all_playlist_loaded_gameobj)
    //     {
    //         Destroy(gameObject);
    //     }

    //     interlaced_bc_control=false;

    //     foreach(Playlist playlist in listof_pl)
    //     {
    //     Init_Playlist_Instance(playlist);
    //     }
    // }