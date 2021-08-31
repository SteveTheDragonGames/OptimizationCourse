using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField]
    GameObject _parentEnemy;



    void Start()
    {
    
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            return;
        }
        
        if(other.CompareTag("Player"))
        {
            Debug.Log("PLAYER DETECTED");
            _parentEnemy.GetComponent<Enemy>().playerNear = true;
        }
        

    }


}
