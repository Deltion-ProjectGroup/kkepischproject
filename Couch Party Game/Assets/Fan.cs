using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    [SerializeField] float force = 1;
    // Start is called before the first frame update
    void Start()
    {
        foreach (string test in Input.GetJoystickNames())
        {
            Debug.Log(test);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.GetAxis("Test"));
        if (Input.GetButton("Test"))
        {
            Debug.Log("BUTTON HELD");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<Rigidbody>() != null)
        {
            Rigidbody rigid = other.GetComponent<Rigidbody>();
            rigid.AddForce(transform.forward * force, ForceMode.Acceleration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().decellerationBlocks++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().decellerationBlocks--;
        }
    }
}
