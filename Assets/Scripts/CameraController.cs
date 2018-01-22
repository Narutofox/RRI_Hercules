using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    //reference to an object to track
    public GameObject trackingObject;
    public new Camera camera;
    //distance of an object to track from the camera
   // private float cameraOffset;
    //private float rightBound;
    //private float leftBound;
    //private SpriteRenderer spriteBounds;
    //private float vertExtent;
    //private float horzExtent;
   // private bool outOfBounds;
    private Vector3 calculatedPosition;
    //private BoxCollider2D LeftBorder;
    //private Bounds CameraBounds;
    //private Camera CalculatedCamera;
    void Start()
    {
        //outOfBounds = false;
        // vertExtent = camera.orthographicSize;
        // horzExtent = vertExtent * Screen.width / Screen.height;
        //LeftBorder = GameObject.Find("LeftBorder").GetComponentInChildren<BoxCollider2D>(); ;
        //spriteBounds = GameObject.Find("BG").GetComponentInChildren<SpriteRenderer>();
        //leftBound = horzExtent + spriteBounds.sprite.bounds.min.x;
        //rightBound = spriteBounds.sprite.bounds.max.x - horzExtent;
        //cameraOffset = camera.transform.position.x - trackingObject.transform.position.x;
     
    }

    //FixedUpdate is called fixed times per second
    void FixedUpdate()
    {
        if (trackingObject == null)
        {
            return;
        }
        //CalculatedCamera = camera;
        //calculatedPosition = new Vector3(trackingObject.transform.position.x + cameraOffset, camera.transform.position.y, camera.transform.position.z);
        //CalculatedCamera.transform.position = calculatedPosition;
        //CameraBounds = OrthographicBounds(CalculatedCamera);

        //if ((CameraBounds.min.x < 0 && CameraBounds.min.x < LeftBorder.bounds.max.x) || (CameraBounds.min.x >= 0 && CameraBounds.min.x > LeftBorder.bounds.max.x))
        //{
        //    outOfBounds = true;
        //}
        //else
        //{
        //    outOfBounds = false;
        //}

        //if (outOfBounds == true)
        //    { 
                calculatedPosition =
                    new Vector3(trackingObject.transform.position.x, camera.transform.position.y, camera.transform.position.z);
            //}
            camera.transform.position = calculatedPosition;

    }

    private Bounds OrthographicBounds(Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }
}
