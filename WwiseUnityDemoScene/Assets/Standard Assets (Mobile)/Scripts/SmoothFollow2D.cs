using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class SmoothFollow2D : MonoBehaviour
{
    public Transform target;
    public float smoothTime;
    private Transform thisTransform;
    private Vector2 velocity;
    public virtual void Start()
    {
        this.thisTransform = this.transform;
    }

    public virtual void Update()
    {

        {
            float _22 = Mathf.SmoothDamp(this.thisTransform.position.x, this.target.position.x, ref this.velocity.x, this.smoothTime);
            Vector3 _23 = this.thisTransform.position;
            _23.x = _22;
            this.thisTransform.position = _23;
        }

        {
            float _24 = Mathf.SmoothDamp(this.thisTransform.position.y, this.target.position.y, ref this.velocity.y, this.smoothTime);
            Vector3 _25 = this.thisTransform.position;
            _25.y = _24;
            this.thisTransform.position = _25;
        }
    }

    public SmoothFollow2D()
    {
        this.smoothTime = 0.3f;
    }

}