using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("Player")]
    public float movementSpeed = 1;
    [SerializeField] float maxVelocity = 1;
    public int decellerationBlocks;
    [SerializeField] float decellerationSpeed = 1;
    [SerializeField] float rotateSpeed = 1;
    [SerializeField] float dashVelocity; //Power of the dash
    [SerializeField] float interactDelay; //Delay before u can interact again
    bool isDashing;
    [SerializeField] bool canInteract = true;
    //public Interactable currentUsingInteractable; //The current interactable the player is using or holding
    public Role role; //The role of the player, this is used for where the player needs to be spawned

    [Header("Slopes")]
    [SerializeField] float maxAngle;
    [SerializeField] float slopeBoosterModifier = 1;
    [SerializeField] Transform slopeCheckOrigin;
    [SerializeField] float rayRange;
    [SerializeField] LayerMask slopeMask;

    [Header("Camera")]
    public Transform playerCamera;
    [SerializeField] bool followX = true, followZ = true;
    [SerializeField] float zDistance;
    [SerializeField] float followSpeed;

    [Header("Interaction")]
    [SerializeField] string interactButton;
    Transform interactionBox; //Box that checks if an interactable is inside
    [SerializeField] LayerMask interactableLayers;
    //Interactable nearestInteractable;

    private void Update()
    {
        Interact();
        CameraFollow();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        Vector3 movementAmount = new Vector3(Input.GetAxisRaw("Horizontal"), 0,  Input.GetAxisRaw("Vertical"));

        movementAmount = movementAmount.normalized;

        Vector3 movementAmountRaw = movementAmount;

        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {

            movementAmount = movementAmount * movementSpeed * Time.fixedDeltaTime;

            Vector3 newVelocity = thisRigid.velocity + movementAmount;

            Vector3 actualMaxVelocity = newVelocity.normalized;
            actualMaxVelocity *= maxVelocity;

            if(newVelocity.x < -actualMaxVelocity.x)
            {
                newVelocity.x = -actualMaxVelocity.x;
            }
            if(newVelocity.x > actualMaxVelocity.x)
            {
                newVelocity.x = actualMaxVelocity.x;
            }

            if (newVelocity.z < -actualMaxVelocity.z)
            {
                newVelocity.z = -actualMaxVelocity.z;
            }
            if (newVelocity.z > actualMaxVelocity.z)
            {
                newVelocity.z = actualMaxVelocity.z;
            }

            newVelocity = newVelocity - thisRigid.velocity;

            thisRigid.AddForce(newVelocity, ForceMode.Acceleration);

            Vector3 velocityDirection = thisRigid.velocity;
            velocityDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(velocityDirection.normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.fixedDeltaTime);

            RaycastHit hitData;

            if(Physics.Raycast(slopeCheckOrigin.position, movementAmountRaw, out hitData, rayRange, slopeMask))
            {
                float angle = Vector3.Angle(transform.up, hitData.transform.up);
                Debug.Log("Angle = " + angle);
                float yDifference = hitData.point.y - transform.position.y;
                transform.Translate(new Vector3(0, yDifference * slopeBoosterModifier, 0));
            }
        }
        else
        {
            if(decellerationBlocks <= 0)
            {
                if (thisRigid.velocity.x < -0.1 || thisRigid.velocity.x > 0.1f || thisRigid.velocity.z < -0.1 || thisRigid.velocity.z > 0.1f)
                {
                    Vector3 velocityDecrease = Vector3.zero;

                    if (thisRigid.velocity.x > 0)
                    {
                        velocityDecrease.x = decellerationSpeed * Time.fixedDeltaTime > thisRigid.velocity.x ? thisRigid.velocity.x : decellerationSpeed * Time.fixedDeltaTime;
                    }
                    if (thisRigid.velocity.z > 0)
                    {
                        velocityDecrease.z = decellerationSpeed * Time.fixedDeltaTime > thisRigid.velocity.z ? thisRigid.velocity.z : decellerationSpeed * Time.fixedDeltaTime;
                    }

                    if (thisRigid.velocity.x < 0)
                    {
                        velocityDecrease.x = -decellerationSpeed * Time.fixedDeltaTime < thisRigid.velocity.x ? thisRigid.velocity.x : -decellerationSpeed * Time.fixedDeltaTime;
                    }
                    if (thisRigid.velocity.z < 0)
                    {
                        velocityDecrease.z = -decellerationSpeed * Time.fixedDeltaTime < thisRigid.velocity.z ? thisRigid.velocity.z : -decellerationSpeed * Time.fixedDeltaTime;
                    }

                    thisRigid.velocity -= velocityDecrease;
                }
            }
        }
    }

    void CheckDash()
    {

    }

    void PerformDash()
    {

    }

    bool CheckInteract() //Checks if the player is near something that they can interact with
    {
        /*Collider[] hitColliders = Physics.OverlapBox(interactionBox.position, interactionBox.lossyScale / 2, interactionBox.rotation, interactableLayers);

        if(hitColliders.Length > 0)
        {
            Interactable closestInteractable;
            float closestDistance = float.MaxValue;

            foreach(Collider col in hitColliders)
            {
                Interactable interactable = col.GetComponent<Interactable>();
                if (interactable.canInteract)
                {
                    float distance = Vector3.Distance(col.transform.position, transform.position);
                    if (distance < closestDistance || closestInteractable == null)
                    {
                        closestInteractable = interactable;
                        closestDistance = distance;
                    }
                }
            }

            nearestInteractable = closestInteractable;
            return nearestInteractable != null;
        }*/
        return false;
    }

    void Interact()
    {
        if(CheckInteract() && canInteract && Input.GetButtonDown(interactButton))
        {
            //nearestInteractable.Interact(this);
        }
    }

    public void FinishedInteract()
    {
        if(interactDelay > 0)
        {
            StartCoroutine(InteractDelay());
        }
        else
        {
            canInteract = true;
        }
    }

    IEnumerator InteractDelay()
    {
        yield return new WaitForSeconds(interactDelay);
        canInteract = true;
    }

    void CameraFollow()
    {
        Vector3 targetPosition = playerCamera.transform.position;
        if (followX)
        {
            targetPosition.x = transform.position.x;
        }
        if (followZ)
        {
            float distanceToMoveOnZ = playerCamera.transform.position.z - transform.position.z <= 0 ? -1 * zDistance : 1 * zDistance;
            targetPosition.z = transform.position.z + distanceToMoveOnZ;
        }
        playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, targetPosition, followSpeed * Time.deltaTime);
    }

    public enum Role { TestRole}
}
