using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class music_flow : MonoBehaviour
{
    List<Song> allSongsLoaded = new List<Song>();
    List<Song> newSongs = new List<Song>();
    List<Song> trendingSongs = new List<Song>();
    List<Song> bannerAlbum = new List<Song>();
    List<Song> searchSongResult = new List<Song>();
    List<Song> bannerSong = new List<Song>();
    public List<string> favorite_song_ids = new List<string>();
    public Playlist currentPlayingPlaylist = new Playlist();
    public List<Playlist> allPlaylistloaded = new List<Playlist>();
    public playbar_script playbar;
    public home_page home_Page;
    public SearchPage searchPage;
    public playlistbar_script playlistbar;
    public song_info_page song_Info_Page;
    public Playlist_Page playlist_Page;
    public ListSongPage listSongPage;
    public static string current_playing_song_id;
    private GameObject currentPage;
    private Playlist temp_playlist=new Playlist();
    //public static string current_playlistname="Temporary Playlist";
    public bool isLoading=false;
    public TMPro.TextMeshProUGUI alert_text;
    private GameObject alert_text_panel_parent;
    
    public home_anim_script home_Anim_Script;
    public API_Call aPI_Call;
    public static string userID="UCIJ8IGZ";
    public static string userName="";
    private string current_display_UI="";
    private int textDisplay_count=0;

    public void Add_or_Remove_FavoriteSong(bool isFavoriteSong, string idSong)
    {
        if(!isFavoriteSong)
            StartCoroutine(API_Call.DeleteRequest(API_Call.REMOVE_FROM_FAVORITE_SONG.Replace("**",idSong)
                        .Replace("*",userID)));
        else
            StartCoroutine(API_Call.PostRequest(API_Call.ADD_TO_FAVORITE_SONG.Replace("**",idSong)
                        .Replace("*",userID)));

        favorite_song_ids.Remove(idSong);
    }

    // public void LoadSongToSearchPage(Song song)
    // {
    //     searchPage.SearchResult.Load_Or_UpdateSong(song);
    //     {
    //         StartCoroutine(aPI_Call.GetSourceSong(song.data.id,song));
    //         StartCoroutine(song.FillSong_Img());
    //     }
    //     searchSongResult.Add(song);
    // }

    public void UpdateUI(string ui_name=null){
        if(ui_name==null)
            ui_name=current_display_UI;
        switch(current_display_UI) //handle when data is change.
        {
            case "list_song":
                // GoTo_ListSongPage("album",null,false);
                Debug.Log("UpdateUI(): Do nothing");
                break;
            case "playlist":
                break;
            case "all_user_playlists":
                GoTo_AllPlaylistPage(false);
            break;
            case "home":
                GoTo_HomePage(false);
                break;
            case "search":
                Debug.Log("UpdateUI() for search: Do nothing");
                break;
            default:
                Debug.Log("UI Name not match");
                break;
        }
    }

    public void MakeText(string text)
    {
        textDisplay_count++;
        StartCoroutine(Display_Text(text));
    }

    public IEnumerator Display_Text(string text)
    {
        alert_text_panel_parent.SetActive(true);
        alert_text.text = text;

        yield return new WaitForSeconds(3);

        textDisplay_count--;
        if(textDisplay_count==0)
            alert_text_panel_parent.SetActive(false);

    }

    public Song GetAlbumByID(string id)
    {
        foreach(Song album in bannerAlbum)
        {if(album.data.id==id) return album;}
        return null;
    }

    public Playlist GetPlaylistByID(string id)
    {
        foreach(Playlist playlist in allPlaylistloaded)
        {if(playlist.data.idPlaylist==id) return playlist;}
        if(id=="0")
            return temp_playlist;
        else
        return null;
    }

    public Song GetSongByID(string id)
    {
        foreach(Song song in allSongsLoaded)
        {if(song.data.id==id) return song;}
        foreach(Song song in searchSongResult)
        {if(song.data.id==id) return song;}
        return null;
    }

    public Song GetSongByID_InSearchResult(string id)
    {
        foreach(Song song in searchSongResult)
        {if(song.data.id==id) return song;}
        return null;
    }

    public bool TryAddSong(Song songToAdd, List<Song> list_song, bool overwrite=true)
    {
        foreach(Song song in list_song)
            if(song.data.id==songToAdd.data.id)
            {
                if(overwrite)
                    if(!song.data.Equals(songToAdd))
                        {
                            // song.data=songToAdd.data; //overwrite *** BUG: Disable overwrite SongDataFunction
                            Debug.Log("Data changed, update UI");
                            // UpdateUI();
                        }

                return false;
            }
        
        list_song.Add(songToAdd);
        Debug.Log("New Song detected, update UI");
        UpdateUI();

        return true;
    }

    public bool TryAddPlaylist(Playlist playlistToAdd, List<Playlist> list_playlist,bool overwrite=true)
    {
        foreach(Playlist playlist in list_playlist)
            if(playlist.data.idPlaylist==playlistToAdd.data.idPlaylist)
            {
                if(overwrite)
                    if(!playlist.data.Equals(playlistToAdd))
                    {
                        playlist.data=playlistToAdd.data; //Update Data
                        Debug.Log("Data changed, update UI");
                        UpdateUI();
                    }
                    
                return false; //Playlist already existed.
            }
        
        list_playlist.Add(playlistToAdd);
        Debug.Log("Data changed, update UI");
        UpdateUI();

        return true;
    }

    public void LoadSong(Song song, bool callAPI, string loadTo)
    {
        //StartCoroutine(song.Fill_AudioClip());
        Debug.Log("Load song "+song.data.id+" complete");
        if(TryAddSong(song, allSongsLoaded)) // new songs
        {
            StartCoroutine(aPI_Call.GetSourceSong(song.data.id,song));
            StartCoroutine(song.FillSong_Img());
        }
        else // Keep Old song
        {
            song = GetSongByID(song.data.id);
        }

        if(loadTo=="trending")
            TryAddSong(song,trendingSongs);
        else if(loadTo=="new")
            TryAddSong(song,newSongs);
        else if(loadTo=="banner_songs")
        {
            if(current_display_UI=="list_song")
                listSongPage.Load_or_UpdateSong(song);
            //TryAddSong(song,bannerSong);
        }
        else if(loadTo=="recent_songs")
        {
            if(current_display_UI=="list_song")
                listSongPage.Load_or_UpdateSong(song);
        }
        else if(loadTo=="favorite_songs")
        {
            if(current_display_UI=="list_song")
                listSongPage.Load_or_UpdateSong(song);
            favorite_song_ids.Add(song.data.id);
        }
        else if(loadTo=="search_page")
        {
            searchPage.SearchResult.Load_Or_UpdateSong(song);
            TryAddSong(song,searchSongResult);
        }
    }

    public void LoadAlbum(Song song, bool callAPI=true)
    {
        //StartCoroutine(song.Fill_AudioClip());
        Debug.Log("Load album "+song.data.id+" complete");
        if(TryAddSong(song, bannerAlbum)) // new album
        {
            StartCoroutine(song.FillSong_Img());
        }
    }

    public void PlaySong(string id)
    {
        Debug.Log("Play song "+id);
        Song songToPlay = GetSongByID(id);

        if(songToPlay!=null)
        {
            if(songToPlay.audioclip==null)
            StartCoroutine(songToPlay.Fill_AudioClip()); //start downloading audio
        }
        else //not find song
        {
            // songToPlay=GetSongByID_InSearchResult(id); //find in additional source
            // if(songToPlay!=null)
            // {
            //     if(songToPlay.audioclip==null)
            //         StartCoroutine(songToPlay.Fill_AudioClip()); //start downloading audio
            // }
            // else
            // {
                MakeText("Error: Song not exist...");
                return;
            // }
        }
        //StartCoroutine(GetSongByID(id).FillPartOfSong());
        if(!playbar.PlaySong(songToPlay))
            {
                MakeText("Song audio is loading...");
                if(playbar.currentPlayedSongID=="")
                    current_playing_song_id=id;
            }
        else current_playing_song_id=id;
        if(currentPlayingPlaylist==temp_playlist)
        {
            currentPlayingPlaylist.TryAddSong(songToPlay);
            playlistbar.UpdatePlaylist();
        }
        
        Debug.Log("API: Listen song"+id);
        StartCoroutine(API_Call.PostRequest(API_Call.ADD_TO_LISTEN_SONG.Replace("**",id).Replace("*",userID)));
    }

    public void Play_Current_Playlist(string songId="")
    {
        Debug.Log("Play current playlist");
        // Song song;
        if(songId=="")
            songId = currentPlayingPlaylist.GetFirstSong().data.id;
        PlaySong(songId);
        // if(!playbar.PlaySong(song))
        //     MakeText("Song audio is loading...");
        // current_playing_song_id=song.data.id;
    }

    public void SetCurrentPlaylist(string id)
    {
        currentPlayingPlaylist = GetPlaylistByID(id);
        playlistbar.UpdatePlaylist();
    }

    public void SongInfo()  //Show current song info
    {
        Song songToDisplay = GetSongByID(playbar.currentDisplayedSongID);
        if(songToDisplay!=null)
            {
                //if()
                {
                    SetCurrentPage(song_Info_Page.gameObject);
                    song_Info_Page.Set_Default_SongInfoUI();
                    StartCoroutine(aPI_Call.GetRequest(API_Call.GET_SONG_INFO.Replace("*",songToDisplay.data.id),"song_info"));
                }
                //else MakeText("Error displaying song info");
            }
        else MakeText("Error: Can't find song info");

        // Song songToDisplay = currentPlaylist.GetSong(current_playing_song_id);
        // if(songToDisplay!=null)
        //     {
        //         if(song_Info_Page.Show_SongInfo(songToDisplay))
        //     {
        //         SetCurrentPage(song_Info_Page.gameObject);
        //     }
        //         else  Debug.Log("Image not loaded.");
        //     }
        // else 
        //     {
        //         songToDisplay=GetSongByID(current_playing_song_id);
        //     }

    }

    public void OnLoadInfoComplete(string genresNames, string sortDescription)
    {
        Song songToDisplay = GetSongByID(playbar.currentDisplayedSongID);
        songToDisplay.genres=genresNames;
        songToDisplay.info = sortDescription;
        song_Info_Page.Show_SongInfo(songToDisplay);
    }

    public void GoTo_PlaylistPage(string id, bool callAPI=false)
    {
        if(callAPI)
            StartCoroutine(aPI_Call.Get_Playlists(API_Call.GET_PLAYLIST_OFUSER.Replace("*",userID)));
        SetCurrentPage(playlist_Page.gameObject);
        playlist_Page.Display_Playlist(GetPlaylistByID(id));
    }

    public void GoTo_PlaylistPage()
    {
        SetCurrentPage(playlist_Page.gameObject,"playlist");
        playlist_Page.Display_Playlist(currentPlayingPlaylist);
        Debug.Log("Display there songs: ");
        foreach(Song song in currentPlayingPlaylist.list_Song)
            Debug.Log(song.data.title);
    }

    public void GoTo_AllPlaylistPage(bool CallAPI, bool UpdateUI=true)
    {
        if(CallAPI)
            StartCoroutine(aPI_Call.Get_Playlists(API_Call.GET_PLAYLIST_OFUSER.Replace("*",userID)));
        
        SetCurrentPage(playlist_Page.gameObject,"all_user_playlists");
        if(UpdateUI)
            foreach(Playlist playlist in allPlaylistloaded)
                playlist_Page.Load_or_Update_Playlist(playlist);
        
        // isLoading=true;
        // SetCurrentPage(playlist_Page.gameObject);
        // StartCoroutine(DisplayAfterLoading(delegate(){}));
    }

    IEnumerator DisplayAfterLoading(UnityEngine.Events.UnityAction action){
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            if(!isLoading)
            {
                action.Invoke();
                break;
            }
        }
    }

    public void GoTo_HomePage(bool callAPI)
    {
        if(callAPI)
        {    
            StartCoroutine(aPI_Call.Get_Songs(API_Call.NEWSONG,true,"new"));
            StartCoroutine(aPI_Call.Get_Songs(API_Call.TRENDSONG,true,"trending"));
            StartCoroutine(aPI_Call.GetRequest(API_Call.GET_BANNER,"banner"));
        }
        
        SetCurrentPage(home_Page.gameObject,"home");
        //home_Page.RemoveAllDisplaySong();
        foreach(Song song in newSongs)
        {   
            home_Page.New_Songs.Load_Or_UpdateSong(song);
        }

        foreach(Song song in trendingSongs)
        {   
            home_Page.trendingSong.Load_Or_UpdateSong(song);
        }

        foreach(Song song in bannerAlbum)
        {   
            home_Page.HotAlbum.Load_Or_UpdateSong(song);
        }
    }

    public void GoTo_SearchPage(string query, bool callAPI)
    {
        if(query!="")
            {
            if(callAPI)
                StartCoroutine(aPI_Call.Get_Songs(API_Call.SEARCH_SONG.Replace("*",query),false,"searchPage"));
            
            searchSongResult.Clear();
            SetCurrentPage(searchPage.gameObject,"search");
            searchPage.ClearSearchResult();
            }
        //home_Page.RemoveAllDisplaySong();
    }

    public void GoTo_ListSongPage(string type, bool callAPI, string idListSong="")
    {
        if(type=="album")
        {
            if(callAPI)
                StartCoroutine(aPI_Call.GetRequest(API_Call.GET_BANNER_SONGS.Replace("*",idListSong),"banner_songs"));
            SetCurrentPage(listSongPage.gameObject,"list_song");
            listSongPage.DisplayListSong(null,"album",GetAlbumByID(idListSong));
        }
        else if (type=="recent_songs")
        {
            if(callAPI)
                StartCoroutine(aPI_Call.GetRequest(API_Call.GET_RECENT_SONGS.Replace("*",userID),"recent_songs"));
            SetCurrentPage(listSongPage.gameObject,"list_song");
            listSongPage.DisplayListSong(null,"recent_songs");
        }
        else if (type=="favorite_songs")
        {
            if(callAPI)
                StartCoroutine(aPI_Call.GetRequest(API_Call.GET_FAVORITE_SONGS.Replace("*",userID),"favorite_songs"));
            SetCurrentPage(listSongPage.gameObject,"list_song");
            listSongPage.DisplayListSong(null,"favorite_songs");
        }
        //home_Page.RemoveAllDisplaySong();
    }

    public void LoadPlaylist(Playlist playlist, bool CallAPI_GetSong=false)
    {
        TryAddPlaylist(playlist, allPlaylistloaded);
        if(CallAPI_GetSong)
        {
            string playlist_id = playlist.data.idPlaylist;
            StartCoroutine(aPI_Call.Get_Songs(API_Call.SONG_BYPLAYLIST.Replace("*",playlist_id),true,"playlist", playlist_id));
        }
    }

    // public void CreateNewPlaylist(Playlist playlist)
    // {
    //     LoadPlaylist(playlist);
    //     aPI_Call.Create_NewPlaylist(playlist.data.name, userID);
    // }

    public void CreateNewPlaylist(Playlist playlist, bool refreshPage=false, bool CallAPI_GetSong=false, List<string> ids_SongAddToPL=null)
    {
        if(waitCreatingNewPL)
            return;
        waitCreatingNewPL=true;
        TryAddPlaylist(playlist,allPlaylistloaded);
        StartCoroutine(aPI_Call.Create_NewPlaylist(playlist.data.name,userID));
        if(refreshPage)
            GoTo_AllPlaylistPage(false);
        if(ids_SongAddToPL!=null)
        {
            SongIDAddToNewCreatedPlaylist=ids_SongAddToPL;
        }
        // if(CallAPI_GetSong)
        // {
        //     string playlist_id = playlist.data.idPlaylist;
        //     aPI_Call.Get_Songs(API_Call.SONG_BYPLAYLIST.Replace("*",playlist_id),true,"playlist", playlist_id);
        // }
    }

    private static bool waitCreatingNewPL=false;
    private static List<string> SongIDAddToNewCreatedPlaylist;
    public void Get_Created_PlaylistData(Playlist_Data newPlaylist_Data){
        if(waitCreatingNewPL)
            foreach(Playlist playlist in allPlaylistloaded)
                {
                    if(playlist.data.idPlaylist==null)
                        {
                            playlist.data= newPlaylist_Data;
                            waitCreatingNewPL=false;
                            if(SongIDAddToNewCreatedPlaylist!=null)
                                {
                                    foreach(string SongID in SongIDAddToNewCreatedPlaylist)
                                        AddSong_ToPlaylist(SongID, newPlaylist_Data.idPlaylist, true);
                                    SongIDAddToNewCreatedPlaylist.Clear();
                                }
                            GoTo_AllPlaylistPage(false);
                        }
                }
    }

    public void AddSong_ToPlaylist(string song_id, string playlist_id, bool CallAPI)
    {
        if(GetPlaylistByID(playlist_id).TryAddSong(GetSongByID(song_id))==true)
            {
                UpdateUI();
                if(currentPlayingPlaylist.data.idPlaylist==playlist_id)
                    playlistbar.UpdatePlaylist();
            } 
        
        if(CallAPI)
            StartCoroutine(API_Call.PostRequest(API_Call.ADD_SONG_TOPLAYLIST.Replace("**",song_id).Replace("*",playlist_id)));
        Debug.Log("Add song "+song_id+" to playlist "+playlist_id);
    }

    // public void AddSong_ToPlaylist(Song song, string playlist_id, bool CallAPI=false)
    // {
    //     if(GetPlaylistByID(playlist_id).TryAddSong(song)==true)
    //         UpdateUI();
    //     if(CallAPI)
    //         StartCoroutine(aPI_Call.AddSong_ToPlaylist(song.data.id, playlist_id));
    //     Debug.Log("Add song "+song.data.id+" to playlist "+playlist_id);
    // }

    public void RemoveSongs_FromPlaylist(List<string> song_ids, string playlist_id, bool callAPI=true)
    {
        foreach(string song_id in song_ids)
            {
                GetPlaylistByID(playlist_id).RemoveSong(song_id);
                Debug.Log("Remove song "+song_id+" from playlist "+playlist_id);
                // StartCoroutine(API_Call.DeleteRequest(API_Call.REMOVE_SONG_FROMPLAYLIST.Replace("**",song_id)
                //     .Replace("*",playlist_id)));
            }
        if(playlist_id!="0")
            StartCoroutine(API_Call.RemoveSongsFromPlaylist(song_ids,playlist_id));    
        GoTo_PlaylistPage(playlist_id);
        if(currentPlayingPlaylist.data.idPlaylist==playlist_id)
            {
                playlistbar.UpdatePlaylist();
            }
    }

    public void DeletePlaylist(string playlist_id, bool callAPI=true)
    {
        if(callAPI)
            StartCoroutine(API_Call.DeleteRequest(API_Call.REMOVE_PLAYLIST.Replace("**",userID).Replace("*",playlist_id)));
        allPlaylistloaded.Remove(GetPlaylistByID(playlist_id));
        GoTo_AllPlaylistPage(false);
    }

    private void SetCurrentPage(GameObject pageToSet, string display_UI="")
    {
        if(currentPage!=pageToSet) //Already set
            {                   
            currentPage.SetActive(false);
            currentPage=pageToSet;
            }
        

        pageToSet.SetActive(true);
        current_display_UI=display_UI;
        if(current_display_UI=="all_user_playlists")
            playlist_Page.Display_AllPlaylist(false);
        // else if (current_display_UI=="list_song")
        //     listSongPage.DisplayListSong()
    }

    public void AddSong_And_PlayTempPlaylist(List<string> song_ids)
    {
        //temp_playlist.list_Song=null;
        Debug.Log("Play there songs:");
        foreach(string song_id in song_ids){
            Debug.Log(song_id);
            Song song = GetSongByID(song_id);
            if(song!=null)
                { 
                    temp_playlist.TryAddSong(song);
                    StartCoroutine(song.Fill_AudioClip());
                }
            else Debug.Log("Null");
        }
        SetCurrentPlaylist(temp_playlist.data.idPlaylist);
        if(song_ids.Count>0)
            Play_Current_Playlist(song_ids[0]);
    }

    // Start is called before the first frame update
    void Start()
    {
        currentPage=playlist_Page.gameObject;
        GoTo_HomePage(false);
        temp_playlist.data.name="Temporary playlist";
        temp_playlist.data.idPlaylist="0";
        currentPlayingPlaylist=temp_playlist;
        playlistbar.currentPlaylist= currentPlayingPlaylist;

        alert_text_panel_parent = alert_text.transform.parent.parent.gameObject;
        StartCoroutine(LoadData());
    }

    IEnumerator LoadData()
    {
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(aPI_Call.GetRequest(API_Call.GET_FAVORITE_SONGS.Replace("*",userID),"favorite_songs"));
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(aPI_Call.GetRequest(API_Call.GET_BANNER,"banner"));
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(aPI_Call.Get_Songs(API_Call.TRENDSONG,true,"trending"));
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(aPI_Call.Get_Songs(API_Call.NEWSONG,true,"new"));
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(aPI_Call.Get_Playlists(API_Call.GET_PLAYLIST_OFUSER.Replace("*",userID)));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
