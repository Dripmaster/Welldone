using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
namespace Com.CrossLab.WellDone
{
    public class PlacedObjects : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback, IPunObservable
    {
        public Placements placementsSettings;
        public PlacedObjUI placedObjName;

        CookedObject cookOrder;

        GameObject handPrefab;
        float eTime;
        bool checkUp;

        // Start is called before the first frame update
        void Start()
        {
            eTime = 0;
            placedObjName.Init(placementsSettings.objectName);
            checkUp = false;
            offTouched();
            handPrefab = GameManager.instance.HandPrefab;
        }

        // Update is called once per frame
        void Update()
        {
        }
        public void onTouched()
        {
            if (checkUp)
                return;

            eTime += Time.deltaTime;
            placedObjName.setFillAmount(eTime / placementsSettings.duration);
            if (eTime >= placementsSettings.duration)
            {
                eTime = 0;
                onFullTouched();
                checkUp = true;
            }
            placedObjName.onTouched();

        }

        public void onFocused()
        {
            placedObjName.onFocused();
        }
        public void offTouched()
        {
            placedObjName.offTouched();
            checkUp = false;
        }
        public void onFullTouched()
        {
            string debug = "debug:";
            if (placementsSettings.type == Placements.PlacedObjectType.ingredients)
            {
                debug += "재료인식"+ GameManager.instance.freeHand();
                if (GameManager.instance.freeHand()) 
                {
                    debug += "손인식";
                    GameManager.instance.GetCookObject(new CookedObject(0,(int)placementsSettings.objType),PlacementType.NONE);
                    GameObject o = Instantiate(placementsSettings.Handprefab, GameObject.Find("ObjectPos").transform);

                    PhotonView.Get(ScoreManager.instance.gameObject).RPC("AddUse",RpcTarget.All, placementsSettings.price);
                    //ScoreManager.instance.AddUse(placementsSettings.price);
                }
            }
            else
            {
                if (GameManager.instance.freeHand())
                {
                    debug += "손인식";
                    if (placementsSettings.objType == PlacementType.DISH)
                    {
                        debug += "접시들기";
                        GameManager.instance.GetCookObject(new CookedObject(0,0), PlacementType.DISH);//접시 들기
                        GameObject o = Instantiate(placementsSettings.Handprefab, GameObject.Find("ObjectPos").transform);
                    }
                }
                else
                {
                    if (placementsSettings.objType != PlacementType.DISH)
                    {
                        debug += "도구인식";
                        if (GameManager.instance.PutCookObject().CookedObjects[0].ingredient == 0&& GameManager.instance.PutCookObject().CookedObjects[0].tool==0) { //접시
                            debug += "접시에 담기";
                            GameManager.instance.GetCookObject(cookOrder,PlacementType.DISH);
                            GameObject o = Instantiate(handPrefab, GameObject.Find("ObjectPos").transform);
                            cookOrder = null;
                            placedObjName.setString("비어있음");
                        }
                        else //재료
                        {
                            debug += "조리하기";
                            cookOrder = new CookedObject((int)placementsSettings.objType,(int)GameManager.instance.PutCookObject().CookedObjects[0].ingredient);
                            GameManager.instance.clearHand();
                            placedObjName.setString(WellDone.GetName(cookOrder.tool) + " " + WellDone.GetName(cookOrder.ingredient));
                        }
                    }
                }
            }
            Debug.Log(debug);
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            PlacedObjects pScript = gameObject.GetComponent<PlacedObjects>();
            GameManager.placedObjects.Add(pScript);

            object[] data = this.gameObject.GetPhotonView().InstantiationData;

            int placementType = (int)data[0];

            if((int) data[1]==0)
                pScript.placementsSettings = PlacementManager.instance.tools[placementType];                    
            else if ((int)data[1] == 1)
                pScript.placementsSettings = PlacementManager.instance.ings[placementType];


            GameObject objInner = Instantiate((pScript.placementsSettings).prefab, transform);
            gameObject.SetActive(true);

            if(PlacementIndicator.plane != null)
            {
                Vector3 tmpPos = transform.position;
                tmpPos.y = PlacementIndicator.plane.transform.position.y;
                transform.position = tmpPos;
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            
        }
    }

}
