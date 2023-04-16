using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notification : MonoBehaviour
{

    private void OnEnable()
    {
        this.GetComponent<Animator>().Play("select_Noti");
        // StartCoroutine(SetAnim_start());
    }

    public void Setidle_Anim()
    {
        this.GetComponent<Animator>().Play("idle_Noti");
        PopupManager.check_Noti = 0;
        StartCoroutine(SetAnim_remove());
    }

    IEnumerator SetAnim_remove()
    {
        yield return new WaitForSeconds(2f);
        DestroyObject(this.gameObject);
    }
}
