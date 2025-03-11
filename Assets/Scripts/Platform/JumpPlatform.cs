using UnityEngine;

public class JumpPlatform : MonoBehaviour
{
    [SerializeField] private float jumpValue;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

                rb.AddForce(Vector3.up * jumpValue, ForceMode.Impulse);
            }
        }
    }
}
