using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected float _enemySpeed;
    [SerializeField]
    private int _enemyID;
    [SerializeField]
    private GameObject _enemyShieldHolder;
    private GameObject _enemyShield;
    public bool _shieldActivated;
    public bool canShoot;

    protected float _fireRate = 3.0f;
    protected float _canFire = -1;
    protected bool _isExploding = false;

    protected Vector3 _currentPos; 
    protected Vector3 _lerpPos = Vector3.zero;


    [SerializeField]
    protected int _shotsToKill = 1;

    protected float _x,_y,_z,  _amp, _freq;
    protected Animator _anim;
    protected AudioSource _audioSource;
    protected Rigidbody2D _rb;

    protected bool _shoots = false;

    [SerializeField] protected GameObject _enemyLaser;

    [SerializeField] protected AudioClip _enemyLaser_Clip;
    
    [SerializeField] protected AudioClip _explosion_Clip;
    public bool enemyLaserGoingUp;
    [SerializeField]
    private float _enemyLaserSpeed = 8f;

    Vector3 _originalPosition;

    Vector3 _direction = new Vector3(0,-1,0);

    public bool isMovingRight;
    protected int _phase = 1;

    [SerializeField]
    GameObject _explosionHolder;
    public bool playerNear = false;

    int _numberOfDodges=0;
    int _numberOfDodgesLimit = 3;
    private bool _isDead = false;




    void Start()
    {   
        _enemySpeed = Random.Range(1f,6f);        

        _audioSource = GetComponent<AudioSource>();
        if(!_audioSource)
            Debug.LogError("Audio source is null");

        _anim = GetComponent<Animator>();
        if(!_anim)
            Debug.LogError("The Animator inside Enemy is Null");

        _rb = GetComponent<Rigidbody2D>();
        if (!_rb)
            Debug.LogError("_rb is NULL");

        if(_shieldActivated)
        {
            _enemyShield.gameObject.SetActive(true);
        }

       
    switch (_enemyID)
        {
            case 2:
            //Enemy 2 initialize side by side movement
            _amp = Random.Range(.1f,2.5f);
            _freq = Random.Range(.5f,4.5f);
            break;

            case 3:
            _amp = Random.Range (.05f, .07f);
            _freq = Random.Range(5f,7f); 
            
            var boolNum = Random.Range(0,2);
            if (boolNum>0)
            {isMovingRight = true;
            }
            else
            {
                {isMovingRight = false;}
            }

            if (isMovingRight)
            {
                transform.position = new Vector2(-10,Random.Range(5,0));
            }else
            {
                transform.position = new Vector2(10, Random.Range(5,0));
            }    
            break;
            case 4:
            break;

            case 5:
            _enemySpeed = Random.Range(3f,4f);
            canShoot=true;
            _fireRate=5f;
            break;

            default:
            break;
        }
   


    }

    protected virtual void Update(){ CalculateMovementByID(); }

    void CalculateMovementByID()
    {
        switch(_enemyID)
        {
            case 0:
                Enemy0Movement();
                PowerupHunting();
                FireLasers(_enemyLaserSpeed);
               
            break;
            case 1:
                Enemy1Movement();
                PowerupHunting();
                FireLasers(_enemyLaserSpeed);
            break;
            case 2:
                Enemy2Movement();
                PowerupHunting();
                FireLasers(_enemyLaserSpeed);
            break;

            case 3:
                Enemy3Movement();
                PowerupHunting();
                FireLasers(_enemyLaserSpeed);
            break;

            case 4:
            //big laser enemy
            break;

            case 5:
            RammerMovement();
            
            if(canShoot)
            {
                PowerupHunting();
                FireLasers(_enemyLaserSpeed);
            }
            break;

            case 6:
                Move(Vector2.down, _enemySpeed);
                CheckPlayerBehind();

                FireLasers(_enemyLaserSpeed);
                CheckBottom();                
                break;
            
            case 7:
            Move(Vector2.down, _enemySpeed);
            //FireLasers(_enemyLaserSpeed);
            CheckBottom();
         
                
            break;

            default:              
            break;
        }
        

    }

    void Move(Vector2 Direction, float speed)
    {
        _direction = Direction;
        _enemySpeed = speed;
        transform.Translate(_direction * _enemySpeed * Time.deltaTime);
    }

 

    void FireLasers(float laserSpeed)
    {
        var _laserSpeed = -laserSpeed;
        var _instantiationPosition = transform.position;

        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(4.5f,7.3f);
            _canFire = Time.time + _fireRate;  

            if(enemyLaserGoingUp)
            {
                _laserSpeed *= -1;
                _instantiationPosition += new Vector3(0,3f,0);

            }


            if (!_isDead)
            {
                GameObject enemyLaser = Instantiate(_enemyLaser, _instantiationPosition, Quaternion.identity) as GameObject;
                enemyLaser.GetComponent<DuoLaser>().speed = _laserSpeed;
                Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser_Enemy>();
                _audioSource.clip = _enemyLaser_Clip;
                _audioSource.Play();
            }

        }
    }

  

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {       
        if(other.CompareTag("Player"))
        {        
            Player.Instance.Damage();
            DestroyEnemyShip();            
        }

       

        if(other.CompareTag("Laser"))
        {
             switch(_enemyID)
            {
                case 0:
                Destroy(other.gameObject);
                TakeDamage();
                break;

                case 1:
                Destroy(other.gameObject);
                TakeDamage();
                break;

                case 2:
                Destroy(other.gameObject);
                TakeDamage();
                break;

                case 3:
                Destroy(other.gameObject);
                TakeDamage();
                break;

                case 4:
                Destroy(other.gameObject);
                TakeDamage();
                break;

                case 5:
                 bool haveChosen=false;

                if(_enemyID == 5)//Check to see if it's the rammer
                {
                    if(other is BoxCollider2D)
                        {
                            var _otherPos = other.gameObject.transform.position;
                            GameObject kaboom = Instantiate(Resources.Load("Explosion_Smaller")) as GameObject;
                            kaboom.transform.position = _otherPos;
                            kaboom.transform.parent = transform;

                            Destroy(other.gameObject);
                            TakeDamage();
                        }
                }     

                break;

                case 6:
                Destroy(other.gameObject);
                TakeDamage();
                break;

                case 7:
                haveChosen = false;
                if (_numberOfDodges < _numberOfDodgesLimit)
                {
                    //Dodge it
                   
                        _numberOfDodges ++;
                        Vector3 amountToMove = new Vector3(1.5f,0,0);
                        var _curPos = transform.position;
                        if(other.transform.position.x < transform.position.x)
                        {   if(!haveChosen)
                            {
                            _curPos += amountToMove;
                            haveChosen=true;                        
                            }
                        }
                        else if(other.transform.position.x>transform.position.x)
                        {
                            if(!haveChosen)
                            {
                                _curPos -= amountToMove;
                                haveChosen = true;
                            }   
                        }
                        _lerpPos = _curPos;
                        StartCoroutine(Dodge());       
                    
                }else{
                    Destroy(other.gameObject);
                    TakeDamage();
                }              
                break;    

             
            }

        }
    }

    void TakeDamage()
    {
                
                _shotsToKill--;
                if(_shotsToKill <1)
                {
                    Destroy(gameObject.GetComponent<Collider2D>());            
                    Player.Instance.AddScore(Random.Range (7,11));
                    DestroyEnemyShip();
                }  
    }
    IEnumerator Dodge()
    {
        
        while (transform.position.x != _lerpPos.x)
        {
            Debug.Log(transform.position.x + " , " + _lerpPos.x);

            _lerpPos.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, _lerpPos, 7f * Time.deltaTime);
            yield return 0;             
        } 
        yield return null;
    }


    public virtual void DestroyEnemyShip()
    {  

        SpawnManager.Instance._powerupTime -= .5f;
        _isDead = true;
        _enemySpeed = 0;
        canShoot = false;
        _isExploding = true;
        //Change the enemy color to white so the internal explosion looks yellow and not tinted.
        GetComponent<SpriteRenderer>().color = Color.white;
        _audioSource.clip = _explosion_Clip;
        _audioSource.Play(); 

        //Some new enemies don't have the built in animation that uses the parameter "onEnemyDeath" to trigger
        //We'll just check the animator for any parameters, if they have them, it's the old one
        //if there's no parameters, it's a new enemy with an explosion tacked on so play that.
        if (_anim.parameterCount>0)
        {
            _anim.SetTrigger("onEnemyDeath");

        }else{
            Instantiate(_explosionHolder, transform.position, Quaternion.identity);
            Destroy(this.gameObject);            
        }
        

    }

    public void RemoveGameObjectFromScene()
    {
        //public method called from within the enemy explosion animation.
        Destroy(this.gameObject);
    }

    void Enemy0Movement()
    {
        Move(new Vector2(0,-1),_enemySpeed);
        CheckBottom();
    }

    void CheckBottom()
    {
         if (transform.position.y < -6f)
        {
            PickNewTopPosition();
        }
    }

    void Enemy1Movement()
    {    
        Move(new Vector2(0,-1),_enemySpeed); 
        //if enemy reaches halfway down the screen
        if(transform.position.y < 1.3)
        {            
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(180,0,0), 400*Time.deltaTime);
        }
        //turn around and move up.
        //if ship reaches top border, pick a new position and move down.
        if (transform.position.y > 9)
        {            
            PickNewTopPosition();
            transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
        }
            
    }

    void PickNewTopPosition()
    {
        transform.position = new Vector3 (Random.Range(-6.5f,6.5f),Random.Range(8f,8.9f),0);
    }

    void Enemy2Movement()
    {

         
         var lastX = _x;

            if(_isExploding)
            {
                _x=lastX;
            }
            else
            {
            _y = transform.position.y;
            _x = Mathf.Cos(Time.time * _freq)*_amp;
            _z = transform.position.z; 
            transform.position = new Vector3 (_x,_y,_z);
            Move(new Vector2(0,-1),_enemySpeed);
            CheckBottom();
            }        
    }

    void Enemy3Movement()
    {

         _y = Mathf.Sin(Time.time * _freq)*_amp;
         _x = Mathf.Cos(Time.time * _freq)*_amp;
         _z = transform.position.z;


            if(_isExploding)
            {
                _x=0;
                _y=0;
            }

            transform.position += new Vector3 (_x,_y,_z);
            
            if (isMovingRight)
            {
                _direction = new Vector2(1,0);
            }else
            {
                _direction = new Vector2(-1,0);
            }
            Move(_direction, _enemySpeed);
        Enemy3CheckBorders();    
    }

    void Enemy3CheckBorders()
    {
        if(isMovingRight && transform.position.x >10)
        {
            isMovingRight = !isMovingRight;
        }else if (!isMovingRight && transform.position.x <-10)
        {
            isMovingRight = !isMovingRight;
        }

    }

    void RammerMovement()
    {
        
        //phase 1, move down as normal
        //until player gets within a certain range
        //then follow the player
        //if they get very close, speed up and ram them.
        if (_phase == 1)
        {   
            _rb.rotation = -90f;
            Move(Vector2.right,_enemySpeed);
            CheckBottom();

            if(playerNear)
            {
                //Follow the player until enemy dies I guess.
                _phase +=1;
                
            }
        }

        if(_phase > 1)
        {
            canShoot = false;
            Vector3 direction = Player.Instance.gameObject.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            _rb.rotation = angle;
            direction.Normalize();
            
            _rb.MovePosition(transform.position+(direction * _enemySpeed * Time.deltaTime));
            
        }
    }
 
    public void SpawnShield()
    {
        _enemyShield = Instantiate(_enemyShieldHolder, transform.position, Quaternion.identity);
        _enemyShield.transform.parent = transform;
    }
    
    void CheckPlayerBehind()
    {
        Vector3 _playerPos = Player.Instance.gameObject.transform.position; 
        Vector3 _relativePos = transform.position - _playerPos;
        bool _isInFront = Vector3.Dot(transform.up, _relativePos) > 0.0f;

        if(_isInFront)
        {
            enemyLaserGoingUp = false;
        }
        else
        {            
            enemyLaserGoingUp = true;
        }
    }

void PowerupHunting()
{
    //If a powerup is directly in front of the enemy, enemy shoots.
    //cast a ray straight down
    RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0,-1f,0), transform.TransformDirection(Vector2.down), 10f);
    //Debug.DrawRay(transform.position + new Vector3(0,-1f,0), transform.TransformDirection(Vector2.down) * 10f, Color.red);

    if (hit)
    {

       if (hit.collider.tag == "PowerUp")
       {
          StartCoroutine(KillPowerup());
       }
    
    }
    

}
IEnumerator KillPowerup()
{
    FireLasers(20f);
    yield return new WaitForSeconds(1f);
}

}
