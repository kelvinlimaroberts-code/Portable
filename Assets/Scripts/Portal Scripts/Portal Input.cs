using UnityEngine;
using System;
public class PortalInput : MonoBehaviour
{
    public event Action ObjectEnteredPortal;
    public event Action ObjectExitedPortal;
    public GameObject TeleportSpot;
    public GameObject affectedObject { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        affectedObject = other.gameObject;
        ObjectEnteredPortal?.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        ObjectExitedPortal?.Invoke();
    }
}
