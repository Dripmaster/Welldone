using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;
namespace Com.CrossLab.WellDone
{
    public class OrderListEntry : MonoBehaviour
    {
        [Header("UI References")]
        public Text PlayerNameText;
        public Button PlayerReadyButton;
        public Image Fill;

        CookOrder cookOrder;
        float eTime;
        float outTime;
        #region UNITY

        public void OnEnable()
        {
            eTime = 0;
            outTime = Random.Range(17,25);
        }

        public void Start()
        {
                PlayerReadyButton.onClick.AddListener(() =>
                {
                    SetPlayerReady();
                    if (PhotonNetwork.IsMasterClient)
                    {

                    }
                });
        }

        public void OnDisable()
        {
        }
        private void Update()
        {
            eTime += Time.deltaTime;
            if (eTime >= outTime)
            {
                OrderManager.instance.timeOut(cookOrder);

                eTime = outTime;
            }
            Fill.fillAmount = (outTime - eTime) / outTime;
        }

        #endregion

        public void Initialize(CookOrder cookOrder)
        {
            this.cookOrder = cookOrder;
            string txt = cookOrder.ToString();
            PlayerNameText.text = txt;
        }
        public void SetPlayerReady()
        {
            OrderManager.instance.updateOrderPanel(GameManager.instance.PutCookObject());
        }
    }
}