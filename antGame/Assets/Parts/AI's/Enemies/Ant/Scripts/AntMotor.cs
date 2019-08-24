using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Photon.Pun;

public class AntMotor : MonoBehaviour, IPunObservable
{
    private AntAnimations antAnimations;
    private NavMeshAgent agent;

    void Start()
    {
        antAnimations = GetComponent<AntAnimations>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void Tick(Vector3 desiredPosition)
    {
        agent.SetDestination(desiredPosition);
        antAnimations.Tick(agent);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && PhotonNetwork.IsMasterClient == true)
        {
            stream.SendNext(transform.rotation);
            stream.SendNext(transform.position);
        }
        if (PhotonNetwork.IsMasterClient == false)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, (Quaternion)stream.ReceiveNext(), Time.deltaTime * 20);
            transform.position = Vector3.Lerp(transform.position, (Vector3)stream.ReceiveNext(), Time.deltaTime * 20);
        }
    }
}
