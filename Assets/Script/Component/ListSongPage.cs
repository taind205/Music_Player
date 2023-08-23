using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListSongPage : MonoBehaviour
{
    public music_flow music_Flow;
    public SongTwoCol_Layout ListSong;
    public Button back_btn;
    public Image listSong_Img;
    public TMPro.TextMeshProUGUI listSong_Title;
    public TMPro.TextMeshProUGUI listSong_ReleaseDate;
    public TMPro.TextMeshProUGUI listSong_SongNum;
    public Sprite favoriteSong_icon;
    public Sprite recentSong_icon;
    //private int song_count;

    public void DisplayListSong(List<Song> listSong, string type, Song listSongInfo=null)
    {
        if(type=="album")
        {
            back_btn.gameObject.SetActive(true);
            listSong_Title.text = listSongInfo.data.title;
            listSong_ReleaseDate.text = "Release Date: "+listSongInfo.createDate;
            StartCoroutine(listSongInfo.FillSong_Img());
            StartCoroutine(FillImg(listSongInfo.img));
        }
        else if(type=="recent_songs")
        {
            back_btn.gameObject.SetActive(false);
            listSong_Title.text = "Recently listened songs";
            listSong_ReleaseDate.text = "";
            listSong_Img.sprite = recentSong_icon;
        }
        else if(type=="favorite_songs")
        {
            back_btn.gameObject.SetActive(false);
            listSong_Title.text = "Favorite songs";
            listSong_ReleaseDate.text = "";
            listSong_Img.sprite = favoriteSong_icon;
        }
        
        ListSong.RemoveAllDisplaySong();
        listSong_SongNum.text = "No songs";
        if(listSong!=null)
            foreach(Song song in listSong)
                ListSong.Load_Or_UpdateSong(song);
    }

    public void Load_or_UpdateSong(Song song)
    {
        ListSong.Load_Or_UpdateSong(song);
        int song_count=ListSong.song_displayed_count;
        if(song_count>1)
        {
            listSong_SongNum.text = song_count + " songs";
        }
        else if(song_count==1)
        {
            listSong_SongNum.text = "1 song";
        }
        else
        {
            listSong_SongNum.text = "No songs";
        }
    }

    IEnumerator FillImg(Texture2D img)
    {
        while(true)
        {
            if(img!=null)
            {
                listSong_Img.sprite = Sprite.Create(img, new Rect(0.0f, 0.0f, img.width, img.height), new Vector2(0.5f, 0.5f), 100.0f);
                break;
            }
            yield return new WaitForSeconds(0.4f);
        }
        Debug.Log("Playlist Img displayed");
    }

    // Start is called before the first frame update
    void Start()
    {
        back_btn.onClick.AddListener(delegate() {music_Flow.GoTo_HomePage(false);});
        ListSong.music_Flow = this.music_Flow;
        // ListSong.SetTitle("");   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
