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
            var other = hit.collider.gameObject;

            // TODO: Est ce que on garde la possiblite qu'un ennemi en touche un autre ???
            if (other.tag == "Player" || other.tag == "Ennemy")
            {
                Debug.Log(hit.transform.name);
                IDamageable entity = other.GetComponent<IDamageable>();
            }
            // Instantiate(impactPrefab, hit.point, hit.transform.rotation);
        }
    }
}