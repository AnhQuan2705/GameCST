using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using CI.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CST
{
    public class SeePicAndDrag : VpbScene
    {

        [SerializeField]
        private TMPro.TextMeshProUGUI descriptionText;

        [SerializeField]
        private GameObject dragContainer;
        [SerializeField]
        private GameObject dragTarget;

        [SerializeField]
        private GameObject btnDrag;
        [SerializeField]
        private GameObject btnShadow;
        [SerializeField]
        private GameObject btnTarget;
        [SerializeField] private Image bgMainImg;

        public List<GameObject> btnDragArr;

        public List<GameObject> targetArr;

        public List<string> data;
        public string title = "";
        public List<string> answers;
        public List<string> right_answers;
        // public List<string> others;
        public string description;

        private string playableName;

        public TextAsset jsonFile;

        public Image _imgq1;
        private Sprite mySprite;

        private GameObject _gameController;

        public GameObject _goTitle;


        private void OnEnable()
        {
            ShowAnimTitle();
        }

        public void ShowAnimTitle()
        {
            //D
            StartCoroutine(SetAnimTitle2());
        }


        IEnumerator SetAnimTitle2()
        {
            yield return new WaitForSeconds(0.1f);

            string pathD = Application.persistentDataPath + "/Playables/" + playableName + "/data.json";

            if (PopupManager.check_showAnim == 0)
            {
                LoadPopTitle("Title/AnimTitle");
            }

            if (System.IO.File.Exists(pathD))
            {
                string fileContents = File.ReadAllText(pathD);
                MatData gameData = JsonUtility.FromJson<MatData>(fileContents);

                _gameController = GameObject.FindGameObjectWithTag("GameController");

                if (gameData.title != "")
                {
                    _gameController.GetComponent<Game_controller>()._btQPlayView.SetActive(true);
                    if (PopupManager.check_showAnim == 0)
                    {
                        _goTitle.transform.GetChild(0).GetComponent<TitleAnim>().RunAnim();
                        _goTitle.transform.GetChild(0).GetComponent<TitleAnim>()._txtAnim.text = gameData.title;
                    }
                }
                else
                {
                    _gameController.GetComponent<Game_controller>()._btQPlayView.SetActive(false);
                }
                if (gameData.redings != "")
                {
                    playableName = PopupManager.selectedPractice.tem_playables[PopupManager.index].playable;
                    string path = Application.persistentDataPath + "/Playables/" + playableName + "/data.json";
                    string pathQ = Application.persistentDataPath + "/Playables/" + playableName + "/bai_doc.png";
                    if (System.IO.File.Exists(path))
                    {
                        byte[] bytes = System.IO.File.ReadAllBytes(pathQ);
                        Texture2D texture = new Texture2D(1, 1);
                        texture.LoadImage(bytes);
                        PopupManager._imgbaidoc = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                        // _goTitle.GetComponent<TitleAnim>()._imgq1.sprite = PopupManager._imgbaidoc;
                    }

                    _gameController.GetComponent<Game_controller>()._btQPlayView.SetActive(true);
                    if (PopupManager.check_showAnim == 0)
                    {
                        _goTitle.transform.GetChild(0).GetComponent<TitleAnim>().RunAnim_baidoc();
                    }
                }
            }
        }


        public void LoadPopTitle(string name)
        {
            GameObject canvas = GameObject.Find("CanvasTitle");
            foreach (Transform t in canvas.transform.GetChild(0))
            {
                Destroy(t.gameObject);
            }
            GameObject instance = Instantiate(Resources.Load(name, typeof(GameObject))) as GameObject;
            instance.transform.SetParent(canvas.transform.GetChild(0));
            instance.transform.localScale = new Vector3(1, 1, 1);
            instance.transform.position = canvas.transform.position;
            _goTitle = instance;
        }


        void Start()
        {

            playableName = PopupManager.selectedPractice.tem_playables[PopupManager.index].playable;
            Debug.Log(PopupManager.index);
            InitQuest();
        }

        // Start is called before the first frame update
        protected void SampleQuest()
        {


            string pathQ = Application.persistentDataPath + "/Playables/" + playableName + "/Q1.png";
            Debug.Log(pathQ);
            if (System.IO.File.Exists(pathQ))
            {
                byte[] bytes = System.IO.File.ReadAllBytes(pathQ);
                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(bytes);
                mySprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                _imgq1.sprite = mySprite;
            }

            string pathBG = Application.persistentDataPath + "/Playables/" + playableName + "/main_background.png";
            Debug.Log(pathBG);
            if (System.IO.File.Exists(pathBG))
            {
                byte[] bytes = System.IO.File.ReadAllBytes(pathBG);
                Texture2D texture = new Texture2D(1, 1);
                texture.LoadImage(bytes);
                mySprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), 100.0f);
                bgMainImg.sprite = mySprite;
            }

            string pathD = Application.persistentDataPath + "/Playables/" + playableName + "/data.json";
            if (System.IO.File.Exists(pathD))
            {
                string fileContents = File.ReadAllText(pathD);
                MatData gameData = JsonUtility.FromJson<MatData>(fileContents);
                Debug.Log(gameData.answers[0]);
                answers = new List<string>(gameData.answers);
                description = gameData.question;
                right_answers = new List<string>(gameData.right_answer);

            }

            title = "";
        }

        public void configView(string plbName)
        {
            playableName = plbName;
        }

        void setupText()
        {
            Match m = Regex.Match(description, @"<#>\s*(.+?)\s*</#>");
            Match mm;
            string str = description;
            List<string> arr = new List<string>();
            mm = m;
            int k = 0;
            bool isOK = true;
            int c = mm.Groups.Count;

            while (isOK)
            {
                if (k == 0)
                {
                    mm = m;
                }
                else
                {
                    mm = mm.NextMatch();

                    if (mm.Groups[1].Length == 0)
                    {
                        isOK = false;
                    }
                }

                if (isOK)
                {
                    arr.Add(mm.Groups[1] + "");
                    int tr = str.IndexOf(arr[k] + "</#>");
                    str = str.Replace("<#>" + arr[k] + "</#>", " <link> ........... </link> ");
                }
                k++;
            }

            description = str;
            descriptionText.text = description;

            StartCoroutine(SetTMP());

        }

        IEnumerator SetTMP()
        {
            yield return new WaitForSeconds(0.25f);

            TMP_TextInfo textInfo = descriptionText.textInfo;
            targetArr = new List<GameObject>();
            float x_, y_;
            for (int i = 0; i < textInfo.linkCount; i++)
            {
                GameObject target_ = Instantiate(btnTarget, dragTarget.transform);
                VpbField drag = target_.AddComponent<VpbField>() as VpbField;
                target_.GetComponent<SpriteRenderer>().sortingOrder = 80;
                drag.eTag = i;
                Vector2 vxy = CalcLinkCenterPosition(textInfo, i);
                x_ = vxy.x * 0.02f - 8f;
                y_ = vxy.y * 0.02f - 7.65f;
                target_.transform.position = new Vector3(x_, y_, 0);
                target_.transform.localScale = new Vector3(0.8f, 0.8f, 0);
                targetArr.Add(target_);
            }
        }


        Vector2 CalcLinkCenterPosition(TMP_TextInfo textInfo, int k)
        {
            Transform m_Transform = gameObject.GetComponent<Transform>();
            Vector3 bottomLeft = Vector3.zero;
            Vector3 topRight = Vector3.zero;
            float maxAscender = -Mathf.Infinity;
            float minDescender = Mathf.Infinity;
            TMP_LinkInfo linkInfo = textInfo.linkInfo[k];
            TMP_CharacterInfo currentCharInfo = textInfo.characterInfo[linkInfo.linkTextfirstCharacterIndex];
            maxAscender = Mathf.Max(maxAscender, currentCharInfo.ascender);
            minDescender = Mathf.Min(minDescender, currentCharInfo.descender);
            bottomLeft = new Vector3(currentCharInfo.bottomLeft.x, currentCharInfo.descender, 0);
            bottomLeft = m_Transform.TransformPoint(new Vector3(bottomLeft.x, minDescender, 0));
            topRight = m_Transform.TransformPoint(new Vector3(currentCharInfo.topRight.x, maxAscender, 0));
            float width = topRight.x - bottomLeft.x;
            float height = topRight.y - bottomLeft.y;
            Vector2 centerPosition = bottomLeft;
            centerPosition.x += width / 2;
            centerPosition.y += height / 2;
            return centerPosition;
        }

        string number;

        void InitQuest()
        {

            SampleQuest();
            setupText();

            float x_, y_;
            int row, col;

            btnDragArr = new List<GameObject>();
            var len = answers.Count;

            string st = "";

            for (int d = 0; d < answers.Count; d++)
            {
                st += d.ToString();
            }

            List<char> lisTemp = new List<char>();
            char[] temArr = st.ToCharArray();
            foreach (char c in temArr)
            {
                lisTemp.Add(c);
            }
            lisTemp = utilRandomList<char>(lisTemp);

            number = new string(lisTemp.ToArray());

            for (int i = 0; i < number.Length; i++)
            {
                row = i / 6;
                col = i % 6;
                GameObject btnShadow_ = Instantiate(btnShadow, dragContainer.transform);
                GameObject btnDrag_ = Instantiate(btnDrag, dragContainer.transform);
                if (len == 5)
                {
                    x_ = col * 1.97f;
                }
                else
                {
                    x_ = col * 2.5f;
                    if (len == 3)
                    {
                        x_ = col * 3.2f;
                    }
                }

                y_ = 2.5f;

                btnDrag_.transform.position = new Vector3(x_, y_, 0);
                btnShadow_.transform.position = new Vector3(x_, y_, 0);

                btnDrag_.transform.localScale = new Vector3(1f, 1f, 0);
                btnShadow_.transform.localScale = new Vector3(1f, 1f, 0);

                //-----
                int memeValue;
                int.TryParse(number[i].ToString(), out memeValue);

                VpbDragObject drag = btnDrag_.AddComponent<VpbDragObject>() as VpbDragObject;
                drag.SetDelegate(this);
                drag.eTag = i;
                drag.eData = answers[memeValue];

                GameObject g1 = btnDrag_.transform.Find("Canvas/lbText").gameObject;
                GameObject g2 = btnShadow_.transform.Find("Canvas/lbText").gameObject;
                if (g1 != null)
                {
                    g1.GetComponent<Text>().text = answers[memeValue];
                    g2.GetComponent<Text>().text = answers[memeValue];
                }
                btnDragArr.Add(btnDrag_);
            }
        }

        public void SetAnswer()
        {
            if (answers.Count > 0)
            {
                int k = 0;

                List<int> posArr = new List<int>();
                List<string> tempArr = new List<string>();
                foreach (string a in answers) // clone answer keys
                {
                    tempArr.Add(a);
                    posArr.Add(-1);
                }
                //------------------------------------- tim kiem vi tri tra loi
                int i = 0;
                foreach (string answer in answers)
                {
                    // k = 0;
                    foreach (string s in tempArr)
                    {
                        if (answer == s)
                        {
                            posArr[i] = k;
                            tempArr[k] = "";
                            break;
                        }
                        k++;
                    }
                    i++;
                }

                // so thu tu trtong target
                List<int> sTargetArr = new List<int>();
                int j = 0;
                foreach (string s in answers)
                {
                    sTargetArr.Add(j);
                    j++;
                }
                // set up lai vi tri da tra loi.
                k = 0;
                int pAnswer = -1;

                foreach (string answer in answers)
                {
                    int a;
                    int.TryParse(answer, out a);
                    pAnswer = a;

                    if (answer != "")
                    {
                        if (pAnswer >= 0)
                        {
                            btnDragArr[pAnswer].transform.position = targetArr[k].transform.position;
                            //btnDragArr[pAnswer].transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().enabled = true;
                            //Color myColor = new Color();
                            //ColorUtility.TryParseHtmlString("#FFFFFF", out myColor);
                            //btnDragArr[pAnswer].transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().color = myColor;
                            btnDragArr[pAnswer].GetComponent<VpbDragObject>().eObject = targetArr[sTargetArr[k]];
                            targetArr[sTargetArr[k]].GetComponent<VpbField>().eObject = btnDragArr[pAnswer];

                            if (targetArr[sTargetArr[k]].GetComponent<BoxCollider2D>() != null)
                            {
                                targetArr[sTargetArr[k]].GetComponent<BoxCollider2D>().enabled = false;
                            }
                        }
                        k++;
                    }
                    else
                    {
                        k++;
                    }
                }
            }
        }

        public List<string> answersU;
        public int score;

        public void getAnswer()
        {

            // int j = 0;
            // int i = 0;
            // answersU.Clear();
            // foreach (string s in answers)
            // {
            //     answersU.Add("");
            //     if (targetArr[i].GetComponent<VpbField>().eObject != null)
            //     {
            //         answersU[j] = "" + targetArr[i].GetComponent<VpbField>().eObject.GetComponent<VpbDragObject>().eTag;
            //     }
            //     j++;
            //     i++;
            // }
        }


        //-- Kiểm tra kết quả
        public void CalScore()
        {
            PopupManager.listAnswer[PopupManager.index].answer_times += 1;
            Debug.Log("------sdfsdf---sdfsdfsd--dsfsdfsdf--sdfsdfds");
            // listAnswer.=
            // getAnswer();
            // bool isTrue = true;
            // int i = 0;
            //
            // Debug.Log(answersU);
            // foreach (string s in answersU)
            // {
            //     int a;
            //     int.TryParse(s, out a);
            //
            //     string b = btnDragArr[a].GetComponent<VpbDragObject>().eData.ToString();
            //
            //     if (b != answers[i])
            //     {
            //         isTrue = false;
            //         break;
            //     }
            //     i++;
            // }
            // if (!isTrue)
            // {
            //     score = 0;
            // }
            // else
            // {
            //     //--demo 
            //     score = 3;
            //     int index = PlayerController.index - 1;
            //     PopupManager.listAnswer[index] = true;
            //     Debug.Log(PopupManager.listAnswer.Count);
            // }

        }

    }
}
