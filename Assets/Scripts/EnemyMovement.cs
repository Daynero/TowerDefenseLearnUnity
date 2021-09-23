using System;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMovement : MonoBehaviour
{
    private Transform _target;
    private int _wavepointIndex;
    private Enemy _enemy;
    
    public Action EndPath;

    private void Start()
    {
        _enemy = GetComponent<Enemy>();

        _target = Waypoints.Points[0];
    }

    private void Update()
    {
        Vector3 dir = _target.position - transform.position;
        transform.Translate(dir.normalized * (_enemy.speed * Time.deltaTime));

        if (Vector3.Distance(_target.position, transform.position) <= 0.4f)
        {
            GetNextWavepoints();
        }

        _enemy.speed = _enemy.startSpeed;
    }

    private void GetNextWavepoints()
    {
        if (_wavepointIndex >= Waypoints.Points.Length - 1)
        {
            EndPath?.Invoke();
            return;
        }

        _wavepointIndex++;
        _target = Waypoints.Points[_wavepointIndex];
    }
}