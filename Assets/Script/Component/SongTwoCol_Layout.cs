using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongTwoCol_Layout : MonoBehaviour
{
    public music_flow music_Flow;
    public GameObject song_layout_instance;
    private int Len;
    private Transform song_layout_instance_parent;
    private TMPro.TextMeshProUGUI title_text;
    private List<GameObject> allLayoutDisplayed = new List<GameObject>();
    private List<GameObject> allSongDisplayed = new List<GameObject>();    
    private GameObject current_songLayout;
    public int song_displayed_count=0;

    public void SetTitle(string title){
        if(this.title_text==null)
            this.title_text = this.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        this.title_text.text = title;
    }

    public void RemoveAllDisplaySong()
    {

        foreach(GameObject gameObject in allLayoutDisplayed)
        {
            Destroy(gameObject);
        }

        song_displayed_count=0;
        allLayoutDisplayed.Clear();
        allSongDisplayed.Clear();
        current_songLayout=null;
    }

    // public void UpdateDisplaySong(Song id)
    // {
    // }


    public void Load_Or_UpdateSong(Song song)
    {
        foreach(GameObject songDisplayed in allSongDisplayed)
            if(song.data.id==songDisplayed.name)
            {
                //only add new song. Exist song will only get update...
                return;
            }
        

        song_displayed_count++;
        StartCoroutine(DisplaySong(song));
    }

    IEnumerator DisplaySong(Song song)
    {
        bool left = (song_displayed_count%2==1);
        if(left) //new layout
        {    
            GameObject new_songLayout_displayed = Instantiate(song_layout_instance,song_layout_instance_parent.transform);
            allLayoutDisplayed.Add(new_songLayout_displayed);
            current_songLayout=new_songLayout_displayed;
            new_songLayout_displayed.SetActive(true);
        }
        GameObject song_instance = current_songLayout.GetComponentsInChildren<Transform>(true)[1].gameObject;
        // trend_song_instance.SetActive(false);
        GameObject new_song_displayed = Instantiate(song_instance,current_songLayout.transform);
        
        new_song_displayed.name=song.data.id;
        allSongDisplayed.Add(new_song_displayed);
        
        if(!left) //change pos
        {
            // new_trend_song_displayed = Instantiate(new_trend_song_displayed,current_trendsong_layout.transform);
            // new_trend_song_displayed.name=song.data.id;
            // TrendSong_allSongDisplayed.Add(new_trend_song_displayed);
            Vector3 local_pos = new_song_displayed.transform.localPosition;
            new_song_displayed.transform.localPosition=new Vector3(-local_pos.x,local_pos.y,local_pos.z);
            //Debug.Log(new_trend_song_displayed.name);
        }
        else
        {
            //Debug.Log(new_trend_song_displayed.name);
        }

        Image song_img = new_song_displayed.GetComponentsInChildren<Image>()[1];
        TMPro.TextMeshProUGUI[] song_info = new_song_displayed.GetComponentsInChildren<TMPro.TextMeshProUGUI>();
        Button song_btn = new_song_displayed.GetComponent<Button>();
        //int i = suggest_song_count;

        song_btn.onClick.AddListener(delegate() {music_Flow.PlaySong(song.data.id);} );
        Debug.Log("STCL_Add click listener for song: "+song.data.title);
        
        song_info[0].text = song.data.title;
        song_info[1].text = song.data.artistsNames;
        song_info[2].text = song.GetDate(true);

        new_song_displayed.SetActive(true);

        Texture2D tex=null;

        while(true)
        {
            Debug.Log("STC Try load Song Img");
            tex = song.img;
            if(tex!=null)
            {
                song_img.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                break;
            }
            yield return new WaitForSeconds(0.4f);
        }
        Debug.Log("Load Song Img On STC complete");
        
    }

    // Start is called before the first frame update
    void Start()
    {
        song_layout_instance_parent = song_layout_instance.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
