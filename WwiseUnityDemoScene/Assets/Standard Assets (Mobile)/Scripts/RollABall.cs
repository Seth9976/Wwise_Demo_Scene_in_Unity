using UnityEngine;
using System.Collections;

[System.Serializable]
[UnityEngine.RequireComponent(typeof(Rigidbody))]
public partial class RollABall : MonoBehaviour
{
    public Vector3 tilt;
    public float speed;
    private float circ;
    private Vector3 previousPosition;
    public virtual void Start()
    {
         //Find the circumference of the circle so that the circle can be rotated the appropriate amount when rolling
        this.circ = (2 * Mathf.PI) * this.GetComponent<Collider>().bounds.extents.x;
        this.previousPosition = this.transform.position;
    }

    public virtual void Update()
    {
        this.tilt.x = -Input.acceleration.y;
        this.tilt.z = Input.acceleration.x;
        this.GetComponent<Rigidbody>().AddForce((this.tilt * this.speed) * Time.deltaTime);
    }

    public virtual void LateUpdate()
    {
        Vector3 movement = this.transform.position - this.previousPosition;
        movement = new Vector3(movement.z, 0, -movement.x);
        this.transform.Rotate((movement / this.circ) * 360, Space.World);
        this.previousPosition = this.transform.position;
    }

    public RollABall()
    {
        this.tilt = Vector3.zero;
    }

}