using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    private Transform target;
    private int wavepointIndex = 0;

    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponent<Enemy>();

        target = Waypoints.points[0];
    }

    private void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * enemy.speed * Time.deltaTime);

        if (Vector3.Distance(target.position, transform.position) <= 0.4f)
        {
            GetNextWavepoints();
        }

        enemy.speed = enemy.startSpeed;
    }

    private void GetNextWavepoints()
    {
        if (wavepointIndex >= Waypoints.points.Length - 1)
        {
            EndPath();
            return;
        }

        wavepointIndex++;
        target = Waypoints.points[wavepointIndex];
    }

    private void EndPath()
    {
        PlayerStats.PlayerLives--;
        WaveSpawner.EnemiesAlive--;
        Destroy(gameObject);
    }
}