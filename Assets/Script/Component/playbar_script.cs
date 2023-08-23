using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class playbar_script : MonoBehaviour
{
    float timer1 = 0; //Testtttttttttttttttttttttt
    float music_Len = 300;
    bool isPlaying = false;
    public Transform time_pointer_trf;
    public RectTransform time_bar_trf;
    public Transform time_bar_trf_start;
    public Transform time_bar_trf_end;
    public RectTransform color_diff;
    public GameObject holding_color;
    //public GameObject text;
    // public GameObject current_time_text;
    // public GameObject len_text;
    //private TMPro.TextMeshProUGUI time_text;
    public TMPro.TextMeshProUGUI current_time_text;
    public TMPro.TextMeshProUGUI len_text;
    public TMPro.TextMeshProUGUI song_name_text;
    public TMPro.TextMeshProUGUI artist_name_text;
    // private const float time_pointer_max_x = 1450; //0 = min, 1920-224*2-x=max
    // private const float mouse_pos_x_delta =230;
    public GameObject play_pause_btn;
    Image play_pause_btn_img;
    public Image song_img;
    public Image speaker_icon;
    public Sprite play_btn_spr;
    public Sprite pause_btn_spr;
    public Sprite mute_icon;
    public Sprite low_sound_icon;
    public Sprite med_sound_icon;
    public Sprite high_sound_icon;
    public Scrollbar volume_bar;
    private float not0_volume;
    public playlistbar_script playlistBar;
    public string currentPlayedSongID="";
    public music_flow music_Flow;

    string audio_path;
    string song_img_path;
    AudioClip myClip;
    AudioSource myAudio;

    
    public Button btn_loop;
    public Button btn_shuff;
    public Button btn_next;
    public Button btn_previos;
    private Image btn_loop_icon;
    private Image btn_shuff_icon;
    public Image progress_img;
    public string currentDisplayedSongID="";

    const float Playbar_width = 1920;
    

    public bool loop = false;
    public bool shuffle = false;

   
    float time_percent = 0;

    private bool isChangingTime = false;

    void PlayNextSong(bool previos=false)
    {
        if(loop)
            {
                time_percent=0;
                UpdateTimePointerPos();
                PlaySong(music_Flow.GetSongByID(currentPlayedSongID));
                return;
            }
        
        if(!shuffle)
        {
            string NextSong;
            if(!previos)
                NextSong = playlistBar.GetNextSong(currentPlayedSongID);
            else
                NextSong = playlistBar.GetPreviousSong(currentPlayedSongID);

            if(NextSong!=null)
                music_Flow.PlaySong(NextSong);
        }
        else
        {
            string NextSong = playlistBar.GetRandomSong_Otherthan(currentPlayedSongID);
            if(NextSong!=null)
                music_Flow.PlaySong(NextSong);
        }
    }

    void Stop() {
        time_percent=0;
        SetAudioTime(time_percent);
        UpdateTimePointerPos();
        //time_percent=1;
        //UpdateTimePointerPos();
        Play_or_Pause();
    }

    // Start is called before the first frame update
    void Start()
    {
        myAudio = GetComponent<AudioSource>();
        timer1 = Time.time; // testtttttttttttttt
        //time_text = text.GetComponent<TMPro.TextMeshProUGUI>();
        //time_text = text.GetComponent<TMPro.TextMeshProUGUI>();
        play_pause_btn_img =play_pause_btn.GetComponent<Image>();
        btn_loop_icon = btn_loop.GetComponentsInChildren<Image>()[1];
        btn_shuff_icon = btn_shuff.GetComponentsInChildren<Image>()[1];

        btn_loop.onClick.AddListener(delegate
        {
            if(loop) {
                Toggle_Btn_Loop(false);
                //playlistBar.Toggle_Btn_Loop(false);
            }
            else {
                Toggle_Btn_Loop(true);
                //playlistBar.Toggle_Btn_Loop(true);
            }
        });
        btn_shuff.onClick.AddListener(delegate
        {
           if(!shuffle) {
                Toggle_Btn_Shuff(true);
                //playlistBar.Toggle_Btn_Shuff(true);
            }
            else {
                Toggle_Btn_Shuff(false);
                //playlistBar.Toggle_Btn_Shuff(false);
            }
        });

        btn_previos.onClick.AddListener(delegate{Stop(); PlayNextSong(true);});
        btn_next.onClick.AddListener(delegate{Stop(); PlayNextSong(false);});
        //Debug.Log(time_text);
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlaying)
        {
            if(isChangingTime)  // User is changing music time
            {
                SetTimePointerPos_ByMousePos();
            }
            else if(time_percent>=1) // Stop player when a song end.
            {
                Stop();
                PlayNextSong();
            }
            else // Song is countinue playing, change Pointer & Time text
            {  
                //Debug.Log(Time.time);
                time_percent = GetTimePercentFromAudio();
                UpdateTimePointerPos();
                //delay();
            }
        }
        else
        {
            if(isChangingTime)  // User is changing music time
            {
                SetTimePointerPos_ByMousePos();
            }
            else //stop music
            UpdateTimePointerPos();
        }

        
    }

    public void AdjustVolume()
    {
        myAudio.volume = volume_bar.value;
        if(volume_bar.value!=0) 
            not0_volume = volume_bar.value;
        Update_Speaker_Icon();
    }

    private void Update_Speaker_Icon()
    {
        if(volume_bar.value>0.7)
            speaker_icon.sprite = high_sound_icon;
        else if (volume_bar.value>0.3)
            speaker_icon.sprite = med_sound_icon;
        else if (volume_bar.value>0)
            speaker_icon.sprite = low_sound_icon;
        else speaker_icon.sprite = mute_icon;
    }

    public void Mute_Unmute()
    {
        if(myAudio.volume != 0) // Mute
            volume_bar.value = 0;
        else //Unmute
            volume_bar.value = not0_volume;

        Update_Speaker_Icon();
    }

    public void Toggle_Btn_Shuff(bool on)
    {
        if(on)
        {
            btn_shuff_icon.color = new Color(1f,1f,1f,1f);
            shuffle=true;
        }
        else
        {
            btn_shuff_icon.color = new Color(0.6f,0.6f,0.6f,0.6f);
            shuffle=false;
        }
    }

    public void Toggle_Btn_Loop(bool on)
    {
        if(on)
        {
            btn_loop_icon.color = new Color(1f,1f,1f,1f);
            loop=true;
        }
        else
        {
            btn_loop_icon.color = new Color(0.6f,0.6f,0.6f,0.6f);
            loop=false;
        }
    }

    public bool PlaySong(Song song)
    {
        //StartCoroutine(song.Fill_AudioClip(myAudio));
        //StartCoroutine(song.FillSong_Img(song_img));
        AudioClip song_ac= song.audioclip;
        Texture2D tex = song.img;
        // if(currentPlayedSongID=="")
        {
            song_name_text.text = song.data.title;
            artist_name_text.text = song.data.artistsNames;  // load 1
            currentDisplayedSongID=song.data.id;
            isPlaying=true;
            Play_or_Pause();
            // Texture2D t = song.img;
            // song_img.sprite = Sprite.Create(t, new Rect(0.0f, 0.0f, t.width, t.height), new Vector2(0.5f, 0.5f), 100.0f);
            StartCoroutine(FillSongImg(song));
            StartCoroutine(FillSongAudio(song));   // load 2
            StartCoroutine(Display_DownloadProgress(song,progress_img));
            if(tex==null || song_ac==null) 
                return false;
            else 
            {
                return true;
            }
        }
        // else
        // {
        //     if(tex==null || song_ac==null) 
        //     {
        //         Debug.Log(Time.time+ ": Loading...");
        //         return false;
        //     }
        //     else 
        //     {
        //         song_img.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        //         song_name_text.text = song.data.title;
        //         artist_name_text.text = song.data.artistsNames;
                
        //         myAudio.clip = song_ac;
        //         music_Len = song.data.duration;
        //         Debug.Log(music_Len);
        //         len_text.text =  Sec_To_HourMinSec((int)music_Len);
        //         isPlaying=false;
        //         Play_or_Pause();
        //         SetAudioTime(0);
        //         time_percent=0;

        //         currentPlayedSongID=song.data.id;
        //         return true;
        //     }
        
    }

    private IEnumerator FillSongImg(Song song)
    {
        while(song.img==null) 
        {
            yield return new WaitForSeconds(0.4f);
        }
        Texture2D tex = song.img;
        //if(currentPlayedSongID=="") //Fill song Img if there's no currently song playing
            song_img.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
    }

    private IEnumerator FillSongAudio(Song song)
    {
        
        while(song.audioclip==null) 
        {   
            yield return new WaitForSeconds(0.5f);
        }
        AudioClip song_ac= song.audioclip;
        //if(currentPlayedSongID=="" && currentDisplayedSongID==song.data.id) //Play the song if there's no currently song playing
        if(currentDisplayedSongID==song.data.id)
        {
            Debug.Log("load audio complete, play the song now!");
            myAudio.clip = song_ac;
            music_Len = song.data.duration;
            len_text.text =  Sec_To_HourMinSec((int)music_Len);
            isPlaying=false;
            Play_or_Pause();
            SetAudioTime(0);
            time_percent=0;
            currentPlayedSongID=song.data.id;
        }

    }

    IEnumerator Display_DownloadProgress(Song song, Image progress_img)
    {
        while(song.audioclip==null)
        {
            progress_img.rectTransform.offsetMax=new Vector2(song.Get_AudioDownload_Progress()*Playbar_width,0);
            yield return new WaitForSeconds(0.3f);
        }
        progress_img.rectTransform.offsetMax=new Vector2(Playbar_width,0);
    }

    float GetTimePercentFromAudio()
    {
        //return (Time.time - timer1)/music_Len;
        return myAudio.time/music_Len;
    }

    // public void OnClick_SetTime()
    // {
    //     float time_pos = Input.mousePosition.x;
    //     Debug.Log(time_pos);
    //     time_percent = time_pos/time_pointer_max_x;
    //     Debug.Log(time_percent);
    //     SetAudioTime(time_percent);
    //     SetTimePointerPos(time_percent);
    // }

    public IEnumerator delay()
    {
        yield return new WaitForSeconds(0.1f);
    }

    void SetAudioTime(float time_percent)
    {
        // this.time_percent = time_percent;
        // timer1 = Time.time-music_Len*time_percent;
        myAudio.time = music_Len*time_percent;
    }

    void UpdateTimePointerPos()
    {
        time_pointer_trf.localPosition = new Vector3((float)(time_percent-0.5)*time_bar_trf.rect.width,0,0); //x=-0.5/0/0.5 -> 0/50/100%
        color_diff.sizeDelta = new Vector2((float)time_percent*time_bar_trf.rect.width,0);
        UpdateTimeText();
    }

    void UpdateTimeText()
    {
        string s = Sec_To_HourMinSec((int)(time_percent*music_Len));
        //time_text.text = s;
        current_time_text.text = s;
    }

    string Sec_To_HourMinSec(int sec)
    {
        int min = sec/60;
        int hour = min/60;
        min = min%(60*60);
        sec = sec%60;
        string s;
        if(hour<10)
            s= "0"+hour.ToString()+":";
        else s= hour.ToString()+":";
        if(min<10)
            s+= "0"+min.ToString()+":";
        else s+= min.ToString()+":";
        if(sec<10)
            s+= "0"+sec.ToString();
        else s+= sec.ToString();
        return s;
    }

    public void OnMousePointerDown_ChangeTime()
    {
        isChangingTime = true;
        holding_color.SetActive(true);
    }

    void SetTimePointerPos_ByMousePos()
    {
        float mouse_pos = Input.mousePosition.x;
        Debug.Log("mousepos: " + mouse_pos);
        time_percent = (mouse_pos-time_bar_trf_start.position.x)/(time_bar_trf_end.position.x-time_bar_trf_start.position.x);
        Debug.Log("tb rect width: " + time_bar_trf.rect.width);
        Debug.Log("Delta: "+ (mouse_pos-time_bar_trf_start.position.x));
        Debug.Log("Width: "+ (time_bar_trf_end.position.x-time_bar_trf_start.position.x));
        if(time_percent<0)
            time_percent=0;
        else if(time_percent>1)
            time_percent=1;
        Debug.Log(time_percent);
        UpdateTimePointerPos();
    }

    public void OnMousePointerUp_SetTime()
    {
        isChangingTime=false;
        holding_color.SetActive(false);
        Debug.Log("Mouse up, set time");
        SetTimePointerPos_ByMousePos();

        SetAudioTime(time_percent);
    }

    public void Play_or_Pause(){
        isPlaying=!isPlaying;
        if(isPlaying)
        {
            // if(time_percent>=1)
            // {
            //     time_percent=0;
            //     SetAudioTime(time_percent);
            //     UpdateTimePointerPos();
            // }
            play_pause_btn_img.sprite = pause_btn_spr;
            //play audio
            myAudio.Play();
        }
        else 
        {
            play_pause_btn_img.sprite = play_btn_spr;
            //pause audio
            myAudio.Pause();
        }
    }
}

