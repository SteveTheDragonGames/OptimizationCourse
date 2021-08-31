using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class target : MonoBehaviour
{    
    [SerializeField] private int _hp = 2;
    private int _maxHP;
    [SerializeField] private int _points = 10;
    Collider2D _col2D;


    void Start()
    {
        _maxHP = _hp;
        _col2D = GetComponent<Collider2D>();        
    }



    void OnTriggerEnter2D(Collider2D other)
    {     

             if(other.CompareTag("Laser"))
            {
                if(_col2D.enabled)
                {
                    _hp--;
                
                    if (_hp <= _maxHP)
                    {
                    Instantiate(Resources.Load("Explosion_Smaller"),transform.position, Quaternion.identity);
                    }
                    if (_hp <=0)
                    {
                        Player.Instance.AddScore(_points);
                        GameObject explosion = Instantiate(Resources.Load("Explosion1"), transform.position, Quaternion.identity) as GameObject;
                        explosion.transform.localScale = transform.localScale;
                        Destroy(this.gameObject);
                    }
                    Destroy(other.gameObject);
                }
                
            }
    }
}
