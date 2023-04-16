using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Start : MonoBehaviour
{

    private void OnEnable()
    {
        this.GetComponent<Animator>().Play("Select_dang_chon");
       // StartCoroutine(SetAnim_start());
    }

    public void Setidle_Anim()
    {
        this.GetComponent<Animator>().Play("idle_dang_chon");
    }

    IEnumerator SetAnim_start()
    {
        yield return new WaitForSeconds(1f);

        this.GetComponent<Animator>().Play("idle_dang_chon");
    }
}
