using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class FollowTransform : MonoBehaviour
{
    //////////////////////////////////////////////////////////////
    // FollowTransform.js
    // Penelope iPhone Tutorial
    //
    // FollowTransform will follow any assigned Transform and 
    // optionally face the forward vector to match for the Transform
    // where this script is attached.
    //////////////////////////////////////////////////////////////
    public Transform targetTransform; // Transform to follow
    public bool faceForward; // Match forward vector?
    private Transform thisTransform;
    public virtual void Start()
    {
         // Cache component lookup at startup instead of doing this every frame
        this.thisTransform = this.transform;
    }

    public virtual void Update()
    {
        this.thisTransform.position = this.targetTransform.position;
        if (this.faceForward)
        {
            this.thisTransform.forward = this.targetTransform.forward;
        }
    }

}