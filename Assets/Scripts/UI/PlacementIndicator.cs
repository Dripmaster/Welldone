using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
namespace Com.CrossLab.WellDone
{
    public class PlacementIndicator : MonoBehaviour
    {
        public static ARPlane plane;
        public ARPlaneManager m_PlaneManager;

        private ARRaycastManager rayManager;
        private GameObject visual;

        void Start()
        {
            //AR Component를 받아온다.
            rayManager = FindObjectOfType<ARRaycastManager>();
            visual = transform.GetChild(0).gameObject;

            //indicatior를 deactivate한다.
            visual.SetActive(false);
        }

        void Update()
        {
            // 화면 중앙에서 ray를 쏜다
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            rayManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);


            if (hits.Count > 0) //만약 ray가  ar plane과 부딪친다면 position과 rotation을 얻는다
            {
                transform.position = hits[0].pose.position;
                transform.rotation = hits[0].pose.rotation;

                if(plane == null)
                {
                    plane  = m_PlaneManager.GetPlane(hits[0].trackableId);
                }
                else
                {
                    if (hits[0].pose.position.y <= plane.transform.position.y)
                    {
                        plane = m_PlaneManager.GetPlane(hits[0].trackableId);

                        GameManager.instance.chagePlaceHeight();
                    }
                }

                //만약 active가 꺼져있다면 true로 바꾼다.
                if (!visual.activeInHierarchy)
                {
                    visual.SetActive(true);
                    PlacementManager.instance.placeBtn.SetActive(true);
                    //PlacementManager.instance.moveScreen.SetActive(false);
                }

            }
        }
    }
}