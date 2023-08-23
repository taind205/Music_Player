using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class song_info_page : MonoBehaviour
{
    public music_flow music_Flow;
    private Song currentDisplayedSong;
    public Image song_img;
    public TMPro.TextMeshProUGUI song_info_txt;
    public TMPro.TextMeshProUGUI lyrics_txt;
    public GameObject dimBackGround_Panel;
    public GameObject addToPlaylist_Panel;
    public GameObject ATP_panel_playlist_instance;
    public GameObject ATP_panel_playlist_instance_layout;
    public GameObject createPlaylist_Panel;
    public TMPro.TMP_InputField playlist_name_input;
    public Button btn_Playnext;
    public Button AddToPlaylist_btn;
    public Button ATP_CreateNewPlaylist_btn;
    public Button createNewPlaylistPanel_Create_btn;
    public Button createNewPlaylistPanel_Cancel_btn;
    public Button favorite_btn;
    public Sprite favorite_icon;
    public Sprite notfavorite_icon; 
    public List<GameObject> all_playlist_onATPPanel = new List<GameObject>();
    public Sprite default_songIMG;
    private bool interlaced_bc_control_atpPanel; //var use to control bg color of playlist instance displayed in AddToPlaylist panel.
    private bool isFavoriteSong;

    public bool Show_SongInfo(Song song)
    {
        currentDisplayedSong=song;
        string song_info="<line-height=140%><font=\"ShantellSans-Italic-VariableFont_BNCE,INFM,SPAC,wght SDF\"><align=\"center\"><size=140%>"+song.data.title+"</line-height></size>"
        + "\n<line-height=120%><b><color=#82f5ff><size=120%>"+ song.data.artistsNames +"</color></size></b></font></align></line-height>"
        + "\n<size=70%> </size>"
        + "\n"+"Đã phát hành: "+song.GetDate(true);

        if(song.genres!="")
            song_info += "\n"+"Thể loại: "+ song.genres;
        
        song_info+= "\n"+song.info;
        
        //string lyrics=song.data.lyrics;
        StartCoroutine(music_Flow.aPI_Call.GetRequest(API_Call.GET_SONG_LYRICS.Replace("*",song.data.id),"lyrics"));
        song_info_txt.text=song_info;

        favorite_btn.image.sprite = notfavorite_icon;
        isFavoriteSong=false;
        foreach(string id in music_Flow.favorite_song_ids)
            if(id==song.data.id)
                {
                    favorite_btn.image.sprite = favorite_icon;
                    isFavoriteSong=true;
                }

        StartCoroutine(FillSongImg(song, song_img));

        return true;
    }

    private void Add_or_Remove_FavoriteSong()
    {
        isFavoriteSong=(!isFavoriteSong);
        music_Flow.Add_or_Remove_FavoriteSong(isFavoriteSong,currentDisplayedSong.data.id);
        
        if(isFavoriteSong)
            favorite_btn.image.sprite = favorite_icon;
        else
            favorite_btn.image.sprite = notfavorite_icon;

        
    }

    public void Set_Default_SongInfoUI()
    {
        //currentDisplayedSong=song;
        // string song_info="<line-height=140%><font=\"ShantellSans-Italic-VariableFont_BNCE,INFM,SPAC,wght SDF\"><align=\"center\"><size=140%>"+song.data.title+"</line-height></size>"
        // + "\n<line-height=120%><b><color=#82f5ff><size=120%>"+ song.data.artistsNames +"</color></size></b></font></align></line-height>"
        // + "\n<size=70%> </size>"
        // + "\n"+"Thể loại: "+ song.genres
        // + "\n"+"Ngày phát hành: "+song.GetDate(true)
        // + "\n"+song.info;
        
        //string lyrics=song.data.lyrics;
        //StartCoroutine(music_Flow.aPI_Call.GetRequest(API_Call.GET_SONG_LYRICS.Replace("*",song.data.id),"lyrics"));
        song_info_txt.text="";
        lyrics_txt.text="";
        song_img.sprite = default_songIMG;

        return;
        
    }

    public void LoadLyrics(string lyrics)
    {
        lyrics_txt.text=lyrics;
    }

    IEnumerator FillSongImg(Song song, Image song_img)
    {
        Texture2D tex=null;

        while(true)
        {
            tex = song.img;
            if(tex!=null)
            {
                song_img.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                Debug.Log("Load img on song info page complete");
                yield break;
            }
            yield return new WaitForSeconds(0.4f);
        }
    }

    public void Play_CurrentSong()
    {
        music_Flow.PlaySong(currentDisplayedSong.data.id);
    }

    public void Open_AddToPlaylist_Panel()
    {
        dimBackGround_Panel.SetActive(true);
        addToPlaylist_Panel.SetActive(true);
        interlaced_bc_control_atpPanel=false;

        foreach(GameObject gameObject in all_playlist_onATPPanel)
            Destroy(gameObject);
        foreach(Playlist playlist in music_Flow.allPlaylistloaded)
        {
            Init_Playlist_Instance_ATP_Panel(playlist);
        }
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
        playlist_btn.onClick.AddListener(delegate() {AddCurrentSong_ToSelectedPlaylist(playlist_id);} );


        Debug.Log("Add click listener on playlistbar for playlist: "+ playlist_id);
        
        playlist_name.text = playlist.data.name;
        //song_rtrf.SetParent(suggest_songs_parent);
        new_playlist_displayed.SetActive(true);
        //song_rtrf.Translate(song_rtrf.rect.width*(1.2f*suggest_song_count), 0f, 0f);
        //song_rtrf.position=(Vector3.right*song_rtrf.rect.width*(1.2f*suggest_song_count));

        return;
    }

    public void AddCurrentSong_ToSelectedPlaylist(string playlist_id)
    {
        Debug.Log("Add "+currentDisplayedSong.data.title+" song to playlist "+playlist_id);
        //foreach(string song_id in selected_song.Keys)
        music_Flow.AddSong_ToPlaylist(currentDisplayedSong.data.id,playlist_id,true);
        
        addToPlaylist_Panel.SetActive(false);
        dimBackGround_Panel.SetActive(false);
    }

    public void OpenCreatePL_Panel()
    {
        playlist_name_input.text="";
        dimBackGround_Panel.SetActive(true);
        createPlaylist_Panel.SetActive(true);
    }

    public void Create_And_AddToPlaylist_CurrentSong()
    {
        Playlist playlist = new Playlist();
        //playlist.data.idPlaylist;
        playlist.data.dateCreate=System.DateTime.Now.ToString("dd/MM/yyyy");
        playlist.data.name = playlist_name_input.text;
        List<string> songAddToPlaylist = new List<string>();
        songAddToPlaylist.Add(currentDisplayedSong.data.id);
        //foreach(string song_id in selected_song.Keys)
        //    selectedSong_IDs.Add(song_id);
        music_Flow.CreateNewPlaylist(playlist,false,false,songAddToPlaylist);
        //AddSelectedSong_ToSelectedPlaylist(playlist.data.idPlaylist);
        createPlaylist_Panel.SetActive(false);
        addToPlaylist_Panel.SetActive(false);
        dimBackGround_Panel.SetActive(false);
    }

    public void CancelCreate_Playlist()
    {
        dimBackGround_Panel.SetActive(false);
        createPlaylist_Panel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        btn_Playnext.onClick.AddListener(delegate() {Play_CurrentSong();} );
        AddToPlaylist_btn.onClick.AddListener(delegate{Open_AddToPlaylist_Panel();});
        ATP_CreateNewPlaylist_btn.onClick.AddListener(delegate{OpenCreatePL_Panel();});
        createNewPlaylistPanel_Create_btn.onClick.AddListener(delegate{Create_And_AddToPlaylist_CurrentSong();});
        createNewPlaylistPanel_Cancel_btn.onClick.AddListener(delegate{CancelCreate_Playlist();});
        favorite_btn.onClick.AddListener(delegate() {Add_or_Remove_FavoriteSong();});
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
