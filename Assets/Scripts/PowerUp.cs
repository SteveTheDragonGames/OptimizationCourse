using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] //0 = Tripleshot, 1 = Speed, 2 = Shields.
    private int _powerUpID;

    [SerializeField]
    private float _powerUpSpeed = 3.0f;
    private float _powerUpSpeedOriginal;
    private float _bottomOfScreen = -7.0f;

    [SerializeField]
    private AudioClip _audioClip;


    [SerializeField]
    private GameObject Explosion;

    private Rigidbody2D _rb;

    protected Vector3 _direction = Vector3.down;

    protected bool CPressed = false;

    void OnEnable()
    {
        Player.powerupMagnet += HeadTowardsPlayer;
    }

    void OnDisable()
    {
        Player.powerupMagnet -= HeadTowardsPlayer;
    }

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _powerUpSpeedOriginal = _powerUpSpeed;
    }

 
    void Update()
    {
        if (CPressed)
        {
         Vector3 target = Player.Instance.transform.position;
        _direction = (target - transform.position).normalized;
        _powerUpSpeed += .5f;

        }else
        {
            _direction = Vector3.down;
            _powerUpSpeed = _powerUpSpeedOriginal;
        }
        transform.Translate(_direction * _powerUpSpeed * Time.deltaTime);

        if(transform.position.y <= _bottomOfScreen)
        {
            Destroy(this.gameObject);
        }

        CPressed = false;

    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {   
            AudioSource.PlayClipAtPoint(_audioClip,transform.position);

           
            Player.Instance.powerUpID = _powerUpID;
            if(Player.Instance)
            {
               switch(_powerUpID)
                {
                    case -1:
                        GameManager.Instance.ActivateNegativePowerup1();
                    break;
                    case 0:
                        Player.Instance.ActivateTripleShot();
                        break;
                    case 1:
                        Player.Instance.ActivateSpeedBoost();
                         break;
                    case 2:
                        Player.Instance.ActivateShields();                        
                         break;
                    case 3:
                        Player.Instance.AddAmmo();
                        break;
                    case 4:
                        Player.Instance.AddHealth();
                        break;
                    case 5:
                        Player.Instance.ActivateMissiles();
                        break;
                    
                    default:
                         break;
                }
            }
            Destroy(this.gameObject);
        }

        if(other.CompareTag("EnemyLaser"))
        {
            StartCoroutine(DestroyPowerup());
        }
    }

    IEnumerator DestroyPowerup()
    {
        Instantiate(Explosion,transform.position+ new Vector3(.3f,0-.3f,0),Quaternion.identity);
        yield return new WaitForSeconds(.4f);
        
        Destroy(this.gameObject);
    }

    void HeadTowardsPlayer()
    {
        CPressed = true;
    }
}
