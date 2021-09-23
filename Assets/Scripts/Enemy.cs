using System;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float startHealth = 100f;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private Image healthBar;
    [SerializeField] private int _earnings;
    
    private EnemyMovement _enemyMovement;
    private float _health;
    private Action<int> Die;
    
    [HideInInspector] public float speed;
    public float startSpeed = 10f;

    public void Initialize(Action enemyPathEnded, Action<int> enemyDie)
    {
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyMovement.EndPath = () => 
        {
            enemyPathEnded.Invoke();
            Destroy(gameObject);
        };
        
        Die = enemyDie;
    }
    
    private void Start()
    {
        speed = startSpeed;
        _health = startHealth;
    }

    public void TakeDamage(float amount)
    {
        _health -= amount;

        healthBar.fillAmount = _health / startHealth;

        if (_health <= 0)
        {
            Death();
        }
    }

    public void Slow(float deceleration)
    {
        speed = startSpeed * (1f - deceleration);
    }

    private void Death()
    {
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Die.Invoke(_earnings);
        Destroy(effect, 5f);
        Destroy(gameObject);
    }
}