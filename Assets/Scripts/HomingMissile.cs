using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    private GameObject target;
    Rigidbody2D rb;
    private float _thrustSpeed = 5f;
    private float rotationSpeed = 75f;
    [SerializeField]
    private GameObject _missileGoBoom;
    public GameObject DummyEnemy;

    private float minHeight, maxHeight, minWidth, maxWidth;
    [SerializeField]
    private float _lifeOfMissile = 5f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Enemy");
        minHeight = -6f;
        maxHeight = 6f;
        minWidth = -9f;
        maxWidth = 9f;
        StartCoroutine(SelfDestruct());
        //StartCoroutine(RandomTarget());
        StartCoroutine(FindNewTarget());

    }

    // Update is called once per frame
    void Update()
    {
        

        if(target)
        {
        
        var dir = (target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var rotateToTarget = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTarget,Time.deltaTime * rotationSpeed);
        rb.velocity = new Vector2(dir.x*_thrustSpeed,dir.y*_thrustSpeed);

        }
        else
        {
            transform.Translate(Vector3.forward * _thrustSpeed * Time.deltaTime);
            //I know this should be Vector3.Up but the forward seems to be a happy accident and makes
            //the missiles look pretty cool by going into the z axis.

        /*
        var dir = (DummyEnemy.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var rotateToTarget = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotateToTarget,Time.deltaTime * rotationSpeed);
        rb.velocity = new Vector2(dir.x*_thrustSpeed/10,dir.y*_thrustSpeed/10);
        */
        
        }

        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            return;
        }
        if(other.CompareTag("Enemy"))
        {   
            if(other.gameObject.GetComponent<Enemy>() !=null)
            {
                other.gameObject.GetComponent<Enemy>().DestroyEnemyShip();
            }          
             
             Destroy(this.gameObject);
        }
       
    }

    IEnumerator RandomTarget()
    {
        yield return new WaitForSeconds(1f);
        //do nothing

        /*
        while (true)
        {
            yield return new WaitForSeconds(.4f);
            var randomX = Random.Range(minWidth,maxWidth);
            var randomY = Random.Range(minHeight,maxHeight);
            Vector3 randomPos = new Vector3(randomX, randomY, 0);
            DummyEnemy.transform.position = randomPos;
        }
        */
    }
    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(_lifeOfMissile);
        target = null;
        DestroyThisMissile();

    }

    private void DestroyThisMissile()
    {
        Instantiate(_missileGoBoom, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    IEnumerator FindNewTarget()
    {
        while(target == null)
        {
            yield return new WaitForSeconds(.2f);
            target = GameObject.FindWithTag("Enemy");
        }
        
    }
    private void CheckBorders()
    {
        if (transform.position.x <= minWidth || transform.position.x >= maxWidth)
        {
            rb.velocity = new Vector2(-_thrustSpeed,_thrustSpeed);
        }
        if (transform.position.y <= minHeight || transform.position.y >= maxHeight)
        {
            rb.velocity = new Vector2(_thrustSpeed,-_thrustSpeed);
        }
    }
}
