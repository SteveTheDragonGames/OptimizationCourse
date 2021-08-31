using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    public float speed;

    [SerializeField]
    public Vector3 _laserDirection = Vector3.up;

    protected virtual void Start()
    {
        Invoke("DestroyLasers",4f);
    }


    protected virtual void Update()
    {
        Move();    
    }


    protected virtual void Move()
    {
        transform.Translate(_laserDirection * speed * Time.deltaTime);
        
    }

    protected virtual void DestroyLasers()
    {
        //this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        //is this really needed?
    }


}
