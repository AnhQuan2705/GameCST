using UnityEngine;

namespace CST
{
    public class VpbField : MonoBehaviour
    {
        public int eTag;
        public string eName;
        public string eData;
        public string other;
        public bool eBool;
        public int eInt;
        public float eFloat;
        public Vector3 pos;
        public Vector3 scale;
        public GameObject eObject;
        public VpbField()
        {
            eTag = 0;
            eBool = false;
            eInt = 0;
            eFloat = 1.0f;
            eName = "";
            eData = "";
            other = "";
            pos = Vector3.zero;
            scale = new Vector3(1,1,1);
            eObject = null;
        }
    }
}

