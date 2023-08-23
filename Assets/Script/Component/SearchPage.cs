using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchPage : MonoBehaviour
{
    public music_flow music_Flow;
    public SongBanner relatedAlbum;
    public SongTwoCol_Layout SearchResult;

    public void ClearSearchResult()
    {
        relatedAlbum.RemoveAllDisplaySong();
        SearchResult.RemoveAllDisplaySong();
    }

    // Start is called before the first frame update
    void Start()
    {
        relatedAlbum.music_Flow = this.music_Flow;
        SearchResult.music_Flow = this.music_Flow;
        SearchResult.SetTitle("Search result");
        relatedAlbum.SetTitle("Related Album");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

// {
    // private List<GameObject> SearchResult_allArtistDisplayed = new List<GameObject>();
    // public GameObject resultArtist_instance;
    // public Transform resultArtist_instance_layout;
    // private int resultArtist_displayed_count=0;
    // public GameObject resultSong_layout_instance;
    // public GameObject resultSong_layout_instance_parent;
    // private GameObject current_resultsong_layout;
    // private List<GameObject> SearchResult_allLayoutDisplayed = new List<GameObject>();
    // private int result_song_displayed_count=0;
    // public music_flow music_Flow;
    
    // public void LoadResultArtist(Song song)
    // {
    //     StartCoroutine(DisplayResultArtist(song));
    // }

    // IEnumerator DisplayResultArtist(Song song)
    // {
        
    //     // Vector3 world_pos = new Vector3(song_big_rtrf.position.x+song_big_rtrf.rect.width*1.2f*suggest_song_count, 
    //     // song_big_rtrf.position.y,0);
    //     GameObject new_resultArtist_displayed = Instantiate(resultArtist_instance,resultArtist_instance_layout);
    //     new_resultArtist_displayed.SetActive(true);
    //     SearchResult_allArtistDisplayed.Add(new_resultArtist_displayed);

    //     Image artist_img = new_resultArtist_displayed.GetComponent<Image>();
    //     TMPro.TextMeshProUGUI artist_name = new_resultArtist_displayed.GetComponentInChildren<TMPro.TextMeshProUGUI>();
    //     Button artist_btn = new_resultArtist_displayed.GetComponent<Button>();
    //     //int i = suggest_song_count;

    //     artist_btn.onClick.AddListener(delegate() {music_Flow.PlaySong(song.data.id);} );
    //     Debug.Log("Add click listener on search page for song: "+song.data.id);
        
    //     artist_name.text = song.data.title;
    //     //song_rtrf.SetParent(suggest_songs_parent);
    //     new_resultArtist_displayed.SetActive(true);
    //     //song_rtrf.Translate(song_rtrf.rect.width*(1.2f*suggest_song_count), 0f, 0f);
    //     //song_rtrf.position=(Vector3.right*song_rtrf.rect.width*(1.2f*suggest_song_count));
    //     //Debug.Log("Move: " + song_big_rtrf.rect.width*(1.2f*suggest_song_count));
    //     resultArtist_displayed_count+=1;

    //     // Texture2D tex=song.img;
    //     // song_img.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

    //     Texture2D tex=null;

    //     while(true)
    //     {
    //         yield return new WaitForSeconds(0.4f);
    //         tex = song.img;
    //         if(tex!=null)
    //         {
    //             artist_img.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
    //             break;
    //         }
    //     }
        
    // }

    // public void LoadResultSong(Song song)
    // {
    //     result_song_displayed_count++;
    //     StartCoroutine(DisplayResultSong(song));
    // }

    // IEnumerator DisplayResultSong(Song song)
    // {
    //     bool left = (result_song_displayed_count%2==1);
    //     if(left) //new layout
    //     {    
    //         GameObject new_trend_song_layout_displayed = Instantiate(resultSong_layout_instance,resultSong_layout_instance_parent.transform);
    //         SearchResult_allLayoutDisplayed.Add(new_trend_song_layout_displayed);
    //         current_resultsong_layout=new_trend_song_layout_displayed;
    //         new_trend_song_layout_displayed.SetActive(true);
    //     }
    //     GameObject new_trend_song_displayed = current_resultsong_layout.GetComponentsInChildren<Transform>()[1].gameObject;
    //     if(!left) //change
    //     {
    //         new_trend_song_displayed = Instantiate(new_trend_song_displayed,current_resultsong_layout.transform);
    //         Vector3 local_pos = new_trend_song_displayed.transform.localPosition;
    //         new_trend_song_displayed.transform.localPosition=new Vector3(-local_pos.x,local_pos.y,local_pos.z);
    //         Debug.Log(new_trend_song_displayed.name);
    //     }
    //     else
    //     {
    //         Debug.Log(new_trend_song_displayed.name);
    //     }

    //     Image song_img = new_trend_song_displayed.GetComponentsInChildren<Image>()[1];
    //     TMPro.TextMeshProUGUI[] song_info = new_trend_song_displayed.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
    //     Button song_btn = new_trend_song_displayed.GetComponent<Button>();
    //     //int i = suggest_song_count;

    //     song_btn.onClick.AddListener(delegate() {music_Flow.PlaySong(song.data.id);} );
    //     Debug.Log("Add click listener on mainpage for trend song: "+song.data.title);
        
    //     song_info[0].text = song.data.title;
    //     song_info[1].text = song.data.artistsNames;
    //     song_info[2].text = song.data.genre;

    //     new_trend_song_displayed.SetActive(true);

    //     Texture2D tex=null;

    //     while(true)
    //     {
    //         yield return new WaitForSeconds(0.4f);
    //         tex = song.img;
    //         if(tex!=null)
    //         {
    //             song_img.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
    //             break;
    //         }
    //     }
        
    // }
    //}