using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
namespace Com.CrossLab.WellDone
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback, IPunObservable
    {
        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        Transform cameraTransform;
        Vector3 tmpPos;
        Quaternion tmpRotaion;

        string playerName;

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            playerName = (string)PhotonView.Get(this).InstantiationData[0];
        }
        #region IPunObservable implementation
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(tmpPos);
                stream.SendNext(tmpRotaion);
            }
            else
            {
                // Network player, receive data
                this.tmpPos = (Vector3)stream.ReceiveNext();
                this.tmpRotaion = (Quaternion)stream.ReceiveNext();
            }
        }
        #endregion
        // Start is called before the first frame update
        void Awake()
        {
            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
            if (photonView.IsMine)
            {
                PlayerManager.LocalPlayerInstance = this.gameObject;
                cameraTransform = Camera.main.transform;
                gameObject.layer = LayerMask.NameToLayer("myAvatar");
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("myAvatar"); 
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {

            }
            else
            {
                tmpPos = cameraTransform.position;
                tmpRotaion = cameraTransform.rotation;
            }

            Vector3 pos = tmpPos;
            if(cameraTransform !=null)
            pos.y = cameraTransform.position.y;
            Quaternion rot = tmpRotaion;
            rot = Quaternion.Euler(0, rot.eulerAngles.y, 0);
            transform.position = pos;
            transform.rotation = rot;

        }
    }
}