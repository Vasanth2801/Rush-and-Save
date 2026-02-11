using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    Image healthBar;
    public GameObject gameOverPanel;

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            health = 0;
            Destroy(gameObject);
            gameOverPanel.SetActive(true);
        }
    }
}