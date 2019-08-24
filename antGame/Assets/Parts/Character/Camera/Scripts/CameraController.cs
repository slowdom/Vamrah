using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform camera;
    public Transform followTarget;

    [Header("CameraCollision")]
    public LayerMask mask;
    public float bumperRange = 0.35f;
    public float cameraCollisionDamp = 15;
    public float maxDistance = -11;
    public float minDistance = -1;

    [Header("Dolly")]
    public float damp = 5f;
    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    [Range(0, -180)]
    public float minimumY = -60F;
    [Range(0, 180)]
    public float maximumY = 60F;

    private float rotationX = 0;
    private float rotationY = 0;

    private float MouseX;
    private float MouseY;

    private void Update()
    {
        Camera();
        Dolly();
        GetInputs();   
    }

    private void Camera()
    {
        float targetZ = maxDistance;
        float actualZ = targetZ;

        CameraCollision(targetZ, ref actualZ);

        Vector3 targetP = camera.localPosition;
        targetP.z = Mathf.Lerp(targetP.z, actualZ, Time.deltaTime * cameraCollisionDamp);
        camera.localPosition = targetP;
    }

    private void GetInputs()
    {
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");
    }

    private void CameraCollision(float targetZ, ref float actualZ)
    {
        float step = Mathf.Abs(maxDistance);
        int stepCount = 2;
        float stepIcremental = step / stepCount;

        RaycastHit hit;
        Vector3 origin = transform.position;
        Vector3 direction = -transform.forward;
        Debug.DrawRay(origin, direction * step, Color.blue);

        if (Physics.Raycast(origin, direction, out hit, step, mask))
        {
            float distance = Vector3.Distance(hit.point, origin);
            actualZ = -(distance / 2);
        }
        else
        {
            for (int s = 0; s < stepCount + 1; s++)
            {
                for (int i = 0; i < 4; i++)
                {
                    Vector3 dir = Vector3.zero;
                    Vector3 secondOrigin = origin + (direction * s) * stepIcremental;

                    switch (i)
                    {
                        case 0:
                            dir = camera.right;
                            break;
                        case 1:
                            dir = -camera.right;
                            break;
                        case 2:
                            dir = camera.up;
                            break;
                        case 3:
                            dir = -camera.up;
                            break;
                        default:
                            break;
                    }

                    Debug.DrawRay(secondOrigin, dir * bumperRange, Color.red);
                    if (Physics.Raycast(secondOrigin, dir, out hit, bumperRange, mask, QueryTriggerInteraction.Ignore))
                    {
                        float distance = Vector3.Distance(secondOrigin, origin);
                        actualZ = -(distance / 2);
                    }
                }
            }
        }
    }

    private void Dolly()
    {
        if (followTarget == null)
            return;

        rotationX += MouseX * sensitivityX;
        rotationY += MouseY * sensitivityY;
        rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

        Quaternion lookDirection = Quaternion.Euler(-rotationY, rotationX, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookDirection, Time.deltaTime * damp);
        transform.position = Vector3.Lerp(transform.position, followTarget.position, Time.deltaTime * damp);
    }
}
