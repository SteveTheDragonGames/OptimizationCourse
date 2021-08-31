using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontGun : MonoBehaviour
{
    [SerializeField] private GameObject _laserprefab;
    private Collider2D _col2D;

    private float _canFire=3f;
    private float _fireRate = 3f;
    private float _fireRateVariance;

    private GameObject _target;
    private Vector3 _shootPosition;

    void Start()
    {
        _col2D = GetComponent<Collider2D>();
        _target = Player.Instance.gameObject;  
    }

    // Update is called once per frame
    void Update()
    {
        if(_col2D.enabled)
        {
            if(Time.time > _canFire)
            {
                this._fireRateVariance = Random.Range(1f,4f);
                Vector3 shootDir = (Player.Instance.transform.position - this.transform.position).normalized;
                GameObject _laser = Instantiate (_laserprefab, transform.position, Quaternion.identity) as GameObject;
                _laser.GetComponent<FrontGunBullet>().Shoot(shootDir);
                
                this._canFire = Time.time + _fireRate + _fireRateVariance;
            }
            

        }
    }
}
