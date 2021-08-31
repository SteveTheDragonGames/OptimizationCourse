using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{ 
    public Camera mainCamera;
    private float _timeToShake;
    private float _shakeLimit;
    private float _cameraZ = -10f;   


    // Start is called before the first frame update
    void Start()
    {           
        mainCamera = Camera.main;
        _timeToShake=0f;
        _shakeLimit=.1f;
    }

    // Update is called once per frame
    void Update()
    {
        _timeToShake -= Time.deltaTime;
        if (_timeToShake==0)
        {
            mainCamera.transform.position = new Vector3(0,0,_cameraZ);
            StopCoroutine("CameraShake");
        }
        if (_timeToShake <0)
        {
            _timeToShake = 0;
        }
    }   
    public void InitiateShake(float _incomingTimeToShake)
    {
        _timeToShake=_incomingTimeToShake;
        StartCoroutine("CameraShake");
    }
    
    IEnumerator CameraShake()
    {
        if (_timeToShake>0)
        {
        yield return new WaitForSeconds(.03f);

        var x = Random.Range(-_shakeLimit, _shakeLimit);
        var y = Random.Range(-_shakeLimit, _shakeLimit);
        var newPos = new Vector3(x,y,_cameraZ);
        mainCamera.transform.position = newPos;
        
        StartCoroutine("CameraShake");
        }
    }
}
