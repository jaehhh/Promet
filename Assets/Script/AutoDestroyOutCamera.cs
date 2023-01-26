using UnityEngine;

public class AutoDestroyOutCamera : MonoBehaviour
{
    [SerializeField]
    private float distance;

    private void Awake()
    {
        CheckOutCamera();
    }

    private void CheckOutCamera()
    {
        var planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(transform.position) < distance)

            {
                Destroy(this.gameObject);
                return;
            }
        }

        Invoke("CheckOutCamera", 0.1f);
    }
}
