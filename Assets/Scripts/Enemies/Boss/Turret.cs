using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] GameObject _bullet;

    [SerializeField] GameObject _leftGun;
    [SerializeField] GameObject _rightGun;
    Collider2D _col2D;
    AudioSource _audio;
    Animator _anim;
    Vector3 _maxRotation = new Vector3 (0f,0f,45f);
    float _speed = 0f;
    float _timeToPause = 1f;

    private float _fireRate = 3f;
    private float _canfire;
    private float _bulletTimeVariance;
    private float _bulletSpeedVariance;
    private float _fireRateVariance;

    private float _startTheCarnage = 7;
    private bool _firstFire = false;


    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _anim.speed = Random.Range(.5f,1f);
        _col2D = GetComponent<Collider2D>();

        this._fireRateVariance = Random.Range(0f,1.5f);
        this._canfire = Time.time + _startTheCarnage;
        this._speed += Random.Range(2.5f,4.5f);
        StartCoroutine(Shoot());
    }


   

    IEnumerator Shoot()
    {
        while(true)
        {
            if(_col2D.enabled)
            {
                if(Time.time > _canfire)
                {
                    if(_firstFire)
                    {
                    _firstFire = false;
                    this._canfire = Time.time + _fireRate + _bulletTimeVariance;
                    yield return 0
                    ;
                    //this prevents an impossible burst of bullets the player cant avoid.
                    }
                    this._bulletTimeVariance = Random.Range(0f,1.5f);
                    this._bulletSpeedVariance = Random.Range(0f,-3f);
                    this._canfire = Time.time + _fireRate + _bulletTimeVariance;
                    GameObject _bulletLeft = Instantiate(_bullet, _leftGun.transform.position, transform.rotation);
                    GameObject _bulletRight = Instantiate(_bullet, _rightGun.transform.position, transform.rotation);
                    _bulletLeft.GetComponent<TurretBullet>().BulletSpeed += _bulletSpeedVariance;
                    _bulletRight.GetComponent<TurretBullet>().BulletSpeed += _bulletSpeedVariance;
                    _audio.Play();
                    yield return 0;
                }
                yield return 0;
            }

            yield return 0;
            
        }
        
    }
}
