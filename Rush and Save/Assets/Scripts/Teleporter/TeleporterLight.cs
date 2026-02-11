using UnityEngine;

public class TeleporterLight : MonoBehaviour
{
    [Header("Delay for the Win screen")]
    [SerializeField] private float delay = 0.3f;

    [Header("Reference for the win Screen")]
    public GameObject winScreenPanel;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("TeleporterLight"))
        {
            Debug.Log("Light Rescuer kept in Place");
            Destroy(collision.gameObject,delay);
            winScreenPanel.SetActive(true);
        }
    }
}
