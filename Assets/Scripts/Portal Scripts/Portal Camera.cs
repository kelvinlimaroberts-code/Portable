using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    public Transform thisPortal;
    public Transform otherPortal;
    private Transform playerCamera;
    private Vector3 playerOffsetFromPortal = new Vector3 (0, 0, 0);
    private float angularDifferenceBetweenPortalRotations = 0f;
    private Quaternion portalRotationalDifference;
    private Vector3 cameraDirection = new Vector3 (0,0,0);

    void Awake()
    {
        playerCamera = GameObject.FindWithTag("MainCamera").transform;
    }

    // Update is called once per frame
    void Update()
    {
        playerOffsetFromPortal = playerCamera.position - otherPortal.position;
        this.transform.position = thisPortal.position + playerOffsetFromPortal;
        //angularDifferenceBetweenPortalRotations = Quaternion.Angle(thisPortal.rotation, otherPortal.rotation);
        //portalRotationalDifference = Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
        //cameraDirection = portalRotationalDifference * playerCamera.forward;
        //this.transform.rotation = Quaternion.LookRotation(cameraDirection, Vector3.up);
    }
}
