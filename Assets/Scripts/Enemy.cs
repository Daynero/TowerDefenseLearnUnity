using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 10f;

    public int health = 100;

    public int earnings = 50;

    public GameObject deathEffct;

    private Transform target;
    private int wavepointIndex = 0;

    void Start()
    {
        target = Waypoints.points[0];
    }

    public void TakeDamage (int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            Die();
        }
    }

    void Die ()
    {
        var effect = (GameObject)Instantiate(deathEffct, transform.position, Quaternion.identity);
        PlayerStats.Money += earnings;
        Destroy(effect, 5f);
        Destroy(gameObject);   
    }
    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime);

        if (Vector3.Distance(target.position, transform.position) <= 0.4f)
        {
            GetNextWavepoints();
        }
    }

    void GetNextWavepoints()
    {
        if (wavepointIndex >= Waypoints.points.Length - 1)
        {
            EndPath();  
            return;
        }

        wavepointIndex++;
        target = Waypoints.points[wavepointIndex];
    }

    void EndPath ()
    {
        PlayerStats.Lives--;
        Destroy(gameObject);
    }
}
