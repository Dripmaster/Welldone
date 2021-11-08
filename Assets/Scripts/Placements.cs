using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Com.CrossLab.WellDone
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Placements", order = 1)]
    public class Placements : ScriptableObject
    {
        public string objectName;
        public int duration;//0일 시 즉시 손에 든다
        public int price;

        public GameObject prefab;
        public GameObject Handprefab;
        public PlacedObjectType type;
        public PlacementType objType;

        public enum PlacedObjectType
        {
            tool = 1,
            ingredients = 2
        }
    }
}