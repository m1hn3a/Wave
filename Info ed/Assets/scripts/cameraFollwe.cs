using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;

    private bool snapNextFrame = false;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 newPos = new Vector3(target.position.x, target.position.y, transform.position.z);

        if (snapNextFrame)
        {
            // ⭐ Sărim instant la player
            transform.position = newPos;
            snapNextFrame = false;
        }
        else
        {
            // ⭐ Follow normal cu smoothing
            transform.position = Vector3.Lerp(transform.position, newPos, smoothSpeed * Time.deltaTime);
        }
    }

    // ⭐ Asta o chemăm când playerul se teleportează
    public void SnapNow()
    {
        snapNextFrame = true;
    }
}
