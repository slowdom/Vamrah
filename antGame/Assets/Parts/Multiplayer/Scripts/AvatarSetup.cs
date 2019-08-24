using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSetup : MonoBehaviour
{
 /*   private PhotonView PV;
    public int characterValue;
    public MeshRenderer myCharacterRenderer;

    private int myCharacter;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (PV.IsMine)
        {
            PV.RPC("RPC_AddCharacter", RpcTarget.AllBuffered, PlayerInfo.PI.mySelectedCharacter);
        }
    }

    [PunRPC]
    void RPC_AddCharacter(int whichCharacter)
    {
        myCharacter = whichCharacter;
        characterValue = whichCharacter;
        myCharacterRenderer.material.color = PlayerInfo.PI.allColors[whichCharacter];
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(myCharacter);
        }
        else
        {
            myCharacter = (int)stream.ReceiveNext();

            myCharacterRenderer.material.color = PlayerInfo.PI.allColors[myCharacter];
        }
    }*/
}