// Old code:
//{

// public void LoadAudio()
    // {
    //     audio_path = "C:\\IT\\Test\\Cơn Mưa Ngang Qua_Sơn Tùng M-TP_-1073981322.mp3";
    //     song_img_path = "C:\\IT\\Test\\1522404477634_640.jpg";
    //     song_name_text.text = "Cơn Mưa Ngang Qua";
    //     StartCoroutine(GetAudioClip());
    //     StartCoroutine(GetSongImg());
    // }
    
    // IEnumerator GetAudioClip()
    // {
    //     using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(audio_path, AudioType.MPEG))
    //     {
    //         yield return www.SendWebRequest();

    //         if (www.result == UnityWebRequest.Result.ConnectionError)
    //         {
    //             Debug.Log(www.error);
    //         }
    //         else
    //         {
    //             myClip = DownloadHandlerAudioClip.GetContent(www);
    //             myAudio.clip = myClip;
    //             music_Len = myClip.length;
    //             len_text.text =  Sec_To_HourMinSec((int)music_Len);
    //             Debug.Log("load audio complete");
    //         }
    //     }
    // }

    // IEnumerator GetSongImg()
    // {   
    //     UnityWebRequest request = UnityWebRequestTexture.GetTexture(song_img_path);
    //     yield return request.SendWebRequest();
    //     if(request.result == UnityWebRequest.Result.ConnectionError) 
    //         Debug.Log(request.error);
    //     else
    //     {
    //         Texture2D tex = ((DownloadHandlerTexture) request.downloadHandler).texture;
    //         song_img.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
    //     }
    // } 
    //}