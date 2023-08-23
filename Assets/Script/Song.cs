// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

using UnityEngine;
using UnityEngine.Networking;

public class Song // : MonoBehaviour
{
    //public static UnityWebRequest AudioDownload;
    public Song_Data data;
    public string createDate="";
    public string lyrics="";
    public string genres="";
    public string info="";
    public string audioSourceLink="";
    public Texture2D img;
    public AudioClip audioclip;
    public bool AudioLoaded=false;
    private UnityWebRequest AudioDownload;

    public string GetDate(bool getMonth=false, bool getDay=false) {
    System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
    dateTime = dateTime.AddSeconds(data.releaseDate);
    if(!getMonth)
        return dateTime.Year.ToString();
    else if(!getDay)
        return dateTime.Month+"/"+dateTime.Year;
    else return dateTime.ToShortDateString();
    }

    public float Get_AudioDownload_Progress()
    {
        if(AudioDownload==null)
            return 0;
        // else if(AudioDownload.downloadProgress==null)
        //     return 0;
        else return AudioDownload.downloadProgress;

        //return AudioDownload.downloadProgress;
    }

    public System.Collections.IEnumerator Fill_AudioClip()//AudioSource myAudioSource)
    {
        if(AudioLoaded)
            yield break;
        if(audioSourceLink=="")
            yield break;
        AudioLoaded=true;
        Debug.Log("Start getting audio clip for song "+data.id);
        Debug.Log(audioSourceLink);
        using (AudioDownload = UnityWebRequestMultimedia.GetAudioClip(audioSourceLink, AudioType.MPEG))
        {
            yield return AudioDownload.SendWebRequest();

            if (AudioDownload.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Error while getting audio clip for song "+data.id);//
                Debug.Log(AudioDownload.error);
                AudioLoaded=false;
            }
            else
            {
                this.audioclip = DownloadHandlerAudioClip.GetContent(AudioDownload);
                // byte[] receivedBytes = AudioDownload.downloadHandler.data;
                // float[] samples = new float[receivedBytes.Length / 4]; //size of a float is 4 bytes

                // //System.Buffer.BlockCopy(receivedBytes, 0, samples, 0, receivedBytes.Length);
                
                // int channels = 1; //Assuming audio is mono because microphone input usually is
                // int sampleRate = 44100; //Assuming your samplerate is 44100 or change to 48000 or whatever is appropriate
                
                // AudioClip clip = AudioClip.Create("ClipName", samples.Length, channels, sampleRate, false);
                // clip.SetData(samples, 0);

                // this.audioclip = clip;

                this.data.duration = audioclip.length;
                Debug.Log(Time.time+": load audio clip to audio source complete");
            }
        }
    }

    //  private float[] ConvertByteToFloat(byte[] array) 
    //         {
    //             float[] floatArr = new float[array.Length / 4];
    //             for (int i = 0; i < floatArr.Length; i++) 
    //             {
    //                 if (System.BitConverter.IsLittleEndian)
    //                 System.Array.Reverse(array, i * 4, 4);
    //                 floatArr[i] = System.BitConverter.ToSingle(array, i * 4) / 0x80000000;
    //             }
    //             return floatArr;
    //         } 

    // public System.Collections.IEnumerator FillPartOfSong()
    // {
    //     AudioClip audioClip;
    //     float[] samples;
    //     yield return new WaitForSeconds(1f);
    //     Debug.Log("Start try load part of audio");
    //     while(!AudioDownload.isDone)
    //     {
    //         yield return new WaitForSeconds(1f);
    //         if(AudioDownload.isDone)
    //             break;
    //         else{
    //         Debug.Log("Try load part of audio");
    //         byte[] receivedBytes = AudioDownload.downloadHandler.data;
    //         samples = new float[receivedBytes.Length / 4];
    //         Debug.Log(samples);
    //         //System.Buffer.BlockCopy(receivedBytes, 0, samples, 0, receivedBytes.Length);
    //         audioClip = AudioClip.Create("testSound", samples.Length, 1, 44100, false);
    //         audioClip.SetData(samples, 0);
    //         this.audioclip = audioClip;
    //         this.data.duration = audioclip.length;
    //         Debug.Log(audioClip.length);
    //         }
    //     }
    // }

    public System.Collections.IEnumerator FillSong_Img()//UnityEngine.UI.Image myImage)
    {
        if(this.img!=null)
            yield break;
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(data.thumbnail);
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.ConnectionError) 
            Debug.Log(request.error);
        else
        {
            this.img = ((DownloadHandlerTexture) request.downloadHandler).texture;
            //myImage.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            Debug.Log(Time.time+": load img complete");
        }
    }

    // public string Name { get => name; set => name = value; }
    // public string Img_big_path { get => img_big_path; set => img_big_path = value; }
    // public string Img_mini { get => img_mini; set => img_mini = value; }
    // public string Artist { get => artist; set => artist = value; }
    // public float Length { get => length; set => length = value; }
    // public string Genre { get => genre; set => genre = value; }
    // public string Info { get => info; set => info = value; }
    // public string Audio_path { get => audio_path; set => audio_path = value; }

    public static Song CreateFromJSON(string jsonString)
    {
        Song s;
        s = JsonUtility.FromJson<Song>(jsonString);
        //s.Fill_AudioClip();
        //s.FillSong_Img();
        return s;
    }

    public static Song CreateFrom_APIData(Song_Data song_Data)
    {
        Song song = new Song();
        song.data = song_Data;
        return song;
    }

    public static string ToJSON(Song song)
    {
        string s="";
        JsonUtility.ToJson(song);
        return s;
    }

    public string GetLength()
    {
        int sec = ((int)this.data.duration);
        int min = sec/60;
        int hour = min/60;
        min = min%(60*60);
        sec = sec%60;
        string s="";
        if(hour!=0)
        {
            if(hour<10)
                s= "0"+hour.ToString()+":";
            else s= hour.ToString()+":";
        }
        if(min<10)
            s+= "0"+min.ToString()+":";
        else s+= min.ToString()+":";
        if(sec<10)
            s+= "0"+sec.ToString();
        else s+= sec.ToString();
        return s;

    }

}
