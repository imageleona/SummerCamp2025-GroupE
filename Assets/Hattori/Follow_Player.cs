using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 0, -10); // í‚ÉZ•ûŒü‚É10ˆø‚¢‚ÄƒvƒŒƒCƒ„[‚Ìã‹ó‚©‚ç‰f‚·

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}
