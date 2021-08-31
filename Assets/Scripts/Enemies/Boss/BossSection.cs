using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BossSection : MonoBehaviour
{
    public delegate void DeleteSection(int sectionID);
    public static event DeleteSection deleteSection;

    [SerializeField] int _sectionID;
    [SerializeField] int _childrenCount = 1;
    [SerializeField] GameObject[] _Layers;
    [SerializeField] protected int _currentLayer;
    int currentCount;
    bool sentMessage = false;
    [SerializeField] int _points = 1000;

  

   void Start()
   {    
        _currentLayer = _Layers.Length;
   }

   //turn on the final layer
   //wait for all children inside layer to disappear
   //deactivate layer and turn on the next layer downward
   //repeat until all layers are destroyed
   //set a flag that this id is done.    
 
    void Update()
    {   
        CheckChildren();
        CheckLayers();
    }

    void CheckChildren()
    {
         if(transform.childCount == _childrenCount)
        {        
                if(!this.sentMessage)
                { 
                    Player.Instance.AddScore(_points);
                    deleteSection(_sectionID); 
                    this.sentMessage = true;
                    //HUGE thank you to Paul Marsh for the clue this was constantly on.
                }
        }
    }

    public void CheckLayers()
    {
        currentCount = 0;
        for (int x = 0; x <= _Layers.Length-1; x++)
        {
            if (_Layers[x] != null)
            {
                currentCount++;
            }
            
        }
        if (currentCount < _currentLayer)
        {
            
            _currentLayer = currentCount;
            
            Collider2D[] colliders = _Layers[_currentLayer-1].GetComponentsInChildren<Collider2D>();
            
            foreach(Collider2D collider in colliders)

            {
                collider.enabled = true;
            }
            
        }
         
    }
}
