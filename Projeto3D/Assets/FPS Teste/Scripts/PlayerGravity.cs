using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    bool onGround;
    Vector3 velocity;

    const float jumpHeight = 1.5f;

    [SerializeField] Vector3 offset;
    [SerializeField] float radius;

    CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        onGround = Physics.CheckSphere(transform.position + offset, radius);

        if (onGround && velocity.y < 0)
        {
            velocity.y = 0;
        }
        else if(!onGround)
        {
            velocity += Physics.gravity * Time.deltaTime;
        }
            characterController.Move(velocity * Time.deltaTime);
    }

    public void Jump()
    {
        if (onGround)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + offset, radius) ;
    }
}
