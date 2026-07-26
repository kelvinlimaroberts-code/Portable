using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PickUp : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public LayerMask playerLayer;
    private bool _isHeld = false;
    private Transform heldPosition;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void BeingHeld(bool isHeld, Transform? objectHolder = null)
    {
        _isHeld = isHeld;
        if (objectHolder != null)
        {
            heldPosition = objectHolder;
        }
        if (_isHeld) 
        {
            _rigidbody.useGravity = !_isHeld;
            _rigidbody.constraints = RigidbodyConstraints.FreezePosition;
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _rigidbody.excludeLayers = playerLayer;  //removes collisions to player, otherwise player would fly away when coliding with held item
        }
        else
        {
            _rigidbody.useGravity = !_isHeld;
            _rigidbody.constraints = RigidbodyConstraints.None;
            _rigidbody.excludeLayers = 0;
        }
    }
    void FixedUpdate()
    {
        if (_isHeld)
        {
            this.transform.position = heldPosition.position;
            this.transform.rotation = heldPosition.rotation;
        }
    }

    public void EditLinearMomentum(Vector3 newDir)
    {
        _rigidbody.linearVelocity = newDir * (_rigidbody.linearVelocity.x + _rigidbody.linearVelocity.y + _rigidbody.linearVelocity.z);
    }
}
