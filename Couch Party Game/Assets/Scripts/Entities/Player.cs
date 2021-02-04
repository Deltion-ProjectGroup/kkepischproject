using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [Header("Player")]
    public float movementSpeed = 1;
    [SerializeField] float rotateSpeed = 1;
    [SerializeField] float dashVelocity; //Power of the dash
    [SerializeField] float interactDelay; //Delay before u can interact again
    bool isDashing;
    [SerializeField] bool canInteract = true;
    //public Interactable currentUsingInteractable; //The current interactable the player is using or holding
    public Role role; //The role of the player, this is used for where the player needs to be spawned

    [Header("Interaction")]
    [SerializeField] string interactButton;
    Transform interactionBox; //Box that checks if an interactable is inside
    [SerializeField] LayerMask interactableLayers;
    //Interactable nearestInteractable;

    private void Update()
    {
        Movement();
        Interact();
    }

    void Movement()
    {
        Vector3 movementAmount = new Vector3(Input.GetAxisRaw("Horizontal"), 0,  Input.GetAxisRaw("Vertical"));

        movementAmount = movementAmount.normalized;


        if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movementAmount);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        }

        movementAmount = movementAmount * movementSpeed * Time.deltaTime;

        transform.Translate(movementAmount, Space.World);
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

    public enum Role { TestRole}
}
