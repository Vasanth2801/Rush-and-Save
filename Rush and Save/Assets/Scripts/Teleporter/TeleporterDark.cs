using UnityEngine;

public class TeleporterDark : MonoBehaviour
{
    [Header("Win screen for the delay")]
    [SerializeField] float delay = 0.3f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("TeleporterDark"))
        {
            Debug.Log("Dark Rescue kept in the place");
            Destroy(collision.gameObject,delay);
        }
    }
}