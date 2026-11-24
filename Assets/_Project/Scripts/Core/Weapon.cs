using _Project.Scripts.Core;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Stats")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float fireRate = 0.1f;
    [SerializeField] private float range = 100f;
    [Header("References")]
    [SerializeField] private Transform muzzlePoint;
    // [SerializeField] private GameObject impactPrefab;


    private float _nextFireTime = 0f;

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
        RaycastHit hit;
        Debug.DrawRay(muzzlePoint.position, cameraTransform.forward * range, Color.red, 1);

        if (Physics.Raycast(muzzlePoint.position, cameraTransform.forward, out hit, range))
        {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            
            if (damageable != null)
            {
                // can take damage
                damageable.TakeDamage(damage, hit.point, hit.normal);
            }
            else
            {
                // hit a wall or something indestructible
                // TODO: Spawn a bullet hole decal or dust particle here
            }
        }
    }
}