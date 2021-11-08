using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Com.CrossLab.WellDone
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Placements", order = 1)]
    public class Placements : ScriptableObject
    {
        public string objectName;
        public int duration;//0�� �� ��� �տ� ���
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