using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletSpawnPos;
    private GameObject spawnedBullet;
    public void FireBullet(Transform target)
    {
        print("SHOOT");
       spawnedBullet = Instantiate(bullet, transform.position + bulletSpawnPos.position, Quaternion.identity);
       spawnedBullet.transform.position = bulletSpawnPos.position;
       spawnedBullet.transform.LookAt(target);
    }

}
