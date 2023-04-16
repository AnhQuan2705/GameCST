using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VpbScaleButtons : MonoBehaviour
{
    private bool isClick = false;

    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(() => playButtonZoom());
        //this.gameObject.AddComponent<AudioSource>();
    }

    public void playButtonZoom()
    {
        if (isClick)
            return;
        isClick = true;

       // playAudioClip(VpbSound.getAudioForClick());

        if (this.gameObject.GetComponent<Animator>() != null)
        {
            this.gameObject.GetComponent<Animator>().enabled = false;
        }

        this.transform.DOPunchScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f);

        Invoke("SetIsClickOff", 0.5f);
    }
    /*
    public void playAudioClip(AudioClip clip)
    {

        float val = VpbSound.getSoundVolume();
        this.GetComponent<AudioSource>().PlayOneShot(clip,val);

    }*/
    public void SetIsClickOff()
    {
        if (this.gameObject.GetComponent<Animator>() != null)
        {
            this.gameObject.GetComponent<Animator>().enabled = true;
        }
        isClick = false;
    }


}
