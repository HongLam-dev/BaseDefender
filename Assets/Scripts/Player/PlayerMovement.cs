using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    private Rigidbody rb;
    private Vector3 movementVelocity;

    protected void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        movementVelocity = moveDirection * moveSpeed;

        Vector3 movement= movementVelocity * speedMultiplier + externalVelocity;
        rb.linearVelocity = new Vector3(movement.x , rb.linearVelocity.y, movement.z);

        externalVelocity = Vector3.zero;
    }

    public override void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

}
