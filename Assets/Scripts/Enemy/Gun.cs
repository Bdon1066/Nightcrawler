using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    public float bulletSpeed;
    public Transform bulletSpawnPos;
    private GameObject spawnedBullet;
    public void FireBullet(Transform target)
    {
       spawnedBullet = Instantiate(bullet, transform.position + bulletSpawnPos.position, Quaternion.identity);
       spawnedBullet.transform.position = bulletSpawnPos.position;
       spawnedBullet.transform.LookAt(target);
       spawnedBullet.GetComponent<Rigidbody>().AddForce(spawnedBullet.transform.forward * bulletSpeed, ForceMode.Impulse);

       StartCoroutine(DestroyBullet(spawnedBullet, 1f));
    }
    private IEnumerator DestroyBullet(GameObject projectile, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(projectile);
    }

}
