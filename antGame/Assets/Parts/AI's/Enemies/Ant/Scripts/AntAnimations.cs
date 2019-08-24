using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;
using Photon.Pun;                                                                   

public class AntAnimations : MonoBehaviour, IPunObservable
{
    public float deadZone;
    public float forwardDamp;
    public float turningDamp;

    private float speed;
    private float angle;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Tick(NavMeshAgent agent)
    {
        GetParameters(agent);
        SetupAnimator();
    }

    void GetParameters(NavMeshAgent agent)
    {
        speed = Vector3.Project(agent.desiredVelocity, transform.forward).magnitude;
        angle = FindAngle(transform.forward, agent.desiredVelocity, transform.up);

        if (Mathf.Abs(angle) < deadZone)
        {
            transform.LookAt(transform.position + agent.desiredVelocity);
            angle = 0f;
        }
    }

    float FindAngle (Vector3 fromVector, Vector3 toVector, Vector3 upVector)
    {
        if (toVector == Vector3.zero)
            return 0f;

        float angle = Vector3.Angle(fromVector, toVector);
        Vector3 normal = Vector3.Cross(fromVector, toVector);
        angle *= Mathf.Sign(Vector3.Dot(normal, upVector));
        angle *= Mathf.Deg2Rad;

        return angle;
    }

    void SetupAnimator()
    {
        anim.SetFloat("Forward", speed, forwardDamp, Time.deltaTime);
        anim.SetFloat("Turn", angle, turningDamp, Time.deltaTime);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && PhotonNetwork.IsMasterClient == true)
        {
            stream.SendNext(anim.GetFloat("Forward"));
            stream.SendNext(anim.GetFloat("Turn"));
        }
        if (PhotonNetwork.IsMasterClient == false)
        {
            anim.SetFloat("Forward", (float)stream.ReceiveNext(), Time.deltaTime, 0.2f);
            anim.SetFloat("Turn", (float)stream.ReceiveNext(), Time.deltaTime, 0.2f);
        }
    }
}
