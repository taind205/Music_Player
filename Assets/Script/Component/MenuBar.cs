using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBar : MonoBehaviour
{
    public Button my_music_btn;
    public Button home_btn;
    public Button recentSong_btn;
    public Button favoriteSong_btn;
    public Button deviceSong_btn;
    public music_flow music_Flow;

    // Start is called before the first frame update
    void Start()
    {
        my_music_btn.onClick.AddListener(delegate() 
                {music_Flow.GoTo_AllPlaylistPage(true); 
                music_Flow.home_Anim_Script.Toggle_MenuBar();} );
        
        home_btn.onClick.AddListener(delegate() {
                music_Flow.GoTo_HomePage(true); 
                music_Flow.home_Anim_Script.Toggle_MenuBar();} );

        recentSong_btn.onClick.AddListener(delegate() {
                music_Flow.GoTo_ListSongPage("recent_songs",true); 
                music_Flow.home_Anim_Script.Toggle_MenuBar();} );
        
        favoriteSong_btn.onClick.AddListener(delegate() {
                music_Flow.GoTo_ListSongPage("favorite_songs",true); 
                music_Flow.home_Anim_Script.Toggle_MenuBar();} );

        deviceSong_btn.onClick.AddListener(delegate() {
                music_Flow.MakeText("This feature is under development"); 
                music_Flow.home_Anim_Script.Toggle_MenuBar();} );
                
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
