using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Vector3 startPosition;
    public Quaternion startRotation;
    bool isHolding = false;
    BoxCollider boxCollider;
    Rigidbody hammerRigidbody;
    private void Start()
    {
        startPosition = transform.localPosition;
        startRotation = transform.rotation;
        boxCollider = transform.GetComponent<BoxCollider>();
        boxCollider.isTrigger = false;
        hammerRigidbody = transform.GetComponent<Rigidbody>();
    }

    public void HoldWeapon()
    {
        boxCollider.isTrigger = true;
        isHolding = true;
    }

    public void ThrowWeapon()
    {
        isHolding = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("mole"))
        {
            GameObject mole = other.gameObject;
            GameController.instance.DestroyMole(mole);
        }
        else if (other.CompareTag("ground") && isHolding == false)
        {
            Respawn();
        }
    }


    void Respawn()
    {
        hammerRigidbody.velocity = Vector3.zero;
        hammerRigidbody.angularVelocity = Vector3.zero;
        boxCollider.isTrigger = false;
        transform.localPosition = startPosition;
        transform.rotation = transform.rotation;
    }
}
