using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public int moveSpeed;
    private float _yDestroyBound = 6.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GameManager.instance.startGame == true)
        {
            MoveUp();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);

        if(transform.position.y > _yDestroyBound)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    { 
        if(other.gameObject.CompareTag("GoodPlat"))
        {
            Destroy(this.gameObject);
        }
    }
    
}
