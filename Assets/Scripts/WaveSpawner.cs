using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private Wave[] waves;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private TMP_Text waveCountdownText;
    [SerializeField] private GameManager gameManager;

    // private Enemy _enemyPrefab;
    private float _countdown = 2f;
    private int _waveIndex;

    public int EnemiesAlive { get; set; }

    public event Action RoundsUpdateNotify;
    public Action EnemyPathEnded;
    public Action<int> EnemyDie;

    private void Start()
    {
        EnemyPathEnded += () => EnemiesAlive--;
    }

    private void Update()
    {
        if (EnemiesAlive > 0)
        {
            return;
        }

        if (_countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            _countdown = timeBetweenWaves;
            return;
        }

        _countdown -= Time.deltaTime;

        _countdown = Mathf.Clamp(_countdown, 0f, Mathf.Infinity);

        waveCountdownText.text = string.Format("{0:00.00}", _countdown);
    }

    private IEnumerator SpawnWave()
    {
        RoundsUpdateNotify?.Invoke();

        Wave wave = waves[_waveIndex];

        EnemiesAlive = wave.count;

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f / wave.rate);
        }

        _waveIndex++;

        if (_waveIndex == waves.Length)
        {
            gameManager.WinLevel();
            enabled = false;
        }
    }

    private void SpawnEnemy(Enemy enemyPrefab)
    {
        Enemy enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        enemy.Initialize(EnemyPathEnded, EnemyDie);
    }
}