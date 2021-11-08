using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.CrossLab.WellDone
{
    public class PlaceSelect : MonoBehaviour
    {

        public Button PlayerReadyButton;

        public int myValue;
        public void Start()
        {
            PlayerReadyButton.onClick.AddListener(() =>
            {
                LobbyManager.instance.selectItem(myValue);
            });
        }
    }
}
