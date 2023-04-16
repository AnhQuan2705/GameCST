using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SettingvideoYoutube : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject _sliderVolume;

    public AudioSource _videoSound;

    public Image _IconPlay;
    public Sprite _iconOn;
    public Sprite _iconPase;

    public Slider _slider2d;

    int _checkplayVideo = 0;
    int _checkVolumeVideo = 0;

    // Start is called before the first frame update
    void Start()
    {
        _IconPlay.sprite = _iconOn;

        Seek();
    }

    public void ClickPlayIconVideo()
    {
        if (_checkplayVideo==0)
        {
            videoPlayer.Pause();
            _IconPlay.sprite = _iconPase;
            _checkplayVideo = 1;
        }
        else
        {
            videoPlayer.Play();
            _IconPlay.sprite = _iconOn;
            _checkplayVideo = 0;
        }
    }

    public void ClickVolumeIconVideo()
    {
        if (_checkVolumeVideo == 0)
        {
            _sliderVolume.SetActive(true);
            _checkVolumeVideo = 1;
        }
        else
        {
            _sliderVolume.SetActive(false);
            _checkVolumeVideo = 0;
        }
    }



    public void ClickSlider()
    {
        _videoSound.volume = _slider2d.value;
    }

    public bool _checkpause = false;
    public Text currentTime;
    public Text totalTime;

    public Slider m_PlaybackProgress;
    public Slider m_PlaybackProgressOn;


    public  RectTransform m_RectTransform;

    private float currentVideoDuration;
    private float totalVideoDuration;


    int _click = 0;
    // Update is called once per frame
    void Update()
    {
        if (videoPlayer.isPlaying && _checkpause == false)
        {
            m_PlaybackProgressOn.value =(float)(videoPlayer.length > 0 ? videoPlayer.time / videoPlayer.length : 0);

            currentVideoDuration = Mathf.RoundToInt(videoPlayer.frame / videoPlayer.frameRate);
            currentTime.text = FormatTime(Mathf.RoundToInt(currentVideoDuration));

            totalVideoDuration = Mathf.RoundToInt(videoPlayer.frameCount / videoPlayer.frameRate);
            totalTime.text = FormatTime(Mathf.RoundToInt(totalVideoDuration));

        }
    }

    private string FormatTime(int time)
    {
        int hours = time / 3600;
        int minutes = (time % 3600) / 60;
        int seconds = (time % 3600) % 60;
        if (hours == 0 && minutes != 0)
        {
            return minutes.ToString("00") + ":" + seconds.ToString("00");
        }
        else if (hours == 0 && minutes == 0)
        {
            return "00:" + seconds.ToString("00");
        }
        else
        {
            return hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    Seek(Input.mousePosition);
    //}

    //public void OnDrag(PointerEventData eventData)
    //{
    //    Seek(Input.mousePosition);
    //}

    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    videoPlayer.Pause();
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    videoPlayer.Play();
    //}

   public void Seek()
    {
        
            Debug.Log("1------------------");

            _click = 1;
            videoPlayer.Pause();
            _checkpause = true;
            videoPlayer.time = videoPlayer.length * m_PlaybackProgress.value;
            _checkpause = false;
            videoPlayer.Play(); 
       
       // StartCoroutine(PlayVideo());
    }
    IEnumerator PlayVideo()
    {
        yield return new WaitForSeconds(1f);
        _click = 0;
    }
}
