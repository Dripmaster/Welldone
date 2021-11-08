using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
namespace Com.CrossLab.WellDone
{
    public class PlacementManager : MonoBehaviour
    {
        public GameObject placeBtn;
        public GameObject PlacementObj;
        public Placements[] tools;
        public Placements[] ings;

        public static PlacementManager instance;

        private PlacementIndicator placementIndicator;
        private void Awake()
        {
            placeBtn.SetActive(false);
            instance = this;
            placementIndicator = FindObjectOfType<PlacementIndicator>();
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void setActive(bool v)
        {
            placementIndicator.gameObject.SetActive(v);
        }
        public void PlaceObject(int placementType,int placementObjType)
        {
            //placementIndicator.gameObject.SetActive(false);
            object[] data = new object[2];
            data[0] = placementObjType;
            data[1] = placementType;



            GameObject placedObj = PhotonNetwork.Instantiate(PlacementObj.name,placementIndicator.transform.position,Quaternion.identity,0,data);
           
        }
    }
}