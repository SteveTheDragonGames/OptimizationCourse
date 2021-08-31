using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField]
    private float _timeToDie = 3f;
    void Start()
    {
        Destroy(this.gameObject, _timeToDie);
    }
}
