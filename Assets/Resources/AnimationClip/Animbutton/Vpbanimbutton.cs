using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vpbanimbutton : MonoBehaviour
{
    public bool _checkAnimbutton = false;

    private void OnEnable()
    {
        if (_checkAnimbutton == true)
        {
            this.GetComponent<Animator>().Play("Pressed");
        }
        else
        {
            StartCoroutine(Setanimbutton());
        }
    }

    IEnumerator Setanimbutton()
    {
        yield return new WaitForSeconds(0.2f);
        this.GetComponent<Animator>().Play("Pressed");
    }

}
