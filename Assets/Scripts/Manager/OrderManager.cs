using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
namespace Com.CrossLab.WellDone
{
    public class OrderManager : MonoBehaviour
    {
        public static OrderManager instance;
        public List<CookOrder> cookOrders;

        public GameObject TopPanel;
        public GameObject BottomPanel;
        public GameObject orderPrefab;


        public float orderDuration = 5;



        private void Awake()
        {
            instance = this;
            cookOrders = new List<CookOrder>();
        }
        public void StartOrder()
        {
            for (int i = 0; i < TopPanel.transform.childCount; i++)
            {
                TopPanel.transform.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < BottomPanel.transform.childCount; i++)
            {
                BottomPanel.transform.GetChild(i).gameObject.SetActive(false);
            }
            if(PhotonNetwork.IsMasterClient)
            StartCoroutine(OrderSpawn());
        }
        public void EndOrder()
        {
            for (int i = 0; i < TopPanel.transform.childCount; i++)
            {
                TopPanel.transform.GetChild(i).gameObject.SetActive(true);
            }
            for (int i = 0; i < BottomPanel.transform.childCount; i++)
            {
                BottomPanel.transform.GetChild(i).gameObject.SetActive(true);
            }

            for (int i = cookOrders.Count-1; i >= 0; i--)
            {
                Destroy(cookOrders[i].orderObject);
                cookOrders.RemoveAt(i);
            }
            if (PhotonNetwork.IsMasterClient)
                StopAllCoroutines();
        }
        public void timeOut(CookOrder cookOrder)
        {
            PhotonView photonView = PhotonView.Get(this);
            photonView.RPC("RemoveOrder", RpcTarget.All, cookOrders.IndexOf(cookOrder));
        }
        IEnumerator OrderSpawn()
        {
            do
            {
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("AddOrder", RpcTarget.All, Random.Range(0, 2), Random.Range(1, 3), Random.Range(7, 9), Random.Range(1, 3), Random.Range(7, 9), Random.Range(4, 7), Random.Range(9000, 15000));

                while (cookOrders.Count >= 4)
                    {
                        yield return null;
                    }


                yield return new WaitForSeconds(orderDuration);
            } while (true);
        }
        public void updateOrderPanel(CookOrder cookOrder)
        {
            CookOrder targetOrder = null;
            if (cookOrder == null)
                return;
            foreach (var order in cookOrders)
            {
                if (order.finalType == cookOrder.finalType)
                {
                    int checkFlag = order.CookedObjects.Count;
                    foreach (var orderItem in order.CookedObjects)
                    {
                        foreach (var cookedItem in cookOrder.CookedObjects)
                        {
                            if (cookedItem == null)
                                continue;
                            if (orderItem.ingredient == cookedItem.ingredient && orderItem.tool == cookedItem.tool)
                            {
                                checkFlag--;
                                continue;
                            }
                        }
                    }
                    if (checkFlag == 0)
                    {
                        targetOrder = order;
                        break;
                    }
                }
            }
            if(targetOrder != null)
            {
                PhotonView photonView = PhotonView.Get(this);
                photonView.RPC("RemoveOrder", RpcTarget.All, cookOrders.IndexOf(targetOrder));

                GameManager.instance.clearHand();
            }
        }
        [PunRPC]
        public void RemoveOrder(int orderId)
        {
            ScoreManager.instance.AddScore(cookOrders[orderId].price);
            Destroy(cookOrders[orderId].orderObject);
            cookOrders.Remove(cookOrders[orderId]);
        }

        [PunRPC]
        public void AddOrder(int a, int b, int c, int d, int e, int f, int g)
        {
            CookOrder cookOrder = new CookOrder(a,b,c,d,e,f,g);
            cookOrders.Add(cookOrder);

            GameObject orderObject = Instantiate(orderPrefab);
            orderObject.transform.SetParent(TopPanel.transform);
            orderObject.transform.localScale = Vector3.one;
            orderObject.GetComponent<OrderListEntry>().Initialize(cookOrder);
            cookOrder.orderObject = orderObject;
        }
    
    }
    public class CookOrder
    {
        public List<CookedObject> CookedObjects;
        public PlacementType finalType;
        public GameObject orderObject;
        public int price;
        public CookOrder(int a, int b , int c, int d,int e, int f, int g)
        {
            CookedObjects = new List<CookedObject>();
           
                CookedObjects.Add(new CookedObject(b, c));
                finalType = PlacementType.DISH;

            
            price = g;
        }
        public CookOrder(CookedObject first, CookedObject second, PlacementType final)
        {
            CookedObjects = new List<CookedObject>();
            finalType = final;
            CookedObjects.Add(first);
            CookedObjects.Add(second);
        }
        public override string ToString()
        {
            string txt = "";
            foreach (var item in CookedObjects)
            {
                if (item == null)
                    continue;
                if (txt.Length > 0)
                    txt += "¿¡ ";
                txt += WellDone.GetName(item.tool) + " " + WellDone.GetName(item.ingredient);
            }
            if (finalType != PlacementType.DISH)
            {
                txt = WellDone.GetName(finalType) + " " + txt;
            }
            return txt;
        }
    }
    public class CookedObject
    {
        public PlacementType tool;
        public PlacementType ingredient;
        public CookedObject(int t, int i)
        {
            tool = (PlacementType)t;
            ingredient = (PlacementType)i;
        }
    }
}