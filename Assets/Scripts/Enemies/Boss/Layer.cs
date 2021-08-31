using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layer : BossSection
{
    void Update()
    {
        if(transform.childCount == 0)
        {   
            Player.Instance.AddScore(100);

            base._currentLayer --;
            Destroy(this.gameObject);
            
        }
    }
}
