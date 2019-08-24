using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public enum SearchStates { idle, sawSomeone, heardSomething };

public class AntSense : MonoBehaviour
{
    [SerializeField] public float killDistance;
    [SerializeField] public float fieldOfView;

    [SerializeField]
    private SearchStates searchStates;

    private Vector3 allertedPosition;

    private SphereCollider sphereCollider;
    private AntNavigation navigation;

    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        navigation = GetComponent<AntNavigation>();
    }

    public void Update()
    {
        if (PhotonNetwork.IsMasterClient == false)
        {
            GetComponent<NavMeshAgent>().enabled = false;
            return;
        }

        navigation.Tick(searchStates, allertedPosition);

        searchStates = SearchStates.idle;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.DrawRay(transform.position, transform.forward);

            if (Vector3.Angle(transform.forward, other.transform.position - transform.position) < fieldOfView)
            {
                RaycastHit hit;

                if (Physics.Linecast(transform.position + Vector3.up, other.transform.position + Vector3.up / 2, out hit))
                {
                    if (hit.transform == other.transform)
                    {
                        allertedPosition = other.transform.position;
                        searchStates = SearchStates.sawSomeone;

                        if (Vector3.Distance(transform.position, other.transform.position) < killDistance)
                        {
                            if (other.GetComponent<PhotonView>().IsMine == true)
                            {
                                GameManager.gameManager.Die(other.transform);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            searchStates = SearchStates.idle;
        }
    }
}
