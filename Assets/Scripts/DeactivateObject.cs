using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DeactivateMe",1f);
    }

    private void DeactivateMe()
    {
        this.gameObject.SetActive(false);
    }
}
