using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;
    public Vector3 direction;
    public System.Action destroyed;

    private void Update()
    {
        this.transform.position += this.direction * this.bulletSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {   
        if(this.gameObject.tag == "PlayerBullet")
        {
            if(!(other.gameObject.tag == "Player") && !(other.gameObject.tag == "EnemyBullet"))
            {
                this.destroyed.Invoke();
                Destroy(this.gameObject);
            }
        }
        else if(this.gameObject.tag == "EnemyBullet")
        {
            if(this.destroyed != null) this.destroyed.Invoke();
            if(!(other.gameObject.tag == "Enemy") && !(other.gameObject.tag == "PlayerBullet"))
            {
                Destroy(this.gameObject);
            }
        }
    }
}