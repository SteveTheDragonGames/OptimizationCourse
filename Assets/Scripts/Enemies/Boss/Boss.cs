using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
  
    [SerializeField] private Text _debugText;
    [SerializeField] float _speed = 1f;
    [SerializeField] float _timeToWaitBetweenSections = 1f;
    [SerializeField] protected List<GameObject> _waypoints = new List<GameObject>();
    [SerializeField] protected GameObject[] _sections = new GameObject[10];

    [SerializeField] private GameObject _destructionHolder;
    [SerializeField] private GameObject _explodePosition;


  
    protected int _currentWaypoint;

    bool _canMove = true;

    private int _deathCount = 0;

    void OnEnable()
    {
        BossSection.deleteSection += IgnoreSectionNow;
    }

    void OnDisable()
    {
        BossSection.deleteSection -= IgnoreSectionNow;
    }

    //cycle through all 10 sections
    //if a section is destroyed
    //skip over to the next section
    //continue until you find a live section
    //move there and continue cycling.

    //if a section is destroyed while your in it,
    //move on to the next section.

    void Start()
    {
        
        SpawnManager.Instance._isBossFight = true;
        SpawnManager.Instance.StartSpawning();
        _currentWaypoint = 0;
        _destructionHolder.gameObject.SetActive(false);
       
             
        StartCoroutine(MoveThroughWaypoints());
        
        
    }
  
    IEnumerator MoveThroughWaypoints()
    {
        while(_canMove)
        {
            MoveTowardsWaypoint();
        
            if(Vector3.Distance(transform.position,_waypoints[_currentWaypoint].transform.position ) <0.5f)
            {
                if(_waypoints.Count <=1)
                {
                    _canMove = false;
                }

                yield return new WaitForSeconds(_timeToWaitBetweenSections);
                ChooseNextWaypoint();
            }
            yield return 0;
        }
        yield return null;       

    }

    void MoveTowardsWaypoint()
    {
        transform.position = Vector3.MoveTowards(transform.position, 
            _waypoints[_currentWaypoint].transform.position, Time.deltaTime * _speed);
        
        
    }
    void ChooseNextWaypoint()
    {
        _currentWaypoint ++;  
            
        if(_currentWaypoint >= _waypoints.Count)
        {
            _currentWaypoint = 0;
        }
    }

    void IgnoreSectionNow(int id)
    {   
        string x = "Path ("+id+")";
        for(int i = _waypoints.Count-1; i>=0 ; i--)
        {
            if(x == _waypoints[i].transform.name)
            {
                _waypoints.RemoveAt(i);
                _deathCount++;
                if(_deathCount == 10)
                {
                    Die();
                }
                ChooseNextWaypoint();
                break;

            }            
        } 
    }

    void Die()
    {
        _destructionHolder.gameObject.SetActive(true);
        StartCoroutine(Fall());
    }

    IEnumerator Fall()
    {
        float x = 15;
        while (x >=0)
        {
            transform.Translate(new Vector3(0f,0f,2f) * Time.deltaTime);
            transform.Rotate(0f,0f,1f * Time.deltaTime);
            x-=Time.deltaTime;
            
            yield return 0;            
        }
        GameObject kaboom = Instantiate(Resources.Load("Explosion1"),this.transform.position, Quaternion.identity) as GameObject;
        kaboom.transform.parent = transform.parent;
        kaboom.transform.position = new Vector3(_explodePosition.transform.position.x,
        _explodePosition.transform.position.y,
        _explodePosition.transform.position.z);
        kaboom.transform.localScale = new Vector3(6f,6f,6f);
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
        SpawnManager.Instance.ForTheLoveOfGodPleaseStop();
        yield return 0;
    }

}
