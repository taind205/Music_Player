using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playlistbar_script : MonoBehaviour
{
    public GameObject song_instance;
    public Transform song_instance_layout;
    private List<GameObject> all_song_display = new List<GameObject>();
    private RectTransform song_rtrf=new RectTransform();
    private int playlist_song_count=0;
    public Playlist currentPlaylist=null;
    public music_flow music_Flow;
    public TMPro.TextMeshProUGUI playlist_name;
    private Button btn_GoTo_PL;
    public Button btn_loop;
    public Button btn_shuff;
    private Image btn_loop_icon;
    private Image btn_shuff_icon;
    public playbar_script playBar;
    const float Song_instance_width = 480;

    public bool loop = false;
    public bool shuffle = false;

    public string GetNextSong(string Songid){
        for(int i=0;i<all_song_display.Count-1;i++)
        {
            if(all_song_display[i].name==Songid)
                return all_song_display[i+1].name;
        }

        return null;
    }

    public string GetPreviousSong(string Songid){
        for(int i=1;i<all_song_display.Count;i++)
        {
            if(all_song_display[i].name==Songid)
                return all_song_display[i-1].name;
        }

        return null;
    }

    public string GetRandomSong_Otherthan(string Songid){
        if(all_song_display.Count==1)
            return null;
        else
        {
            string random;
            while(true)
            {
                random = all_song_display[((int)(Random.Range(0f,1f)*all_song_display.Count))].name;
                if(random!=Songid)
                    return random;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        song_rtrf = song_instance.GetComponent<RectTransform>();
        btn_GoTo_PL = playlist_name.gameObject.GetComponentInParent<Button>();
        btn_GoTo_PL.onClick.AddListener(delegate() {music_Flow.GoTo_PlaylistPage(); music_Flow.home_Anim_Script.Toggle_PlaylistBar();} );
        playlist_name.text = "Temporary playlist";
        // btn_loop_icon = btn_loop.GetComponentsInChildren<Image>()[1];
        // btn_shuff_icon = btn_shuff.GetComponentsInChildren<Image>()[1];
        // btn_loop.onClick.AddListener(delegate
        // {
        //     if(loop) {
        //         Toggle_Btn_Loop(false);
        //         playBar.Toggle_Btn_Loop(false);
        //     }
        //     else {
        //         Toggle_Btn_Loop(true);
        //         playBar.Toggle_Btn_Loop(true);
        //     }
        // });
        // btn_shuff.onClick.AddListener(delegate
        // {
        //    if(!shuffle) {
        //         Toggle_Btn_Shuff(true);
        //         playBar.Toggle_Btn_Shuff(true);
        //     }
        //     else {
        //         Toggle_Btn_Shuff(false);
        //         playBar.Toggle_Btn_Shuff(false);
        //     }
        // });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePlaylist()
    {
        foreach(GameObject gameObject in all_song_display)
        {
            Destroy(gameObject);
        }
        this.currentPlaylist = music_Flow.currentPlayingPlaylist;
        all_song_display.Clear();
        playlist_song_count=0;
        playlist_name.text = currentPlaylist.data.name;
        Debug.Log("Update playlistbar, Display there song:");
        foreach(Song song in currentPlaylist.GetListSong())
        {
            Debug.Log(song.data.title);
            DisplayInPlaylistBar(song);
        }
    // }
    // if(playlist.data.idPlaylist==currentPlaylist.data.idPlaylist)
    // {
    //  foreach(Song song in playlist.GetListSong())
    //        if(currentPlaylist.TryAddSong(song)) //Update UI

    //     foreach(GameObject gameObject in all_song_display)
    //         foreach(Song song in playlist.GetListSong())
    //             if(gameObject.name==song.data.id)
    //                 {
    //                     Destroy(gameObject);
    //                     all_song_display.Remove(gameObject);
    //                     playlist_song_count--;
    //                 }
    //     StartCoroutine(DisplayInPlaylistBar(song));
    // // }
    // // else
    // {
    }

    void DisplayInPlaylistBar(Song song)
    {
        
        // Vector3 world_pos = new Vector3(song_rtrf.position.x, 
        //                         song_rtrf.position.y-(song_rtrf.rect.height*1.2f*playlist_song_count),0);

        GameObject new_song_displayed = Instantiate(song_instance,song_instance_layout);
        new_song_displayed.SetActive(true);
        new_song_displayed.name=song.data.id;
        all_song_display.Add(new_song_displayed);
        Component[] all_img = new_song_displayed.GetComponentsInChildren<Image>();
        Image song_img = (Image)all_img[1];
        Image progress_img = (Image)all_img[2];
        TMPro.TextMeshProUGUI song_name = new_song_displayed.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        Button song_btn = new_song_displayed.GetComponent<Button>();

        song_btn.onClick.AddListener(delegate() {music_Flow.PlaySong(song.data.id);} );
        //Debug.Log("Add click listener on playlistbar for song: "+song.data.id);
        
        song_name.text = song.data.title;
        //song_rtrf.SetParent(suggest_songs_parent);
        new_song_displayed.SetActive(true);
        //song_rtrf.Translate(song_rtrf.rect.width*(1.2f*suggest_song_count), 0f, 0f);
        //song_rtrf.position=(Vector3.right*song_rtrf.rect.width*(1.2f*suggest_song_count));
        //Debug.Log("Move: " + Vector3.right*song_rtrf.rect.width*(1.2f*playlist_song_count));
        playlist_song_count+=1;

        StartCoroutine(FillSongImg(song, song_img));
        StartCoroutine(Display_DownloadProgress(song, progress_img));
        
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
                Debug.Log("Load img on playlist bar complete");
                yield break;
            }
            yield return new WaitForSeconds(0.4f);
        }
    }

    IEnumerator Display_DownloadProgress(Song song, Image progress_img)
    {
        while(song.audioclip==null)
        {
            progress_img.rectTransform.offsetMax=new Vector2(song.Get_AudioDownload_Progress()*Song_instance_width,0);
            yield return new WaitForSeconds(0.3f);
        }
        progress_img.rectTransform.offsetMax=new Vector2(Song_instance_width,0);
    }

}

    

//{
// public void Toggle_Btn_Shuff(bool on)
    // {
    //     if(on)
    //     {
    //         btn_shuff_icon.color = new Color(1f,1f,1f,1f);
    //         shuffle=true;
    //     }
    //     else
    //     {
    //         btn_shuff_icon.color = new Color(1f,1f,1f,0.5f);
    //         shuffle=false;
    //     }
    // }

    // public void Toggle_Btn_Loop(bool on)
    // {
    //     if(on)
    //     {
    //         btn_loop_icon.color = new Color(1f,1f,1f,1f);
    //         loop=true;
    //     }
    //     else
    //     {
    //         btn_loop_icon.color = new Color(1f,1f,1f,0.5f);
    //         loop=false;
    //     }
    // }
//}