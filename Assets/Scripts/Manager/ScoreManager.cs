using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
namespace Com.CrossLab.WellDone
{
    public class ScoreManager : MonoBehaviour
    {
        public GameObject resultPanel;

        public Text scoreText;
        public Text useText;
        public Text resultText;

        public static ScoreManager instance;
        int score;
        int useScore;
        [PunRPC]
        public void AddScore(int v)//주문 완료
        {
            score += v;
        }
        [PunRPC]
        public void AddUse(int v)// 식재료 사용 등 비용
        {
            useScore += v;
        }

        public void showResult()
        {

            scoreText.text = ""+score+ "원";
            useText.text = "재료비:"+ useScore+ "원" + "/세금:"+(int)(score*0.1f)+"원";
            useScore += (int)(score * 0.1f);
            resultText.text = ""+ (score-useScore)+"원";
            resultPanel.SetActive(true);
        }
        public void hideResult()
        {
            resultPanel.SetActive(false);
        }


        // Start is called before the first frame update
        void Start()
        {
            instance = this;
            score = 0;
            useScore = 0;
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}