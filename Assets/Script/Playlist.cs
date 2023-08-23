using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Playlist //: MonoBehaviour
{
    public Playlist_Data data;
    public List<Song> list_Song = new List<Song>();
    public Texture2D image;

    public static Playlist CreateFromJSON(string jsonString)
    {
        Playlist p;
        p = JsonUtility.FromJson<Playlist>(jsonString);
        return p;
    }

    public static Playlist CreateFrom_APIData(Playlist_Data data)
    {
        Playlist p = new Playlist();
        p.data = data;
        return p;
    }

    public System.Collections.IEnumerator FillPlaylist_Img()//UnityEngine.UI.Image myImage)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(data.thumbnail);
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.ConnectionError) 
            Debug.Log(request.error);
        else
        {
            this.image = ((DownloadHandlerTexture) request.downloadHandler).texture;
            //myImage.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            Debug.Log(Time.time+": load playlist img complete");
        }
    }

    public int GetNumOfSong()
    {
        return list_Song.Count;
    }

    public List<Song> GetListSong()
    {
        return list_Song;
    }

    public Song GetFirstSong()
    {
        return list_Song[0];
    }

    public Song GetSong(string id)
    {
        foreach(Song s in list_Song)
        {if(s.data.id==id) return s;}
        return null;
    }

    public void AddSong(Song song)
    {
        list_Song.Add(song);
    }

    public bool TryAddSong(Song songToAdd, bool overwrite=true)
    {
        foreach(Song song in list_Song)
            if(song.data.id==songToAdd.data.id)
            {
                if(overwrite)
                    if(!song.data.Equals(songToAdd))
                        {
                            song.data=songToAdd.data; //overwrite
                            Debug.Log("Data changed, update UI");
                            return true;
                        }

                return false;
            }
        
        list_Song.Add(songToAdd);
        Debug.Log("Data changed, update UI");

        return true;
    }

    public bool RemoveSong(string songID)
    {
        foreach(Song s in list_Song)
        {if(s.data.id==songID) 
            {
            list_Song.Remove(s);
            return true;
            }
        }

        return false;
    }
}
