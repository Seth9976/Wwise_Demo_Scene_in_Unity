using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class ZoomCamera : MonoBehaviour
{
    //////////////////////////////////////////////////////////////
    // ZoomCamera.js
    // Penelope iPhone Tutorial
    //
    // ZoomCamera is a simple camera that uses a zoom value to zoom 
    // the camera in or out relatively from the default position set
    // in the editor. It can snap to zoom values when moving closer
    // to the specified origin and smoothly seeks when moving farther
    // away. The camera checks for any objects that obstruct the view
    // of the camera to the origin and snaps to be in front of those
    // locations.
    //////////////////////////////////////////////////////////////
    public Transform origin; // What is considered the origin to the camera
    public float zoom;
    public float zoomMin;
    public float zoomMax;
    public float seekTime;
    public bool smoothZoomIn;
    private Vector3 defaultLocalPosition;
    private Transform thisTransform;
    private float currentZoom;
    private float targetZoom;
    private float zoomVelocity;
    public virtual void Start()
    {
         // Cache component instead of looking it up every frame
        this.thisTransform = this.transform;
        // The default position is the position that is set in the editor
        this.defaultLocalPosition = this.thisTransform.localPosition;
        // Default the current zoom to what was set in the editor 
        this.currentZoom = this.zoom;
    }

    public virtual void Update()
    {
        RaycastHit hit = default(RaycastHit);
         // The zoom set externally must still be within the min-max range
        this.zoom = Mathf.Clamp(this.zoom, this.zoomMin, this.zoomMax);
        // Only collide with non-Player (8) layers
        int layerMask = ~((1 << 8) | (1 << 2));
        Vector3 start = this.origin.position;
        Vector3 zoomedPosition = this.defaultLocalPosition + this.thisTransform.parent.InverseTransformDirection(this.thisTransform.forward * this.zoom);
        Vector3 end = this.thisTransform.parent.TransformPoint(zoomedPosition);
        // Cast a line from the origin transform to the camera and find out if we hit anything in-between
        if (Physics.Linecast(start, end, out hit, layerMask))
        {
             // We hit something, so translate this to a zoom value
            Vector3 position = hit.point + this.thisTransform.TransformDirection(Vector3.forward);
            Vector3 difference = position - this.thisTransform.parent.TransformPoint(this.defaultLocalPosition);
            this.targetZoom = difference.magnitude;
        }
        else
        {
            // We didn't hit anything, so the camera should use the zoom set externally
            this.targetZoom = this.zoom;
        }
        // Clamp target zoom to our min-max range
        this.targetZoom = Mathf.Clamp(this.targetZoom, this.zoomMin, this.zoomMax);
        if (!this.smoothZoomIn && ((this.targetZoom - this.currentZoom) > 0))
        {
             // Snap the current zoom to our target if it is closer. This is useful if
             // some object is between the camera and the origin
            this.currentZoom = this.targetZoom;
        }
        else
        {
             // Smoothly seek towards our target zoom value
            this.currentZoom = Mathf.SmoothDamp(this.currentZoom, this.targetZoom, ref this.zoomVelocity, this.seekTime);
        }
        // Set the position of the camera
        zoomedPosition = this.defaultLocalPosition + this.thisTransform.parent.InverseTransformDirection(this.thisTransform.forward * this.currentZoom);
        this.thisTransform.localPosition = zoomedPosition;
    }

    public ZoomCamera()
    {
        this.zoomMin = -5;
        this.zoomMax = 5;
        this.seekTime = 1f;
    }

}