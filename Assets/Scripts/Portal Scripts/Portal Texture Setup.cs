using UnityEngine;

public class PortalTextureSetup : MonoBehaviour
{
    public Camera yellowCam;
    public Camera purpleCam;
    public Material yellowDisplayMat;
    public Material purpleDisplayMat;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //set up the purple display with the yellow cam
        if (yellowCam.targetTexture != null)
        {
            yellowCam.targetTexture.Release();
        }
        yellowCam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        purpleDisplayMat.mainTexture = yellowCam.targetTexture;

        //set up the yellow display with the purple cam
        if(purpleCam.targetTexture != null)
        {
            purpleCam.targetTexture.Release();
        }
        purpleCam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        yellowDisplayMat.mainTexture = purpleCam.targetTexture;
    }


}
