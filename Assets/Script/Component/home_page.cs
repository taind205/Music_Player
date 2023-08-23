using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class home_page : MonoBehaviour
{
    public music_flow music_Flow;
    public SongBanner HotAlbum;
    public SongBanner New_Songs;

    public SongTwoCol_Layout trendingSong;

    // private RectTransform song_big_rtrf;
    
    // public GameObject suggest_song_instance;
    // public Transform suggest_song_instance_layout;
    
    // private List<GameObject> SuggestSong_allSongDisplayed = new List<GameObject>();
    // private List<GameObject> TrendSong_allLayoutDisplayed = new List<GameObject>();
    // private List<GameObject> TrendSong_allSongDisplayed = new List<GameObject>();
    // public GameObject trendSong_layout_instance;
    // public GameObject trendSong_layout_instance_parent;
    // private GameObject current_trendsong_layout;
    // private int trend_song_displayed_count=0;

    

    // Start is called before the first frame update
    void Start()
    {
        HotAlbum.SetType(true);
        HotAlbum.music_Flow = this.music_Flow;
        New_Songs.music_Flow = this.music_Flow;
        trendingSong.music_Flow = this.music_Flow;
        HotAlbum.SetTitle("Hot playlist");
        New_Songs.SetTitle("New songs");
        trendingSong.SetTitle("Trendings");
        //song_big_rtrf = suggest_song_instance.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
