using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Com.CrossLab.WellDone
{
    public class PlacedObjUI : MonoBehaviour
    {
        public Text nameText;
        public Image fillImage;
        public Image BackImage;

        string tmpName;
        public void Init(string objName)
        {
            nameText.text = objName;
            tmpName = objName;
            fillImage.fillAmount = 0;
        }
        public void setString(string objName)
        {
            nameText.text = tmpName+"("+objName+")";
        }
        public void onTouched()
        {
            nameText.gameObject.SetActive(true);
            fillImage.gameObject.SetActive(true);
            BackImage.gameObject.SetActive(true);
           
        }
        public void onFocused()
        {
            nameText.gameObject.SetActive(true);
        }
        public void offTouched()
        {
            nameText.gameObject.SetActive(false);
            fillImage.gameObject.SetActive(false);
            BackImage.gameObject.SetActive(false);
        }
        public void setFillAmount(float v)
        {
            fillImage.fillAmount = v;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            nameText.transform.rotation = Quaternion.LookRotation(nameText.transform.position - Camera.main.transform.position);
            fillImage.transform.rotation = Quaternion.LookRotation(nameText.transform.position - Camera.main.transform.position);
            BackImage.transform.rotation = Quaternion.LookRotation(nameText.transform.position - Camera.main.transform.position);
        }
    }
}