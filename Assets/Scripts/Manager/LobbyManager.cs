using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Com.CrossLab.WellDone
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        public GameObject TopPanel;
        public GameObject StartGameButton;

        public GameObject PlayerListEntryPrefab;

        public GameObject playerPrefab;
        public GameObject itemBtn;
        public GameObject selectPanel;

        private Dictionary<int, GameObject> playerListEntries;
        private int placementType;
        private int placementObjType;

        public static LobbyManager instance;
        #region Photon Callbacks


        /// <summary>
        /// Called when the local player left the room. We need to load the launcher scene.
        /// </summary>
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }
        public override void OnPlayerEnteredRoom(Player other)
        {
            GameObject entry = Instantiate(PlayerListEntryPrefab);
            entry.transform.SetParent(TopPanel.transform);
            entry.transform.localScale = Vector3.one;
            entry.GetComponent<PlayerEntry>().Initialize(other.ActorNumber, other.NickName);

            playerListEntries.Add(other.ActorNumber, entry);

            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
            playerListEntries.Remove(otherPlayer.ActorNumber);

            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }

        public void JoinedRoom()
        {
            // joining (or entering) a room invalidates any cached lobby room list (even if LeaveLobby was not called due to just joining a room)
            Debug.Log("OnJoined");
            if (playerListEntries == null)
            {
                playerListEntries = new Dictionary<int, GameObject>();
            }

            foreach(Player p in PhotonNetwork.PlayerList) {
                GameObject entry = Instantiate(PlayerListEntryPrefab);
                entry.transform.SetParent(TopPanel.transform);
                entry.transform.localScale = Vector3.one;
                entry.GetComponent<PlayerEntry>().Initialize(p.ActorNumber, p.NickName);

                object isPlayerReady;
                
                if (p.CustomProperties.TryGetValue(WellDone.PLAYER_READY, out isPlayerReady))
                {
                    entry.GetComponent<PlayerEntry>().SetPlayerReady((bool)isPlayerReady);
                }
                playerListEntries.Add(p.ActorNumber, entry);
            }
            
            FindObjectOfType<LobbyManager>().LocalPlayerPropertiesUpdated();
            
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
            {
                {WellDone.PLAYER_LOADED_LEVEL, false}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            //playerAvatar »ý¼º
            if (PlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                object[] data = new object[1];
                data[0] = PhotonNetwork.NickName;
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0,data);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }


        }
        public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
        {
            if (playerListEntries == null)
            {
                playerListEntries = new Dictionary<int, GameObject>();
            }

            GameObject entry;
            if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
            {
                object isPlayerReady;
                if (changedProps.TryGetValue(WellDone.PLAYER_READY, out isPlayerReady))
                {
                    entry.GetComponent<PlayerEntry>().SetPlayerReady((bool)isPlayerReady);
                }
            }

            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }
        #endregion
        private void Awake()
        {
            JoinedRoom();
            placementType = 0;
            placementObjType = 0;
            instance = this;
        }
        private void Update()
        {
            if(tools == null)
            initPlace();
        }

        #region Public Methods
        public void LocalPlayerPropertiesUpdated()
        {
            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }

        public void OnStartGameButtonClicked()
        {
            try
            {

                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
            }
            catch (System.Exception)
            {

            }

            //PhotonNetwork.LoadLevel("DemoAsteroids-GameScene"); 
            //GameManager.instance.StartStage();
            PhotonView photonView = PhotonView.Get(GameManager.instance);
            photonView.RPC("StartStage",RpcTarget.All);
        }
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void PlaceObject()
        {
            PlacementManager.instance.PlaceObject(placementType, placementObjType);
        }
        public void selectItem(int v)
        {
            placementObjType = v;
        }
        public void selectTools()
        {
            placementType = 0;
            foreach (var item in ings)
            {
                item.SetActive(false);
            }
            foreach (var item in tools)
            {
                item.SetActive(true);
            }
        }
        public void selectIngs()
        {
            placementType = 1;
            foreach (var item in tools)
            {
                item.SetActive(false);
            }
            foreach (var item in ings)
            {
                item.SetActive(true);
            }
        }
        List<GameObject> tools;
        List<GameObject> ings;
        public void initPlace()
        {
            tools = new List<GameObject>();
            ings = new List<GameObject>();
            int i = 0;
            foreach (var item in PlacementManager.instance.tools)
            {
                GameObject g = Instantiate(itemBtn, selectPanel.transform);
                g.transform.localScale = Vector3.one;
                g.GetComponent<PlaceSelect>().myValue = i++;
                g.GetComponent<PlaceSelect>().PlayerReadyButton.GetComponentInChildren<Text>().text = item.objectName;
                tools.Add(g);
            }
            i = 0;
            foreach (var item in PlacementManager.instance.ings)
            {
                GameObject g = Instantiate(itemBtn, selectPanel.transform);
                g.transform.localScale = Vector3.one;
                g.GetComponent<PlaceSelect>().myValue = i++;
                g.GetComponent<PlaceSelect>().PlayerReadyButton.GetComponentInChildren<Text>().text = item.objectName;
                ings.Add(g);

                g.SetActive(false);
            }

        }
        #endregion

        #region Private Methods

        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("1_InGame");
        }
        private bool CheckPlayersReady()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return false;
            }

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object isPlayerReady;
                if (p.CustomProperties.TryGetValue(WellDone.PLAYER_READY, out isPlayerReady))
                {
                    if (!(bool)isPlayerReady)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        #endregion




    }
}