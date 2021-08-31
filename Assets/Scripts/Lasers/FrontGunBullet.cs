using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontGunBullet : MonoBehaviour
{
    private float _speed = -2f;
    private Vector3 shootDir;
    
    void Start()
    {
        Invoke("DestroyBullet", 4f);
    }
    public void Shoot(Vector3 shootDir)
    {
        this.shootDir = shootDir;        
    }
    void Update()
    {
        transform.position -= shootDir * _speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {

          if(other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            DestroyBullet();
        }
        

        
        if(other.CompareTag("Player"))
        {
            if (Random.value>.3);
            Player.Instance.Damage();
        }
    }

    void DestroyBullet()
    {
        
        Destroy(this.gameObject);
    }
}
