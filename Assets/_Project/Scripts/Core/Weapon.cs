using _Project.Scripts.Core;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    [SerializeField] private GameObject bulletVfxPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float fireRate = 0.1f;
    [SerializeField] private float range = 100f;
    
    [Header("References")]
    [SerializeField] private Transform muzzlePoint;

    [Header("Paramètres (si le prefab ne bouge pas tout seul)")]
    [SerializeField] private float bulletSpeed = 0.05f;
    [SerializeField] private float bulletLifeTime = 3f;

    [SerializeField] private GameObject impactPrefab;

    private float _nextFireTime = 0f;

    // 1. Create a struct to hold the Transform and its unique Direction, I ADDED OTHER PROPERTY
    private struct ProjectileData
    {
        public Transform Transform;
        public Vector3 Direction;
        public bool HasTarget;
        public Vector3 TargetPoint;
        public Vector3 TargetNormal;
    }

    // 2. Store the struct instead of just the Transform
    private readonly List<ProjectileData> _activeProjectiles = new List<ProjectileData>();

    private void Update()
    {
        for (int i = _activeProjectiles.Count - 1; i >= 0; i--)
        {
            var proj = _activeProjectiles[i];
            if (_activeProjectiles[i].Transform == null)
            {
                _activeProjectiles.RemoveAt(i);
                continue;
            }
            float moveDistance = bulletSpeed * Time.deltaTime;

        // Si le Raycast avait touché un obstacle
        if (proj.HasTarget)
        {
            // On calcule la distance restante entre la balle visuelle et le mur
            float distanceToTarget = Vector3.Distance(proj.Transform.position, proj.TargetPoint);

            if (distanceToTarget <= moveDistance)
            {
                proj.Transform.position = proj.TargetPoint;                
                _activeProjectiles.RemoveAt(i);

                if (impactPrefab != null)
                {
                    // Quaternion.LookRotation permet d'orienter le decal parfaitement à plat sur le mur
                    Instantiate(impactPrefab, proj.TargetPoint, Quaternion.LookRotation(proj.TargetNormal));
                }

                Destroy(proj.Transform.gameObject);
                continue;
            }
        }

            _activeProjectiles[i].Transform.position += _activeProjectiles[i].Direction * bulletSpeed * Time.deltaTime;
        }
    }

    public void TryFire(Transform cameraTransform)
    {
        if (Time.time >= _nextFireTime)
        {
            _nextFireTime = Time.time + fireRate;
            ExecuteShot(cameraTransform);
        }
    }

    private void ExecuteShot(Transform cameraTransform)
    {
        GameObject projectile = Instantiate(bulletVfxPrefab, firePoint.position, firePoint.rotation);

        
        
        // Save the direction locally for this specific shot
        Vector3 currentShotDirection = cameraTransform.forward;

        bool hasHit = Physics.Raycast(muzzlePoint.position, currentShotDirection, out RaycastHit hit, range);
        
        Debug.DrawRay(muzzlePoint.position, currentShotDirection * range, Color.red, 1);

        if (projectile.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.linearVelocity = firePoint.forward * bulletSpeed;
        }
        else
        {
            // 4. Add the new bullet AND its specific direction to the list
            _activeProjectiles.Add(new ProjectileData 
            { 
                Transform = projectile.transform, 
                Direction = currentShotDirection,
                HasTarget = hasHit,
                TargetPoint = hasHit ? hit.point : Vector3.zero,
                TargetNormal = hasHit ? hit.normal : Vector3.zero
            });
        }

        if (hasHit)
        {
            var other = hit.collider.gameObject;

            if (other.CompareTag("Player") || other.CompareTag("Ennemy"))
            {
                Debug.Log(hit.transform.name);
                IDamageable entity = other.GetComponent<IDamageable>();
            }
        }

        Destroy(projectile, bulletLifeTime);
    }
}