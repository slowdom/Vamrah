using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;

public class PlayerInputManager : MonoBehaviour
{
    private Camera camera;
    private PlayerController playerController;
    private PhotonView PV;

    private bool jumping;
    private bool sprinting;
    private bool crouching;
    private Vector2 rawInputs;
    private Vector3 Cameraforward;

    private void Awake()
    {
        camera = Camera.main;
        playerController = GetComponent<PlayerController>();
        PV = GetComponent<PhotonView>();

        if (!PV.IsMine)
        {
        //    this.enabled = false;
        }
    }

    private void Update()
    {
        float delta = Time.deltaTime;

        Debug.Log(Input.GetAxis("Vertical"));

        GetKeys();
        SetMovementInputs(camera.transform.forward);
        playerController.Tick(Cameraforward, rawInputs, sprinting, crouching, delta);
    }

    private void GetKeys()
    {
        rawInputs.y = Input.GetAxis("Vertical");
        rawInputs.x = Input.GetAxis("Horizontal");

        crouching = Input.GetKey(KeyCode.LeftControl);
        sprinting = Input.GetKey(KeyCode.LeftShift);
        jumping = Input.GetKeyDown(KeyCode.Space);
    }

    void SetMovementInputs(Vector3 cameraForward)
    {
        Vector3 cameraForwardNorm = Vector3.Scale(cameraForward, new Vector3(1, 0, 1)).normalized;
        Vector3 cameraRight = -Vector3.Cross(cameraForward, Vector3.up);

        Cameraforward = rawInputs.y * cameraForwardNorm + rawInputs.x * cameraRight;
    }
}
