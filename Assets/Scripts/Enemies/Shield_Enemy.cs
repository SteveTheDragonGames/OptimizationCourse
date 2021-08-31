using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_Enemy : MonoBehaviour
{
    Animator _anim;

    void Awake() { _anim = GetComponent<Animator>(); }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")){ return; }
        if (other.CompareTag("EnemyLaser")) { return; }
        if (other.CompareTag("Player")) { Player.Instance.Damage(); }
        if (other.CompareTag("Laser")) 
        { 
            Destroy(other.gameObject);
            StartCoroutine(DestroyShield());  
        }
    }
    IEnumerator DestroyShield()
    {
        _anim.SetTrigger("isShot");
        _anim.Play("EnemyShield_anm");
        yield return new WaitForSeconds(.3f);
        Destroy(gameObject);
    }
}
