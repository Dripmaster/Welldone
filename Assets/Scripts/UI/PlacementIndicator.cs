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
            //AR Component�� �޾ƿ´�.
            rayManager = FindObjectOfType<ARRaycastManager>();
            visual = transform.GetChild(0).gameObject;

            //indicatior�� deactivate�Ѵ�.
            visual.SetActive(false);
        }

        void Update()
        {
            // ȭ�� �߾ӿ��� ray�� ���
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            rayManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);


            if (hits.Count > 0) //���� ray��  ar plane�� �ε�ģ�ٸ� position�� rotation�� ��´�
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

                //���� active�� �����ִٸ� true�� �ٲ۴�.
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