using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Com.CrossLab.WellDone {
    public class Timer : MonoBehaviour
    {
        public Text UIText;
        float timerDuraition;
        float tmpDuration;
        bool timerPaused;
        // Start is called before the first frame update
        void OnEnable()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (timerPaused) return;

            tmpDuration -= Time.deltaTime;
            if (tmpDuration <= 0)
            {
                tmpDuration = 0;
                GameManager.instance.TimeEnd();
            }
            updateText();
        }

        public void Init(float seconds)
        {
            timerDuraition = seconds;
            tmpDuration = timerDuraition;
            timerPaused = true;
            updateText();
        }
        public void pauseTimer(bool v)
        {
            timerPaused = v;
        }
        void updateText()
        {
            int m = (int)tmpDuration / 60;
            int s = (int)tmpDuration % 60;
            string t = "";
            if (m != 0)
            {
                t += m + ":";
            }
            t += s;
            UIText.text = t;
        }
    } 
}
