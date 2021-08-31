using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Enemy : DuoLaser
{

    private GameObject _target;
    private Vector3 _laserHomingDirection;
    public Vector3 whatDirection = Vector3.down;

    GameObject _parent;

  
    protected override void Start()
    {
        Invoke("DestroyLasers",4f);   
         if(GameManager.Instance._negativePowerupActivated)
        {
            HomingLasers();
        }else
        {
            _laserDirection = whatDirection * speed * Time.deltaTime;
        }
    }
    

    protected override void DestroyLasers()
    {
        if(transform.parent.gameObject.name == "Enemy_DuoLaser(Clone)")
        {
            //transform.parent.gameObject.SetActive(false);
            Destroy(transform.parent.gameObject);
            
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            return;
        }
        if (other.CompareTag("Player"))
        {
            Player.Instance.Damage();
        }
    }

    public void HomingLasers()
    {
        _target = Player.Instance.gameObject;
        _laserHomingDirection = (_target.transform.position - transform.position).normalized;
        _laserDirection =  new Vector3(_laserHomingDirection.x,-1,0) * speed * Time.deltaTime;
    }

}
