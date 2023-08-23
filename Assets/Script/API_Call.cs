using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class API_Call : MonoBehaviour
{   
    public const string DOMAIN1= "https://musicappapi-production-95cf.up.railway.app";
    public const string REMOVE_FROM_FAVORITE_SONG = DOMAIN1 + "/pmdv/db/song/delete/song_from_favorite_song?idSong=**&idUser=*";
    public const string ADD_TO_FAVORITE_SONG = DOMAIN1 + "/pmdv/db/song/add/song_to_favorite_song?idSong=**&idUser=*";
    public const string ADD_TO_LISTEN_SONG = DOMAIN1 + "/pmdv/db/song/add/song_to_listen_song?idSong=**&idUser=*";
    public const string GET_RECENT_SONGS = DOMAIN1 + "/pmdv/db/song/get/listen_song_by_user?idUser=*";
    public const string GET_FAVORITE_SONGS = DOMAIN1 + "/pmdv/db/song/get/favorite_song_by_user?idUser=*";
    public const string GET_BANNER_SONGS = DOMAIN1 + "/pmdv/src/get/songs_by_playlist_on?id=*";
    public const string GET_BANNER = DOMAIN1 + "/pmdv/src/get/info/banner";
    public const string GET_SONG_INFO = DOMAIN1 + "/pmdv/src/get/info/song?id=*";
    public const string GET_SONG_LYRICS = DOMAIN1 + "/pmdv/src/get/info/source/lyric?id=*";
    public const string GET_GENRE_NAME = DOMAIN1 + "/pmdv/src/get/info/genre?id=*";
    public const string REMOVE_SONG_FROMPLAYLIST = DOMAIN1 + "/pmdv/db/playlist/delete/song_from_playlist?idSong=**&idPlaylist=*";
    public const string REMOVE_SONGS_FROMPLAYLIST = DOMAIN1 + "/pmdv/db/playlist/delete/songs_from_playlist?idPlaylist=*";
    public const string REMOVE_PLAYLIST = DOMAIN1 + "/pmdv/db/playlist/delete/user_from_playlist?idUser=**&idPlaylist=*";
    public const string GET_PLAYLIST_OFUSER = DOMAIN1 + "/pmdv/db/playlist/get/playlist_by_user?idUser=*";
    public const string SONG_BYPLAYLIST = DOMAIN1 + "/pmdv/db/song/get/songs_by_playlist?idPlaylist=*";
    public const string ADD_SONG_TOPLAYLIST = DOMAIN1 + "/pmdv/db/playlist/add/song_to_playlist?idSong=**&idPlaylist=*";
    public const string NEWSONG=DOMAIN1 + "/pmdv/src/get/song/new-release";
    public const string TRENDSONG=DOMAIN1 + "/pmdv/src/get/songs/db";
    public const string CREATE_NEW_PLAYLIST=DOMAIN1 + "/pmdv/db/playlist/add/user_to_playlist?idUser=*";
    public const string SET_PLAYLIST_FORUSER=DOMAIN1 + "/pmdv/db/playlist/add/user_to_playlist?idUser=*&idPlaylist=**";
    public const string CREATE_NEW_PLAYLIST_FORUSER=DOMAIN1 + "/pmdv/db/playlist/add/user_to_playlist?idUser=*";
    public const string GET_SOURCESONG=DOMAIN1 + "/pmdv/src/get/streaming/song?id=*";
    public const string SEARCH_SONG=DOMAIN1 + "/pmdv/src/search/multi/song?query=*";
    public const string SEARCH_ARTIST=DOMAIN1 + "/pmdv/src/search/multi/artist?query=*";
    public music_flow music_Flow;
    //Song song = null;

    public static IEnumerator DeleteRequest(string path)
    { 
        Debug.Log("Delete Request:"+path);
        using (UnityWebRequest www = UnityWebRequest.Delete(path))
        {
            // byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(raw);
            // www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            // www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            //www.SetRequestHeader("accept", "application/json; charset=UTF-8");
            www.SetRequestHeader("content-type","application/json; charset=UTF-8");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                //www.uploadHandler.Dispose();
                Debug.Log("API: Delete request complete!");
                //string json = www.downloadHandler.text;
                Debug.Log(www.result);
            }
        }
    }

    public static IEnumerator RemoveSongsFromPlaylist(List<string> song_ids, string playlist_id)
    { 
        string raw = "[";
        foreach(string song_id in song_ids)
            raw += "\""+song_id+"\""+",";
        
        //Debug.Log(raw);
        if(raw.Length>1)
            raw=raw.Remove(raw.Length-1,1);
        raw +="]";
        
        string path = REMOVE_SONGS_FROMPLAYLIST.Replace("*",playlist_id);
        Debug.Log(path);
        Debug.Log(raw);
        using (UnityWebRequest www = UnityWebRequest.Post(path,raw))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(raw);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            // www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            //www.SetRequestHeader("accept", "application/json; charset=UTF-8");
            www.SetRequestHeader("content-type","application/json; charset=UTF-8");
            yield return www.SendWebRequest();
            string json = www.downloadHandler.text;
            Debug.Log(json);

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                www.uploadHandler.Dispose();
                Debug.Log("Remove songs from playlist complete!");
                //string json = www.downloadHandler.text;
                Debug.Log(www.result);
            }
        }
    }

    public IEnumerator GetRequest(string path, string type)
    {
        yield return new WaitForSeconds(0.1f);
        UnityWebRequest request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();
        string json = "";
        json = request.downloadHandler.text;
        Debug.Log(json);
        if (request.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(request.error);
        else
        {
        
        //Debug.Log(general_Request.data.name);
        //Debug.Log(general_Request.data.songs[0].title);
            switch(type){
                case "lyrics":
                {
                    General_Request general_Request = JsonUtility.FromJson<General_Request>(json);
                    music_Flow.song_Info_Page.LoadLyrics(general_Request.data.content);
                } break;
                case "song_info":
                {
                    General_Request general_Request = JsonUtility.FromJson<General_Request>(json);
                    music_Flow.OnLoadInfoComplete(general_Request.data.genresNames, general_Request.data.album.sortDescription);
                } break;
                case "banner":
                {
                    Albums_Data_Request albums_Data_Request = JsonUtility.FromJson<Albums_Data_Request>(json);
                    foreach(Album_Data album_Data in albums_Data_Request.data) 
                    {
                        Song album = new Song();
                        album.data.id = album_Data.encodeId;
                        album.data.title = album_Data.sortTitle;
                        album.data.thumbnail = album_Data.thumbnail; // Convert from Song to Album, to Display on Banner
                        album.createDate = album_Data.dateCreate;
                        music_Flow.LoadAlbum(album,true);
                    }
                } break;
                case "banner_songs":
                {
                    General_Request general_Request = JsonUtility.FromJson<General_Request>(json);
                    foreach(Song_Data song_Data in general_Request.data.items)
                        {
                            Song song = Song.CreateFrom_APIData(song_Data);
                            music_Flow.LoadSong(song,true,"banner_songs");
                        }
                } break;
                case "recent_songs":
                {
                    Songs_Data_Request songs_Data_Request = JsonUtility.FromJson<Songs_Data_Request>(json);
                    foreach(Song_Data song_Data in songs_Data_Request.data)
                        {
                            Song song = Song.CreateFrom_APIData(song_Data);
                            music_Flow.LoadSong(song,true,"recent_songs");
                        }
                } break;
                case "favorite_songs":
                {
                    Songs_Data_Request songs_Data_Request = JsonUtility.FromJson<Songs_Data_Request>(json);
                    foreach(Song_Data song_Data in songs_Data_Request.data)
                        {
                            Song song = Song.CreateFrom_APIData(song_Data);
                            music_Flow.LoadSong(song,true,"favorite_songs");
                        }
                } break;
                default:
                    Debug.Log("UI Name not match");
                break;
            }
        }
        
        // // if (song != null)
        // //     playlist.AddSong(song);

        // Debug.Log(playlist.data.name);

        // music_Flow.LoadPlaylist(playlist, false);

        // string song_img_path = "C:\\IT\\Test\\1522404477634_640.jpg";
        // Texture2D t = new Texture2D(1,1);
        // StartCoroutine(GetSongImg(song_img_path,t));

    }
    
    public IEnumerator Create_NewPlaylist(string playlist_name, string userID)
    {
        string raw = "{\"name\":\""+playlist_name+"\"}"; //, \"dateCreate\":\""+System.DateTime.Now.ToShortDateString()+"\"}";
        Debug.Log(raw);
        using (UnityWebRequest www = UnityWebRequest.Post(CREATE_NEW_PLAYLIST.Replace("*",userID), raw))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(raw);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            // www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            //www.SetRequestHeader("accept", "application/json; charset=UTF-8");
            www.SetRequestHeader("content-type","application/json; charset=UTF-8");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                www.uploadHandler.Dispose();
                Debug.Log("Add playlist complete!");
                string json = www.downloadHandler.text;
                Debug.Log(json);
                Playlist_Data_Request playlist_Data_Request = JsonUtility.FromJson<Playlist_Data_Request>(json);
                Debug.Log(playlist_Data_Request.data.idPlaylist);
                music_Flow.Get_Created_PlaylistData(playlist_Data_Request.data);

                // foreach(Playlist_Data playlist_Data in playlist_Data_Request.data)
                // {   
                //     Playlist playlist = Playlist.CreateFrom_APIData(playlist_Data);
                //     music_Flow.LoadPlaylist(playlist,false,true);
                // }

                //StartCoroutine(Set_User_For_Playlist(playlist_Data_Request.data.idPlaylist,userID));
            }
        }
    }

    public static IEnumerator PostRequest(string path)
    {
        yield return new WaitForSeconds(0.1f);
        using (UnityWebRequest www = UnityWebRequest.Post(path,""))
        {
            // byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(raw);
            // www.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            // www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            //www.SetRequestHeader("accept", "application/json; charset=UTF-8");
            www.SetRequestHeader("content-type","application/json; charset=UTF-8");
            yield return www.SendWebRequest();
            string json = www.downloadHandler.text;
            Debug.Log(json);
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                //www.uploadHandler.Dispose();
                Debug.Log("API: Post Request Complete!");
                

                //StartCoroutine(Set_User_For_Playlist(playlist_Data_Request.data.idPlaylist,userID));
            }
        }
    }

    public IEnumerator GetSourceSong(string SongID, Song song)
    {
        
        if(song.audioSourceLink!="")
            yield break;
        yield return new WaitForSeconds(0.1f);

        string path = GET_SOURCESONG.Replace("*",SongID);
        
        UnityWebRequest request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();
        string json = "";
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(path);
            Debug.Log(request.error);
        }
        else
        {
            json = request.downloadHandler.text;
            Debug.Log(path);
            Debug.Log(json);
        }
        SourceSong_Data_Request sourceSong_Data_Request = JsonUtility.FromJson<SourceSong_Data_Request>(json);
        song.audioSourceLink=sourceSong_Data_Request.data.uri128;
        if(music_Flow.playbar.currentDisplayedSongID==song.data.id && song.audioclip==null)
            music_Flow.PlaySong(song.data.id);


        // string song_img_path = "C:\\IT\\Test\\1522404477634_640.jpg";
        // Texture2D t = new Texture2D(1,1);
        // StartCoroutine(GetSongImg(song_img_path,t));
    }

    public IEnumerator Get_A_Song(string path)
    {
        yield return new WaitForSeconds(0.1f);
        UnityWebRequest request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();
        string json = "";
        if (request.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(request.error);
        else
        {
            json = request.downloadHandler.text;
            Debug.Log(json);
        }
        Song song = Song.CreateFromJSON(json);

        Debug.Log(song.data.artistsNames);
        Debug.Log(song.data.title);

        music_Flow.LoadSong(song,true,"");

        // string song_img_path = "C:\\IT\\Test\\1522404477634_640.jpg";
        // Texture2D t = new Texture2D(1,1);
        // StartCoroutine(GetSongImg(song_img_path,t));
    }

    public IEnumerator Get_Songs(string path, bool isDataTypeList, string loadTo ,string of_playlist_id = null)
    {
         yield return new WaitForSeconds(0.1f);
        Debug.Log(path);
        UnityWebRequest request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();
        string json = "";
        json = request.downloadHandler.text;
        Debug.Log(json);

        if (request.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(request.error);
        else
        {   
            List<Song> list_song = new List<Song>();
            if(isDataTypeList)
                {
                    Songs_Data_Request request_Data = JsonUtility.FromJson<Songs_Data_Request>(json);
                    foreach (Song_Data song_Data in request_Data.data)
                    {
                        list_song.Add(Song.CreateFrom_APIData(song_Data));
                        Debug.Log("Getsongs from ListData: "+song_Data.title);
                    }
                }
            else
                {
                    General_Request request_Data = JsonUtility.FromJson<General_Request>(json);
                    foreach(Song_Data song_Data in request_Data.data.songs)
                    {
                        list_song.Add(Song.CreateFrom_APIData(song_Data));
                        Debug.Log("Getsongs from field [songs]: "+song_Data.title);
                    }
                }
            {
                // Debug.Log(request_Data.status);
                // Debug.Log(request_Data.message);
                // Debug.Log(request_Data.data);

                if (loadTo=="playlist")
                {
                    foreach (Song song in list_song)
                        {music_Flow.LoadSong(song,true,"");
                        music_Flow.AddSong_ToPlaylist(song.data.id, of_playlist_id,false);}
                }
                else if(loadTo=="trending")
                {
                    foreach (Song song in list_song)
                        music_Flow.LoadSong(song,false,"trending");
                }
                else if(loadTo=="new")
                {
                    foreach (Song song in list_song)
                        music_Flow.LoadSong(song,false,"new");
                }
                else if (loadTo=="searchPage")
                {
                    foreach (Song song in list_song)
                        music_Flow.LoadSong(song,false,"search_page");
                }
                else
                    Debug.Log("######---  No place to load into");
            }
        }

        // string song_img_path = "C:\\IT\\Test\\1522404477634_640.jpg";
        // Texture2D t = new Texture2D(1,1);
        // StartCoroutine(GetSongImg(song_img_path,t));
    }

    public IEnumerator Get_Playlists(string path)
    {
         yield return new WaitForSeconds(0.1f);
        UnityWebRequest request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();
        string json = "";
        json = request.downloadHandler.text;
            Debug.Log(json);
        if (request.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(request.error);
        else
        {
        Playlists_Data_Request request_Data = JsonUtility.FromJson<Playlists_Data_Request>(json);
        {
            Debug.Log(request_Data.status+"_"+request_Data.message+"_"+request_Data.data);

            List<Playlist> playlists = new List<Playlist>();
            foreach (Playlist_Data data in request_Data.data)
            {
                playlists.Add(Playlist.CreateFrom_APIData(data));
                Debug.Log(data.name);
            }

            foreach (Playlist playlist in playlists)
                music_Flow.LoadPlaylist(playlist, true);
        }
        }

        // string song_img_path = "C:\\IT\\Test\\1522404477634_640.jpg";
        // Texture2D t = new Texture2D(1,1);
        // StartCoroutine(GetSongImg(song_img_path,t));
    }

    IEnumerator Get_A_Playlist(string path)
    {
        yield return new WaitForSeconds(0.1f);
        UnityWebRequest request = UnityWebRequest.Get(path);
        yield return request.SendWebRequest();
        string json = "";
        if (request.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log(request.error);
        else
        {
            json = request.downloadHandler.text;
            Debug.Log(json);
        }
        Playlist playlist = Playlist.CreateFromJSON(json);

        Debug.Log(playlist.data.name);

        music_Flow.LoadPlaylist(playlist, false);

        // string song_img_path = "C:\\IT\\Test\\1522404477634_640.jpg";
        // Texture2D t = new Texture2D(1,1);
        // StartCoroutine(GetSongImg(song_img_path,t));
    }


    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(Get_Songs(SUGGESTSONG));
        

        // StartCoroutine(Get_Songs("C:\\IT\\Test\\list_song1.json"));
        // StartCoroutine(Get_Playlists("C:\\IT\\Test\\list_playlist.json"));

        //GetPlayList(); 
    }

    void GetSong()
    {
        StartCoroutine(Get_A_Song("C:\\IT\\Test\\song1.json"));
        StartCoroutine(Get_A_Song("C:\\IT\\Test\\song6.json"));
        StartCoroutine(Get_A_Song("C:\\IT\\Test\\song3.json"));
        StartCoroutine(Get_A_Song("C:\\IT\\Test\\song4.json"));
        StartCoroutine(Get_A_Song("C:\\IT\\Test\\song5.json"));

    }

    public void GetPlayList()
    {
        StartCoroutine(Get_A_Playlist("C:\\IT\\Test\\pl1.json"));
        StartCoroutine(Get_A_Playlist("C:\\IT\\Test\\pl2.json"));
        StartCoroutine(Get_A_Playlist("C:\\IT\\Test\\pl3.json"));
        StartCoroutine(Get_A_Playlist("C:\\IT\\Test\\pl4.json"));
        StartCoroutine(Get_A_Playlist("C:\\IT\\Test\\pl5.json"));
    }

    // Update is called once per frame
    void Update()
    {

    }
}


    //{
    // public IEnumerator Create_NewPlaylist(string playlist_name, string userID)
    // {
    //     string raw = "{\"name\":\""+playlist_name+"\",\"dateCreate\":\""+System.DateTime.Now.ToShortDateString()+"\"}";
    //      string raw = "{\"name\":\""+playlist_name+"\",\"dateCreate\":\""+System.DateTime.Now.ToShortDateString()+"\"}";
    //      var uwr = new UnityWebRequest(CREATE_NEW_PLAYLIST, "POST");
    //      byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(raw);
    //      uwr.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
    //      uwr.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
    //      uwr.SetRequestHeader("Content-Type", "application/json");

    // //Send the request then wait here until it returns
    //      yield return uwr.SendWebRequest();

    //     // if (uwr.result == UnityWebRequest.Result.ConnectionError)
    //     // {
    //     //     Debug.Log("Error While Sending: " + uwr.error);
    //     // }
    //     // else
    //     // {
    //     //     Debug.Log("Received: " + uwr.downloadHandler.text);
    //     // }
    // }

    

    // IEnumerator Set_User_For_Playlist(string playlistID, string userID)
    // {
    //     string uri = SET_PLAYLIST_FORUSER.Replace("**",playlistID).Replace("*",userID);

    //     using (UnityWebRequest www = UnityWebRequest.Post(uri,""))
    //     {
    //         Debug.Log(uri);
    //         yield return www.SendWebRequest();

    //         if (www.result != UnityWebRequest.Result.Success)
    //         {
    //             Debug.Log(www.error);
    //         }
    //         else
    //         {
    //             Debug.Log("Set user for playlist complete!");
    //         }
    //     }
    // }

    // public IEnumerator Get_ListSong(string path)
    // {
    //     UnityWebRequest request = UnityWebRequest.Get(path);
    //     yield return request.SendWebRequest();
    //     string json = "";
    //     if(request.result == UnityWebRequest.Result.ConnectionError) 
    //         Debug.Log(request.error);
    //     else
    //     {
    //         json = request.downloadHandler.text;
    //         Debug.Log(json);
    //     }
    //     API_Request aPI_Request = API_Request.CreateFromJSON(json);
    //     if(aPI_Request.status!=200)
    //     {
    //         Debug.Log(aPI_Request.message); 
    //     }
    //     else{
    //         Debug.Log(aPI_Request.status);
    //         Debug.Log(aPI_Request.message);
    //         Debug.Log(aPI_Request.data_string[0]);

    //         List<Song> list_song = new List<Song>();
    //         foreach(string s in aPI_Request.data_string)
    //         {
    //             Debug.Log(s);
    //             Song song = JsonUtility.FromJson<Song>(s);
    //             list_song.Add(song);
    //         }

    //         Debug.Log(list_song[0].artistsNames);
    //         Debug.Log(list_song[1].artistsNames);
    //         Debug.Log(list_song[2].artistsNames);
    //         Debug.Log(list_song[3].artistsNames);

    //         foreach(Song song in list_song)
    //              music_Flow.AddSong(song);
    //     }

    //     // string song_img_path = "C:\\IT\\Test\\1522404477634_640.jpg";
    //     // Texture2D t = new Texture2D(1,1);
    //     // StartCoroutine(GetSongImg(song_img_path,t));
    // }
    //}