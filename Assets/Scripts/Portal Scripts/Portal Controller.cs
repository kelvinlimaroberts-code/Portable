using UnityEngine;

public class PortalController : MonoBehaviour
{
    public GameObject yellowPortal;
    public GameObject purplePortal;
    private PortalInput _yellowPortalInput;
    private PortalInput _purplePortalInput;
    private bool yellowCanTeleport = true;
    private bool purpleCanTeleport = true;

    void Awake()
    {
        //get components needed
        _yellowPortalInput = yellowPortal.GetComponent<PortalInput>();
        _purplePortalInput = purplePortal.GetComponent<PortalInput>();
        //subscribe to collision detection events
        _yellowPortalInput.ObjectEnteredPortal += YellowEntered;
        _yellowPortalInput.ObjectExitedPortal += YellowExited;
        _purplePortalInput.ObjectEnteredPortal += PurpleEntered;
        _purplePortalInput.ObjectExitedPortal += PurpleExited;
    }

    private void YellowEntered()
    {
        if (yellowCanTeleport)
        {
            //teleport to purple 
            purpleCanTeleport = false; //prevent constant swap
            //change position
            _yellowPortalInput.affectedObject.transform.position = _purplePortalInput.TeleportSpot.transform.position; //change position of object
            
            if (_yellowPortalInput.affectedObject.tag == "Player")
            {
                //change rotation of player like this, to be intergrated with the cam/head rotation system
                Vector3 PortRot;
                PortRot.x = _purplePortalInput.TeleportSpot.transform.eulerAngles.x;
                PortRot.y = _purplePortalInput.TeleportSpot.transform.eulerAngles.y;
                FindFirstObjectByType<PlayerController>().EditRotation(PortRot.x, PortRot.y);

                //alter momentum to be in the new direction
                FindFirstObjectByType<PlayerController>().EditLinearMomentum(_purplePortalInput.TeleportSpot.transform.forward);
            }
            else
            {
                //change rotation of object normally
                _yellowPortalInput.affectedObject.transform.rotation = _purplePortalInput.TeleportSpot.transform.rotation;
            }
        }
    }
    private void YellowExited()
    {
        //after the object has fully left the portal, then it can teleport again
        yellowCanTeleport = true;
    }

    private void PurpleEntered()
    {
        if (purpleCanTeleport)
        {
            //teleport to yellow 
            yellowCanTeleport = false; //prevent constant swap
            //change position
            _purplePortalInput.affectedObject.transform.position = _yellowPortalInput.TeleportSpot.transform.position; //change position
            
            if (_purplePortalInput.affectedObject.tag == "Player")
            {
                //change rotation of player like this, to be intergrated with the cam/head rotation system
                Vector3 PortRot;
                PortRot.x = _yellowPortalInput.TeleportSpot.transform.eulerAngles.x;
                PortRot.y = _yellowPortalInput.TeleportSpot.transform.eulerAngles.y;
                FindFirstObjectByType<PlayerController>().EditRotation(PortRot.x, PortRot.y);

                //alter momentum to be in the new direction
                FindFirstObjectByType<PlayerController>().EditLinearMomentum(_yellowPortalInput.TeleportSpot.transform.forward);
            }
            else
            {
                //change rotation of object normally
                _purplePortalInput.affectedObject.transform.rotation = _yellowPortalInput.TeleportSpot.transform.rotation;
            }
        }
    }
    private void PurpleExited()
    {
        //after the object has fully left the portal, then it can teleport again
        purpleCanTeleport = true; 
    }
}
