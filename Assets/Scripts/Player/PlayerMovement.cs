using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour, TargetWithLifeThatNotifies.IDeathNotifiable
{
    public static PlayerMovement instance;

    [SerializeField] LayerMask layerMaskAimingDetection;
    [SerializeField] float moveSpeed = 4f;

    [Header("FPS rotation")]
    [SerializeField] bool rotateWithMouse = false;
    [SerializeField] float mouseSensitivityX = 0.0001f;


    [SerializeField] bool orientateToCamera = false;
    [SerializeField] bool rotateToCamera;


    CharacterController characterController;
    Animator anim;

    Vector3 movementFromInput;
    public Vector3 movementOnPlane;

    float gravity = -9.8f;
    float speedY;
    bool isDead = false;

    public bool movementAllowed = true;
    public bool canRun = false;

    float oldMousePositionX;

    void Awake()
    {
        instance = this;
        Time.timeScale = 1.0f;
        characterController = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        movementAllowed = true;

        oldMousePositionX = Input.mousePosition.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        if (movementAllowed)
        {
            UpdateMovement();
            UpdateOrientation();
        }

    }


    void UpdateMovement()
    {
        movementFromInput = Vector3.zero;
        if (Input.GetKey(KeyCode.A)) { movementFromInput += Vector3.left; }
        if (Input.GetKey(KeyCode.W)) { movementFromInput += Vector3.forward; }
        if (Input.GetKey(KeyCode.S)) { movementFromInput += Vector3.back; }
        if (Input.GetKey(KeyCode.D)) { movementFromInput += Vector3.right; }
        movementFromInput.Normalize();

        movementOnPlane = Camera.main.transform.TransformDirection(movementFromInput);
        movementOnPlane = Vector3.ProjectOnPlane(movementOnPlane, Vector3.up);
        movementOnPlane.Normalize();

        speedY += gravity * Time.deltaTime;
        movementOnPlane.y = speedY;

        if (Input.GetKey(KeyCode.LeftShift) && canRun)
        {
            //playerShooting.currentWeapon.isUsable = false;

            characterController.Move(movementOnPlane * moveSpeed * 2.5f * Time.deltaTime);

            //if (playerShooting.currentWeapon.canShootContinuously) { playerShooting.currentWeapon.StopShooting(); }
        }
        else
        {
            //playerShooting.currentWeapon.isUsable = true;

            characterController.Move(movementOnPlane * moveSpeed * Time.deltaTime);
        }

        if (characterController.isGrounded) { speedY = 0; }
    }


    void UpdateOrientation()
    {
        if (rotateWithMouse)
        {
            UpdateOrientationWithMouse();
        }
        else if (rotateToCamera)
        {
            UpdateOrientationToCamera();
        }
        else
        {
            UpdateOrientationToMouse();
        }
    }

    private void UpdateOrientationToCamera()
    {
        Vector3 desiredForward = Vector3.zero;

        if (movementFromInput.sqrMagnitude > (0.01f * 0.01f))
        {
            desiredForward = Camera.main.transform.forward;
            PerformOrientation(desiredForward);
        }
    }

    private void UpdateOrientationToMouse()
    {
        Vector3 desiredForward = Vector3.zero;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMaskAimingDetection))
        {
            desiredForward = hit.point - transform.position;
            PerformOrientation(desiredForward);
        }
    }

    private void UpdateOrientationWithMouse()
    {
        float mouseDelta = Input.GetAxis("Mouse X");
        float mouseSpeed = (mouseDelta / Screen.width) / Time.deltaTime;

        Quaternion rotationToApply = Quaternion.AngleAxis(mouseSpeed * mouseSensitivityX, Vector3.up);
        transform.rotation = rotationToApply * transform.rotation;

        oldMousePositionX = Input.mousePosition.x;
    }

    void PerformOrientation(Vector3 desiredForward)
    {
        desiredForward = Vector3.ProjectOnPlane(desiredForward, Vector3.up);
        desiredForward.Normalize();

        Quaternion desiredRotation = Quaternion.LookRotation(desiredForward, Vector3.up);
        Quaternion currentRotation = transform.rotation;

        transform.rotation = Quaternion.Lerp(currentRotation, desiredRotation, 0.1f);
    }

    void TargetWithLifeThatNotifies.IDeathNotifiable.NotifyDeath()
    {
        if (!isDead)
        {
            isDead = true;
            anim.SetTrigger("Death");
            movementAllowed = false;
            GetComponent<CapsuleCollider>().enabled = false;

            Invoke("Death", 2f);
        }
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}
