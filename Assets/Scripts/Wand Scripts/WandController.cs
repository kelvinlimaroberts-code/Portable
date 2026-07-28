using UnityEngine;

public class WandController : MonoBehaviour
{
    //shooting
    private PlayerInputController _playerInputController;
    private PortalController _portalController;
    public LayerMask BloodWoodLayer;
    private Transform _head;
    private RaycastHit hit;
    private enum portalColour {yellow, purple}

    void Awake()
    {
        //get components and objects needed 
        _playerInputController = GetComponent<PlayerInputController>();
        _portalController = FindFirstObjectByType<PortalController>();
        _head = GetComponent<PlayerController>().Head;
        //subscribe to events
        _playerInputController.OnYellowPortalShot += YellowPortalShot; //subscribe to yellow portal shot event
        _playerInputController.OnPurplePortalShot += PurplePortalShot; //subscribe to purple portal shot event
    }


    private void YellowPortalShot()
    {
        if(HitDetection())
        {
            ConjurePortal(portalColour.yellow);
        }
    }
    
    private void PurplePortalShot()
    {
        if(HitDetection())
        {
            ConjurePortal(portalColour.purple);
        }
    }


    private bool HitDetection()
    {
        bool validSurfaceHit = false;
        // Does the ray intersect with the blood wood layer
        if (Physics.Raycast(_head.position, _head.forward, out hit, Mathf.Infinity, BloodWoodLayer))
        {
            validSurfaceHit = true;
        }
        return validSurfaceHit;
    }

    private void ConjurePortal(portalColour desiredColour)
    {
        if (desiredColour == portalColour.yellow)
        {
            _portalController.yellowPortal.transform.position = hit.point;
            _portalController.yellowPortal.transform.rotation = hit.transform.rotation;
        }
        else if (desiredColour == portalColour.purple)
        {
            _portalController.purplePortal.transform.position = hit.point;
            _portalController.purplePortal.transform.rotation = hit.transform.rotation;
        }
    }

}
