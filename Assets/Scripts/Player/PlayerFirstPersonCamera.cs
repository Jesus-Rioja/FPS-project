using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFirstPersonCamera : MonoBehaviour
{
    [SerializeField][Range(0f, 180f)] float maxLookAngle = 60f;
    [SerializeField] float mouseSensitivityY = -0.0001f;
    //float oldMousePositionY;

    void Awake()
    {
        //oldMousePositionY = Input.mousePosition.y;
    }

    void Update()
    {
        float mouseDelta = Input.GetAxis("Mouse Y");
        if(mouseDelta != 0f)
        {
            float mouseSpeed = (mouseDelta / Screen.height) / Time.deltaTime;

            Vector3 forwardOnPlane = Vector3.ProjectOnPlane(transform.forward, Vector3.up);
            float angle = Vector3.SignedAngle(forwardOnPlane, transform.forward, transform.right);
            float angleToApply = mouseSensitivityY * mouseSpeed;

            if ((angle + angleToApply) > maxLookAngle) { angleToApply += (maxLookAngle - (angle + angleToApply)); }
            else if ((angle + angleToApply) < -maxLookAngle) { angleToApply += (-maxLookAngle - (angle + angleToApply)); }

            Quaternion rotationToApply = Quaternion.AngleAxis(angleToApply, transform.right);
            transform.rotation = rotationToApply * transform.rotation;

            //oldMousePositionY = Input.mousePosition.y;
        }

    }
}
