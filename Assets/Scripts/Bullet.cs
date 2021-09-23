using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 70f;
    [SerializeField] private int damage = 50;
    [SerializeField] private float explosionRadius;
    [SerializeField] private GameObject impactEffect;

    private Transform _target;
    
    private void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = _target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(_target);

    }

    public void SeekTarget(Transform target)
    {
        _target = target;
    }
    
    private void HitTarget()
    {
        GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 5f);

        if (explosionRadius > 0f)
        {
            Explode();
        }
        else
        {
            Damage(_target);
        }
       
        Destroy(gameObject);
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var collid in colliders)
        {
            if (collid.CompareTag("Enemy"))
            {
                Damage(collid.transform);
            }
        }
    }

    private void Damage(Transform enemy)
    {
        Enemy _enemy = enemy.GetComponent<Enemy>();

        if (_enemy != null)
        {
            _enemy.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}
