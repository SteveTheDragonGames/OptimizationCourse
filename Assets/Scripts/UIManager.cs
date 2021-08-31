using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {      
            if(_instance == null)
            {
                Debug.LogError("UIManager is null!");
            }      
            return _instance;            
        }
    }

    [SerializeField]
    Canvas _canvas;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;

    [SerializeField]
    private Text _restartGameText;

    //Ammo count information
    private Text _ammoCountText;
    private int _ammoCount;
    private int _maxAmmo;
    [SerializeField]
    private Image _ammoHealthBar;
    [SerializeField]
    private Image _ammoIcon;
    [SerializeField]
    private Transform _AmmoHolder;

    private Image[] ammoBits;

    private Text _missileCountText;
    private int _missileCount;

    private bool _canRestart;

    private float _thrusterPower;
    private Image _thrusterPowerBar;

    [SerializeField] private Text _levelText;


    void Awake()
    {   if(Instance == null)
    {
        _instance = this;
        Debug.Log("Never mind, UIManager isn't null anymore.");
    }else
    {
        Destroy(gameObject);
    }

        _gameOverText.gameObject.SetActive(false);
        _restartGameText.gameObject.SetActive(false);

        _maxAmmo = _ammoCount = Player.Instance.ammoCount;
        ammoBits = new Image[_maxAmmo];
        _ammoCountText = GameObject.Find("AmmoText").GetComponent<Text>();

        _missileCountText = GameObject.Find("Missile_Text").GetComponent<Text>();
        _missileCount = Player.Instance.missileCount;
        _thrusterPowerBar = GameObject.Find("ThrusterPowerBar").GetComponent<Image>();
    }
    void Start()
    {
        InitializeAmmoCounter();
        UpdateAmmoCount();
        UpdateMissileCount();
        UpdateScore(0);
        UpdateThrusterCount();
    }

    void InitializeAmmoCounter()
    {
        for (int i=0; i < _maxAmmo; i++)
        {
            ammoBits[i] = Instantiate(_ammoIcon, new Vector2(0,0), Quaternion.identity);
            ammoBits[i].transform.SetParent(_AmmoHolder);
            ammoBits[i].transform.localPosition = new Vector3(0,0,0);
            ammoBits[i].transform.localPosition = new Vector3(i*6,0,0);
            ammoBits[i].enabled = false;
        }
    }

    public void UpdateAmmoCount()
    {      
        _ammoCount = Player.Instance.ammoCount;
        _ammoCountText.text = "Ammo: "+_ammoCount.ToString();      
        //turn off all the bullets
        for (int i=0; i < _maxAmmo ;i++)
        {
            if(ammoBits != null)
            ammoBits[i].enabled = false;
        }
        //turn all the bullets on until the end of _ammoCount.
        for(int i = 0; i < _ammoCount; i++)
        {
            ammoBits[i].enabled = true;
        }
    }
    public void UpdateMissileCount()
    {
        _missileCount = Player.Instance.missileCount;
        _missileCountText.text = "Missiles: " + _missileCount;
    }

    public void UpdateThrusterCount()
    {
        _thrusterPower = Player.Instance._thrusterPower;
        _thrusterPowerBar.fillAmount = _thrusterPower/100;

    }

    public void UpdateScore(int playerScore)
    {
        scoreText.text = "Score: "+playerScore;
    }

    public void UpdateLevel(int level)
    {
        if(_levelText != null)
        {
            _levelText.text = "Level: "+level;
        }
        
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];
    }

    public void GameOverText()
    {
        _canRestart=true;
        _gameOverText.gameObject.SetActive(true);
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        StartCoroutine(ShowRestartText());

        while(_canRestart)
        {
            yield return new WaitForSeconds(.75f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(.5f);
            _gameOverText.gameObject.SetActive(true);
        }
    }

    IEnumerator ShowRestartText()
    {
        yield return new WaitForSeconds(3);
        _restartGameText.gameObject.SetActive(true);
    }

}
