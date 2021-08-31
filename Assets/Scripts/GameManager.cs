using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get{
            if (_instance == null)
            {
                Debug.Log("GameManager is null");
            }
            return _instance;
        }
    }
    
    public bool _negativePowerupActivated = false;

    [SerializeField]
    private bool _isGameOver;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Update()
    {

        if(Input.GetKey(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(0); //Current game scene
        }

        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        
    }
    public void GameOver()
    {
        _isGameOver = true;
    }

    public void ActivateNegativePowerup1()
    {
        StartCoroutine(TemporaryHomingLasers());
    }

    IEnumerator TemporaryHomingLasers()
    {
        _negativePowerupActivated = true;
        yield return new WaitForSeconds(7f);
        _negativePowerupActivated = false;
    }

}
