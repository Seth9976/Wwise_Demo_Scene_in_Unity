using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class ObliqueNear : MonoBehaviour
{
    public Transform plane;
    public virtual Matrix4x4 CalculateObliqueMatrix(Matrix4x4 projection, Vector4 clipPlane)
    {
        Vector4 q = projection.inverse * new Vector4(Mathf.Sign(clipPlane.x), Mathf.Sign(clipPlane.y), 1f, 1f);
        Vector4 c = clipPlane * (2f / Vector4.Dot(clipPlane, q));
        projection[2] = c.x - projection[3];
        projection[6] = c.y - projection[7];
        projection[10] = c.z - projection[11];
        projection[14] = c.w - projection[15];
        return projection;
    }

    public virtual void OnPreCull()
    {
        Matrix4x4 projection = this.GetComponent<Camera>().projectionMatrix;
        Matrix4x4 m = this.GetComponent<Camera>().worldToCameraMatrix;
        Vector3 planePos = m.MultiplyPoint(this.plane.position);
        Vector3 planeNormal = m.MultiplyVector(-Vector3.up);
        planeNormal.Normalize();
        Vector4 nearPlane = planeNormal;
        nearPlane.w = -Vector3.Dot(planeNormal, planePos);
        this.GetComponent<Camera>().projectionMatrix = this.CalculateObliqueMatrix(projection, nearPlane);
    }

}