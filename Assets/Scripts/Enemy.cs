using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float startHealth = 100f;
    [SerializeField] private int earnings = 50;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private Image healthBar;

    private float health;
    
    [HideInInspector] public float speed;
    public float startSpeed = 10f;

    private void Start()
    {
        speed = startSpeed;
        health = startHealth;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        healthBar.fillAmount = health / startHealth;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Slow(float deceleration)
    {
        speed = startSpeed * (1f - deceleration);
    }

    private void Die()
    {
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        PlayerStats.instance.PlayerMoney += earnings;
        Destroy(effect, 5f);

        WaveSpawner.EnemiesAlive--;

        Destroy(gameObject);
    }
}