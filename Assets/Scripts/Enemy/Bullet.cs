using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && !collider.gameObject.GetComponent<PlayerController>().isInvisible)
        {  
            //Debug.Log("Shot Player");
            collider.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(this.gameObject);
        }
    }
}
