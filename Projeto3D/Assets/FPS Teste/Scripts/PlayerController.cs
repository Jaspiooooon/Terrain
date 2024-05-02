using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    PlayerGravity playerGravity;
    Transform camTransform;
    Weapon weapon;
    Vector3 direction;
    Vector2 camDirection;
    
    const float speed = 5;
    float verticalRotation;
    bool shooting, crouching;

    [SerializeField] GameObject grenade;
    [SerializeField] Transform throwPoint;

    [SerializeField] [Range(1, 500)]
    float mouseSensitivity;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        characterController = GetComponent<CharacterController>();
        camTransform = transform.GetChild(0).GetComponent<Transform>();
        playerGravity=GetComponent<PlayerGravity>();
        weapon = GetComponentInChildren<Weapon>();

    }

    private void Update()
    {
        direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");
        camDirection.x = Input.GetAxis("Mouse X");
        camDirection.y = Input.GetAxis("Mouse Y");


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            ThrowGrenade();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            crouching = !crouching;
            Crouch();
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
           shooting = true;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            shooting = false;
        }

        Movement();
        Rotation();
        Fire();
    }

    private void Movement()
    {
        float newSpeed = crouching ? speed/2 : speed;
        Vector3 move = transform.right * direction.x + transform.forward * direction.z;
        characterController.Move(move * newSpeed * Time.deltaTime);
    }

    private void Rotation()
    {
        camDirection *= mouseSensitivity * Time.deltaTime;

        verticalRotation -= camDirection.y;
        verticalRotation = Mathf.Clamp(verticalRotation, -90, 90);

        camTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        transform.Rotate(Vector3.up * camDirection.x);
    }

    private void Fire()
    {
        if(shooting)
        {
            weapon.Fire(crouching);
        }
    }

    private void Jump()
    {
        playerGravity.Jump();
    }

    private void Crouch()
    {
        if (crouching)
        {
            camTransform.localPosition = Vector3.zero;
        }
        else
        {
            camTransform.localPosition = new Vector3(0, 0.5f, 0);
        }
    }

    private void ThrowGrenade()
    {
        Rigidbody grenadeRb = Instantiate(grenade, throwPoint.position, camTransform.rotation).GetComponent<Rigidbody>();
        Vector3 throwForce = camTransform.forward * 10 + transform.up * 5;
        grenadeRb.AddForce(throwForce, ForceMode.Impulse);
        Destroy(grenadeRb.gameObject, 10);
    }
}
