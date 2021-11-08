using System;
using System.Collections;


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using System.Collections.Generic;

using Photon.Pun;
using Photon.Realtime;


namespace Com.CrossLab.WellDone
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static List<PlacedObjects> placedObjects;
        int stageState;
        public static GameManager instance;

        public Timer timer;
        public int StageDuration = 300;
        public Text explaneText;
        public GameObject selectPanel;


        public CookOrder currentHandOrder;
        bool handFree;

        public GameObject[] buttons;

        public GameObject HandPrefab;

        public void Awake()
        {
            timer.gameObject.SetActive(false);
            placedObjects = new List<PlacedObjects>();
            stageState = 0;
            instance = this;
            explaneText.text = "";
            explaneText.gameObject.SetActive(false);
            currentHandOrder = null;
            handFree = true;
        }

        public void Update()
        {
            PlacedObjects tmp = null;

            if (RayUtility.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), out RaycastHit hit))
            {
                PlacedObjects p = hit.collider.gameObject.GetComponentInParent<PlacedObjects>();
                if (p != null)
                {
                    p.onFocused();
                    tmp = p;
                }
            }

            if (stageState == 0)
            {

            }
            else if(stageState == 1)
            {
                bool isEmpty = false;

                if (RayUtility.TryGetInputPosition(out Vector2 touchPoint))
                {
                    
                    if (!RayUtility.isOnUI() && RayUtility.Raycast(touchPoint, out hit))
                    {
                        PlacedObjects p = hit.collider.gameObject.GetComponentInParent<PlacedObjects>();
                        if (p == null)
                        {
                            isEmpty = true;
                        }
                        if (p != null && tmp != null && p ==tmp)
                        {
                            
                            p.onTouched();
                        }
                    }
                    if (isEmpty)
                    {
                        clearHand();
                    }
                }
            }

            foreach (var p in placedObjects)
            {
                if (tmp==null ||  p != tmp)
                {
                    p.offTouched();
                }
            }
        }
        [PunRPC]
        public void StartStage()
        {
            stageState = 1;
            OrderManager.instance.StartOrder();
            timer.Init(StageDuration);
            timer.pauseTimer(false);
            timer.gameObject.SetActive(true);
            explaneText.gameObject.SetActive(true);
            PlacementManager.instance.setActive(false);
            currentHandOrder = null;
            selectPanel.SetActive(false);
        }
        public void TimeEnd()
        {
            timer.pauseTimer(true);
            timer.gameObject.SetActive(false);
            PlacementManager.instance.setActive(true);
            explaneText.gameObject.SetActive(false);
            OrderManager.instance.EndOrder();
            stageState = 0; 
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            selectPanel.SetActive(true);
            clearHand();
            foreach (var item in FindObjectsOfType<PlayerEntry>())
            {
                item.setDefault();
            }
            foreach (var item in buttons)
            {
                item.SetActive(true);
            }
            ScoreManager.instance.showResult();
        }
        /*
        public void GetIngredient(PlacementType ingredient)
        {
            currentHand = new CookedObject(0,(int)ingredient);
            explaneText.text = WellDone.GetName(currentHand.ingredient);
            handFree = false;
        }
        public void GetIngredient(PlacementType ingredient, PlacementType tool)
        {

            currentHand = new CookedObject((int)tool, (int)ingredient);
            explaneText.text = WellDone.GetName(currentHand.tool) + " " + WellDone.GetName(currentHand.ingredient);
            handFree = false;
        }*/
        public void GetCookObject(CookedObject first, CookedObject second,PlacementType finalType)
        {
            currentHandOrder = new CookOrder(first, second, finalType);
            explaneText.text = currentHandOrder.ToString();
            handFree = false;
        }
        public void GetCookObject(CookedObject first, PlacementType finalType)
        {

            currentHandOrder = new CookOrder(first, null, finalType);
            explaneText.text = currentHandOrder.ToString();
            handFree = false;
        }
        /*
        public PlacementType PutIngredient()
        {
            handFree = true;
            return currentHand.ingredient;
        }
        public PlacementType PutTool()
        {
            handFree = true;
            return currentHand.tool;
        }*/

        public bool freeHand()
        {
            return handFree;
        }
        public CookOrder PutCookObject()
        {
            return currentHandOrder;
        }
        public void clearHand()
        {
            explaneText.text = "";
            currentHandOrder = null;
            handFree = true;
            int count = GameObject.Find("ObjectPos").transform.childCount;
            for (int i = 0; i <count ; i++)
            {
                GameObject.Find("ObjectPos").transform.GetChild(i).gameObject.SetActive(false);
                Destroy(GameObject.Find("ObjectPos").transform.GetChild(i).gameObject,1);
            }
        }

        public void chagePlaceHeight()
        {
            foreach (var item in placedObjects)
            {
                Vector3 tmpPos = item.transform.position;
                tmpPos.y = PlacementIndicator.plane.transform.position.y;
                item.transform.position = tmpPos;
            }
        }
    }
}
