using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CST
{

    public class VpbDragObject : VpbField
    {
        public GameObject correctForm;
        private VpbScene delegate_ = null;
        public bool isEnabled = true;
        private float startPosX;
        private float startPosY;
        private List<GameObject> targetArr;
        // Start is called before the first frame update
        void Start()
        {
            eName = "DRAG_OBJECT";
            correctForm = null;
            pos = this.gameObject.transform.position;
            eInt = -1;
            targetArr = new List<GameObject>();
        }
        public void SetDelegate(VpbScene _delegate, List<GameObject> _targetArr = null)
        {
            delegate_ = _delegate;
            targetArr = _targetArr;
            resetZoder(gameObject,90);
            //Debug.Log("setpos");
        }

        // Update is called once per frame
        void Update()
        {
                if (eBool)
                {
                    Vector3 mousePos;
                    mousePos = Input.mousePosition;
                    mousePos = Camera.main.ScreenToWorldPoint(mousePos);
                    this.gameObject.transform.position = new Vector3(mousePos.x, mousePos.y, this.gameObject.transform.position.z);
                    //this.gameObject.transform.position = new Vector2(mousePos.x - startPosX, mousePos.y - startPosY);
                }
        }

        private void OnMouseDown()
        {
            if (!isEnabled )
                return;

            if (Input.GetMouseButton(0))
            {
                eBool = true;
                resetZoder(gameObject,100);
                Debug.Log("drag");
                if (correctForm != null && correctForm.gameObject != null)
                {
                   // correctForm.GetComponent<BoxCollider2D>().enabled = true;
                    correctForm.GetComponent<VpbField>().eObject = null;
                    correctForm = null;
                }
                if (this.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>() != null)
                {
                    this.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;
                    Color myColor = new Color();
                    ColorUtility.TryParseHtmlString("#FFFFFF", out myColor);
                    this.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().color = myColor;
                }
                }
           this.GetComponent<Animator>().Play("Default");
        }

        private void OnMouseUp()
        {
            if (!isEnabled )
                return;
            eBool = false;
            this.GetComponent<Animator>().Play("Pressed");
            if (eObject != null) // xoa old target 
            {
                eObject.GetComponent<BoxCollider2D>().enabled = true;
                eObject.GetComponent<VpbField>().eObject = null;
                eObject = null;
            }
            //---------------------------------
            if (correctForm != null)
            {
                
                if (correctForm.GetComponent<VpbField>().eObject != null)
                {
                    GameObject old = correctForm.GetComponent<VpbField>().eObject;
                    old.transform.position = old.GetComponent<VpbField>().pos;
                    resetZoder(old,90);
                    old.GetComponent<VpbField>().eObject = null;
                    correctForm.GetComponent<VpbField>().eObject = null;
                }
                //----------------------------
                correctForm.GetComponent<VpbField>().eObject = gameObject;
                eObject = correctForm;
                this.gameObject.transform.position = new Vector2(correctForm.transform.position.x, correctForm.transform.position.y);
                Debug.Log("drag finish : call delegate");
                this.GetComponent<Animator>().Play("start 1");
                //this.GetComponentInChildren<ParticleSystem>().Play();

                eObject.GetComponent<BoxCollider2D>().enabled = false;
                resetZoder(gameObject, 90);
                if (this.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>()!=null) 
                {
                    this.transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<Text>().color = Color.white;
                    this.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>().enabled = true;

                    //Color myColor = new Color();
                    //ColorUtility.TryParseHtmlString("#FFFFFF", out myColor);
                }
                
            }
            else
            {
                this.gameObject.transform.position = pos;
                this.GetComponent<Animator>().Play("start 1");
                resetZoder(gameObject,90);
                Debug.Log("thoat drag");
                if (this.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>() != null)
                {
                    this.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Image>().enabled = false;
                    Color myColor = new Color();
                    ColorUtility.TryParseHtmlString("#93278F", out myColor);
                    this.transform.GetChild(0).transform.GetChild(1).gameObject.GetComponent<Text>().color = myColor;
                }
            }
            if (delegate_)
            {
                delegate_.callFuncWithObject(gameObject);
            }
            correctForm = null;

            
        }
        private void resetZoder(GameObject obj,int z)
        {
            
            int num = obj.transform.childCount;
            for (int i = 0; i < num; i++)
            {
                GameObject g = gameObject.transform.GetChild(i).gameObject;
                //Debug.Log(obj.name + "/" +  g.name);
                if (g.GetComponent<Canvas>())
                {
                    g.GetComponent<Canvas>().sortingOrder = z + 5;
                }
                if (g.GetComponent<SpriteRenderer>())
                {
                    g.GetComponent<SpriteRenderer>().sortingOrder = z + 5;
                }

            }
            obj.GetComponent<SpriteRenderer>().sortingOrder = z;
        }
        private GameObject lastObject = null;
        private void OnTriggerEnter2D(Collider2D col)
        {
            VpbField vp = col.GetComponent<VpbField>();
            if(vp != null && eBool)
            {
                if (eName == "DRAG_OBJECT" && vp.eObject != null)
                {
                    correctForm = vp.eObject;
                    lastObject = col.gameObject;
                }
                else if (col.tag == "number")
                {
                    correctForm = col.gameObject;
                    lastObject = col.gameObject;
                }
               
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            VpbField vp = col.GetComponent<VpbField>();
            if (vp != null && eBool && lastObject == col.gameObject)
            {
                correctForm = null;
            }
        }

    }
}