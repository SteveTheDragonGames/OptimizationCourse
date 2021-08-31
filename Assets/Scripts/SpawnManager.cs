using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;
    public static SpawnManager Instance
    {
        get{
            if(_instance == null)
                Debug.Log("SpawnManager is NULL");
            
            return _instance;
        }
    }

    [SerializeField]
    private GameObject[] _enemyPrefab;
    [SerializeField] private Text _enemyHolderHolder;

    [SerializeField] private GameObject[] _powerUps;
    private float _minpowerupTime, _maxpowerupTime;
    public float _powerupTime;

    [SerializeField] private GameObject _enemyHolder;
    private float xPos = 6.5f;
    private float Ypos = 8f;
    [SerializeField] private float _timeToWait = 5.0f;

    public bool _stopSpawning = false;
    private int _powerUpId;


[SerializeField] private Text _levelText;
[SerializeField] private Text _debugText;

[SerializeField]
    private int _level;
    private int _spawnEnemyNumber;
    private float _timeToSpawn;
    
    [SerializeField]
    private float _maxEnemies;
    
    [SerializeField]
    private int _enemiesSpawned = 0;
    public int _enemiesDestroyed =0;
    public int _enemiesOnScreen = 0;
    private GameObject[] _enemiesOnScreenArray;
    private float _maxEnemiesOnScreen = 1f;

    [SerializeField]
    private int _powerupToSpawn;

    private int _weightedTotal;

    private bool _firstPowerupSpawn = true;
    public bool _isBossFight = false;
    private int _bossFightLevel = 11;
    private float _canSpawn = -1f;
    private bool _asteroidDead = false;
    

    void Awake()
    {
        if(_instance == null)
            _instance = this;
        else
            Destroy(this.gameObject);
    }

    void Start()
    {
        if(!_isBossFight)
        {
        _level = 1;
        _timeToSpawn = 5f;
        _timeToWait = 5f;
        _maxEnemies = 5f;
        _maxEnemiesOnScreen = 3;
        _levelText.gameObject.SetActive(false);
        _minpowerupTime = 5f;
        _maxpowerupTime = 15f;
        _powerupTime = Random.Range(_minpowerupTime,_maxpowerupTime);
        }
        else
        {
            _level = 1;
            _timeToSpawn = 8f;
            _timeToWait = 8f;
            _maxEnemies = 1f;
            _maxEnemiesOnScreen = 1f;
            _enemiesOnScreen = 0;
            _levelText.gameObject.SetActive(false);
            _minpowerupTime = 1f;
            _maxpowerupTime = 5f;
            _powerupTime = _maxpowerupTime;

        }     
    }

    public void StartSpawning()
    {
        StartCoroutine("SpawnPowerUpRoutine");
        StartCoroutine("ShowLevel");
        _asteroidDead = true;
    }

 

    void Update()
    {
        if(!_isBossFight)
        {
            if (_asteroidDead)
        {
            if (Time.time > _canSpawn)
            {
                if(!_stopSpawning)
                {
                    SpawnEnemy();
                    _enemiesSpawned ++;
                    _canSpawn = Time.time + _timeToSpawn;
                    if(_enemiesSpawned >= (int)_maxEnemies)
                    {
                        StartCoroutine(NextLevel());
                    }
                }
            
             }
        }
        }
        
        
    }

  
    

    private void SpawnEnemy()
    {
         float randomX = Random.Range(-xPos,xPos); //pick a random spot on the x axis
                Vector3 posToSpawn = new Vector3(randomX,Ypos,0); //put spawning position in variable
            
                ChooseEnemy();

                GameObject newEnemy = Instantiate (_enemyPrefab[Random.Range(0,_spawnEnemyNumber)],posToSpawn, Quaternion.identity);
                
                    //No shields for ships during bossfights.
                    float ShieldChance;
                    if(_isBossFight)
                    {
                        ShieldChance=1;
                    }
                    else
                    {
                        ShieldChance = .8f;
                    }
                    var shieldOrNo = Random.value;
                    if (shieldOrNo >ShieldChance)
                    {
                        newEnemy.GetComponent<Enemy>().SpawnShield();
                    }
                
                
               
                newEnemy.transform.parent = _enemyHolder.transform;    
    }
  


    IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(3f);
         
         
        if(!_isBossFight)
        {
        _enemiesSpawned = 0;
        _timeToSpawn -= .2f;
        _maxEnemies += 1;
        _level ++;
        StartCoroutine(ShowLevel());
        }

        else
        {
            StartCoroutine(ShowLevel());
        }   
        if (_level == _bossFightLevel)
        {
            Player.NewGame = false;
            LoadBossFight();
        }
                  
    }
    void LoadBossFight()
    {
        SceneManager.LoadScene(2);
    }
    IEnumerator ShowLevel()
    {
        if (_level == 999999999)
        {
            _levelText.text = "Boss Fight!";
        }
        else
        {
            _levelText.text = "Level: "+_level;
        }
        UIManager.Instance.UpdateLevel(_level);
        
        _levelText.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        string[] _hype = new string[] {"GET READY!", "RIP AND TEAR!", "NO MERCY!", "YIPPEE KAY YAY, SPACE COWBOY!",
         "RIP 'EM WITH YOUR LASER BEAMS!", "NO SURVIVORS!", "WARNING: OUT OF BUBBLEGUM!", "PEW PEW!","DISINTIGRATION IS STRENGTH!",
         "ANNHILATION IS SERVED!","THEY'RE AFRAID!","WHY DO THEY ALWAYS WANT TO DO IT THE HARD WAY?" };
        _levelText.text = _hype[Random.Range(0, _hype.Length)];
        yield return new WaitForSeconds(1f);
        _levelText.gameObject.SetActive(false);
        yield return new WaitForSeconds (2f);

    }

    void ChooseEnemy()
    {
        _weightedTotal = 0;

        var _maxSpawnEnemyNumber = _level;
        if (_maxSpawnEnemyNumber > _enemyPrefab.Length)
        {
            _maxSpawnEnemyNumber = _enemyPrefab.Length;
        }

        int[] enemyTable =
        {
            100, // 0 main enemy            
            90, // 2 sidewinder
            80, // 1 flip up
            75,  // rear shooter
            60, // 3 circler
            50,   // Dodger
            40,  // big boy
            30  //rammer
            
            
        };

        int[] enemyID =
        {
            0,  // main enemy
            2,  // sidewinder
            1,  // flip up
            6,  // rear shooter
            3,  // circler
            5,  //rammer  
            4,  // big boy                      
            7   // Dodger
        };

        if(_isBossFight)
        {
            _maxSpawnEnemyNumber = 7;
        }
        for(int i = 0; i < _maxSpawnEnemyNumber; i++)
        {
            _weightedTotal += enemyTable[i];
        }

        var randomNumber = Random.Range(0, _weightedTotal);
        var x = 0;
        foreach (var weight in enemyTable)
        {
             if(randomNumber <= weight)
                {
                    _spawnEnemyNumber = enemyID[x];
                    return;
                }                
                else
                {
                    x++;
                    randomNumber -= weight;
                }   
        }


    }
    IEnumerator SpawnPowerUpRoutine()
    {
        while(!_stopSpawning)
        {   
            if(!_isBossFight)
            {
                if(_firstPowerupSpawn)
                {
                    _firstPowerupSpawn = false;
                    yield return new WaitForSeconds(15f);
                }
            }
        
           

            float randomX = Random.Range(-xPos,xPos);
            Vector3 posToSpawn = new Vector3(randomX, Ypos,0);

            ChooseAPowerup();

            GameObject newPowerUp = Instantiate(_powerUps[_powerupToSpawn], posToSpawn,Quaternion.identity);
            float _powerupTime = Random.Range(_minpowerupTime,_maxpowerupTime);
            yield return new WaitForSeconds(_powerupTime);   
        }
    }

    void ChooseAPowerup()
    {
        _weightedTotal = 0;

           int[] powerupTable =
           {
               50, // ammo
               25, // missile  
               16, // health   
               8, // shield   
               6, // speed     
               3, // tripleshot 
               2 // negative           
           };
            int[] powerupToAward =
           {
               3, //ammo
               5, //missile
               4, //health
               2, //shield
               1, // speed
               0, // tripleshot
               6 // negative
           };

            foreach(var item in powerupTable)
            {
                _weightedTotal += item;
            }

            var randomNumber = Random.Range(0, _weightedTotal);
            var i = 0;
            
            foreach(var weight in powerupTable)
            {
                if(randomNumber <= weight)
                {
                    _powerupToSpawn = powerupToAward[i];
                    return;
                }                
                else
                {
                    i++;
                    randomNumber -= weight;
                }
            }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

    public void ForTheLoveOfGodPleaseStop()
    {
      SceneManager.LoadScene("GameOver");
    }

}
