using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("General")] 
    [SerializeField] private float range = 15f;
    [Header("Use Bullets (default)")] 
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 1f;
    private float _fireCountdown;
    [Header("Use Laser")] 
    [SerializeField] private bool useLaser;
    [SerializeField] private float damageOverTime = 30;
    [SerializeField] private float slowAmount = 0.5f;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private ParticleSystem impactEffect;
    [SerializeField] private Light impactLight;
    [Header("Unity Setup Fields")] 
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private Transform partToRotate;
    [SerializeField] private float turnSpeed = 10f;
    [SerializeField] private Transform firePoint;

    private Transform _target;
    private Enemy _targetEnemy;

    private void Start()
    {
        StartCoroutine(UpdateTarget());
    }

    private void Update()
    {
        if (_target == null)
        {
            if (useLaser && lineRenderer.enabled)
            {
                lineRenderer.enabled = false;
                impactEffect.Stop();
                impactLight.enabled = false;
            }

            return;
        }

        LockOnTarget();

        if (useLaser)
        {
            Laser();
        }
        else
        {
            if (_fireCountdown <= 0f)
            {
                Shoot();
                _fireCountdown = 1f / fireRate;
            }

            _fireCountdown -= Time.deltaTime;
        }
    }

    private IEnumerator UpdateTarget()
    {
        while (true)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }

            if (nearestEnemy != null && shortestDistance <= range)
            {
                _target = nearestEnemy.transform;
                _targetEnemy = nearestEnemy.GetComponent<Enemy>();
            }
            else
            {
                _target = null;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void LockOnTarget()
    {
        Vector3 dir = _target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }

    public void Laser()
    {
        _targetEnemy.TakeDamage(damageOverTime * Time.deltaTime);
        _targetEnemy.Slow(slowAmount);

        if (!lineRenderer.enabled)
        {
            lineRenderer.enabled = true;
            impactEffect.Play();
            impactLight.enabled = true;
        }

        Vector3 firePointPosition = firePoint.position;
        lineRenderer.SetPosition(0, firePointPosition);
        Vector3 targetPosition = _target.position;
        lineRenderer.SetPosition(1, targetPosition);

        Vector3 dir = firePointPosition - targetPosition;

        impactEffect.transform.position = targetPosition + dir.normalized * 0.5f;

        impactEffect.transform.rotation = Quaternion.LookRotation(dir);
    }

    public void Shoot()
    {
        Bullet bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation).GetComponent<Bullet>();

        if (bullet != null)
        {
            bullet.SeekTarget(_target);
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}