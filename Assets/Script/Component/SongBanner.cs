using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongBanner : MonoBehaviour
{
    public GameObject song_instance;
    private Transform song_instance_layout;
    public music_flow music_Flow;
    private TMPro.TextMeshProUGUI title_text;
    private List<GameObject> allSongDisplayed = new List<GameObject>();
    private int all_song_count=0;
    private bool isAlbumSong=false;
    
    public void SetTitle(string title){
        if(this.title_text==null)
            this.title_text = this.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        else this.title_text.text = title;
    }

    public void SetType(bool isAlbumSong){
        this.isAlbumSong=isAlbumSong;
    }

    public void RemoveAllDisplaySong()
    {
        foreach(GameObject gameObject in allSongDisplayed)
        {
            all_song_count=0;
            Destroy(gameObject);
        }
        allSongDisplayed.Clear();
    }

    public void Load_Or_UpdateSong(Song song)
    {
        foreach(GameObject songDisplayed in allSongDisplayed)
            if(song.data.id==songDisplayed.name)
            {
                //only add new song. Exist song will only get update...
                return;
            }
        
        StartCoroutine(DisplaySong(song));
        all_song_count+=1;
    }

    IEnumerator DisplaySong(Song song)
    {
        
        // Vector3 world_pos = new Vector3(song_big_rtrf.position.x+song_big_rtrf.rect.width*1.2f*suggest_song_count, 
        // song_big_rtrf.position.y,0);
        GameObject new_song_displayed = Instantiate(song_instance,song_instance_layout);
        new_song_displayed.SetActive(true);
        new_song_displayed.name = song.data.id;
        allSongDisplayed.Add(new_song_displayed);

        Image song_img = new_song_displayed.GetComponent<Image>();
        TMPro.TextMeshProUGUI song_name = new_song_displayed.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        Button song_btn = new_song_displayed.GetComponent<Button>();
        //int i = suggest_song_count;

        if(isAlbumSong) song_btn.onClick.AddListener(delegate() {music_Flow.GoTo_ListSongPage("album",true,song.data.id);});
        else song_btn.onClick.AddListener(delegate() {music_Flow.PlaySong(song.data.id);} );
        Debug.Log("SB_Add click listener for song: "+song.data.id);
        
        song_name.text = song.data.title;
        //song_rtrf.SetParent(suggest_songs_parent);
        new_song_displayed.SetActive(true);
        //song_rtrf.Translate(song_rtrf.rect.width*(1.2f*suggest_song_count), 0f, 0f);
        //song_rtrf.position=(Vector3.right*song_rtrf.rect.width*(1.2f*suggest_song_count));
        //Debug.Log("Move: " + song_big_rtrf.rect.width*(1.2f*suggest_song_count));
        

        // Texture2D tex=song.img;
        // song_img.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

        Texture2D tex=null;

        while(true)
        {
            yield return new WaitForSeconds(0.4f);
            tex = song.img;
            if(tex!=null)
            {
                song_img.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        title_text = this.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        song_instance_layout = song_instance.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
