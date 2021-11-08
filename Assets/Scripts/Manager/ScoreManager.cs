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
        public void AddScore(int v)//�ֹ� �Ϸ�
        {
            score += v;
        }
        [PunRPC]
        public void AddUse(int v)// ����� ��� �� ���
        {
            useScore += v;
        }

        public void showResult()
        {

            scoreText.text = ""+score+ "��";
            useText.text = "����:"+ useScore+ "��" + "/����:"+(int)(score*0.1f)+"��";
            useScore += (int)(score * 0.1f);
            resultText.text = ""+ (score-useScore)+"��";
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