using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Song_Data
{
    public string id;
    public string title;
    public string thumbnail;
    public string thumbnailM;
    public string artistsNames;
    public float duration;
    public long releaseDate;
    
    // public string genre;
    // public string file;
    // public string lyrics;
    // public string chart;
    // public string info;
}
 
[System.Serializable]
public struct Playlist_Data
{
    public string idPlaylist;
    public string name;
    public string dateCreate;
    public string thumbnail;
}
 
[System.Serializable]
public struct SourceSong_Data_Request
{
    public int status;
    public string message;
    public SourceSong_Data data;
}


[System.Serializable]
public struct SourceSong_Data
{
    public string uri128;
    public string uri320;
    public string uriLossless;
}

[System.Serializable]
public struct Songs_Data_Request
{
    public int status;
    public string message;
    public List<Song_Data> data;
}

public struct Song_Data_Request
{
    public int status;
    public string message;
    public Song_Data data;
}

[System.Serializable]
public struct Playlists_Data_Request
{
    public int status;
    public string message;
    public List<Playlist_Data> data;
}

[System.Serializable]
public struct Playlist_Data_Request
{
    public int status;
    public string message;
    public Playlist_Data data;
}

[System.Serializable]
public struct General_Request
{
    public int status;
    public string message;
    public AllData data;
}

[System.Serializable]
public struct AllData
{
    public List<Song_Data> songs;
    public string name;
    public string username;
    public string idUser;
    public string content;
    public string genresNames;
    public Album_Data album;
    public List<Song_Data> items;
    public Album_Data playlistOn;
}

[System.Serializable]
public struct Album_Data
{
    public string sortDescription;
    public string thumbnail;
    public string sortTitle;
    public string dateCreate;
    public string encodeId;
}

[System.Serializable]
public struct Albums_Data_Request
{
    public int status;
    public string message;
    public List<Album_Data> data;
}

public class Request : MonoBehaviour
{   
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

// public static Songs_Data_Request CreateSongFromJSON(string jsonString)
    // {
    //     Songs_Data_Request request_Data = new Songs_Data_Request();
    //     request_Data = JsonUtility.FromJson<Songs_Data_Request>(jsonString);
    //     Debug.Log(request_Data.data);
    //     //s.Fill_AudioClip();
    //     //s.FillSong_Img();
    //     return request_Data;
    // }

    // public static Playlists_Data_Request CreatePlaylistFromJSON(string jsonString, bool list)
    // {
    //     Playlists_Data_Request request_Data = new Playlists_Data_Request();
    //     request_Data = JsonUtility.FromJson<Playlists_Data_Request>(jsonString);
    //     Debug.Log(request_Data.data);
    //     Debug.Log(request_Data.data.Capacity);
    //     //s.Fill_AudioClip();
    //     //s.FillSong_Img();
    //     return request_Data;
    // }