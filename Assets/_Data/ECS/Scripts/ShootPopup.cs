using UnityEngine;

public class ShootPopup : MonoBehaviour
{
    private float destroyTimer = 1f;

    private void Update()
    {
        float moveSpeed = 2f;
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        destroyTimer -= Time.deltaTime;
        if ( destroyTimer < 0)
        {
            Destroy(gameObject);
        }
    }
}
