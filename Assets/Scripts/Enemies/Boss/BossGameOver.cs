using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGameOver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.GameOver();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
