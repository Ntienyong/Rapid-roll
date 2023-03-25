using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int speed;
    public int playerLife;
    public bool hasShield;
    private float _gravityModifier;
    private float _xBoundary = 5.8f;
    private Rigidbody _playerRb;
    private float _addGravity;
    private GameManager _manager;
    public GameObject shieldIndicator;
    GameObject shieldInd;

    // Start is called before the first frame update
    void Start()
    {
        _gravityModifier = 7000f;
        _playerRb = GetComponent<Rigidbody>();
        //Physics.gravity *= gravityModifier;
        _manager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    void Update()
    {
       if(hasShield == true)
        {
            shieldInd.transform.position = transform.position;
        } 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GameManager.instance.startGame == true)
        {
            Movement();
        }

        _manager.playerLifeText.text = "XP: " + playerLife;

    }

    void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        _playerRb.velocity = (Vector3.right * speed * horizontalInput);
        
        if(transform.position.x >= _xBoundary)
        {
            transform.position = new Vector3(_xBoundary, transform.position.y,transform.position.z);
        }

        if(transform.position.x <= -_xBoundary)
        {
            transform.position = new Vector3(-_xBoundary, transform.position.y,transform.position.z);
        }
        _playerRb.AddForce((Vector3.down * _gravityModifier * Time.deltaTime), ForceMode.Acceleration);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("BadPlat") && !hasShield)
        {
            transform.position = _manager._reSpawnPos + new Vector3(0, 0.34f, 0);
            playerLife -=1;
            _manager.playerLifeText.text = "XP: " + playerLife;

            if(playerLife <= 0)
            {
            playerLife = 0;
            Destroy(gameObject);
            _manager.GameOver();
            }
        }

        else if(other.gameObject.CompareTag("BadPlat") && hasShield)
        {
            Destroy(other.gameObject);
        }

        else if(other.gameObject.CompareTag("Top Sensor"))
        {
            transform.position = _manager._reSpawnPos + new Vector3(0, 0.34f, 0);
            playerLife -=1;
            _manager.playerLifeText.text = "XP: " + playerLife;

            if(playerLife <= 0)
            {
            playerLife = 0;
            Destroy(gameObject);
            _manager.GameOver();
            }
        }

        else if(other.gameObject.CompareTag("Low Sensor"))
        {
            transform.position = _manager._reSpawnPos + new Vector3(0, 0.34f, 0);
            playerLife -=1;
            _manager.playerLifeText.text = "XP: " + playerLife;

            if(playerLife <= 0)
            {
            playerLife = 0;
            Destroy(gameObject);
            _manager.GameOver();
            }
        }
        else if(other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("ExtraLife"))
        {
            playerLife += 1;
            Destroy(other.gameObject);
        }

        else if(other.gameObject.CompareTag("ScoreX PU"))
        {
            Destroy(other.gameObject);
            GameManager.instance.StartCoroutine("ScoreXPU");
        }

        else if(other.gameObject.CompareTag("Magnet PU"))
        {

            Destroy(other.gameObject);
        }

        else if(other.gameObject.CompareTag("Shield PU"))
        {
            shieldInd = Instantiate(shieldIndicator, transform.position, transform.rotation, this.gameObject.transform);
            hasShield = true;
            Destroy(other.gameObject);
            StartCoroutine(ShieldLifeSpan());
        }
    }

    public IEnumerator ShieldLifeSpan()
    {
        yield return new WaitForSeconds(5.0f);
        hasShield = false;
        Destroy(shieldInd);
    }

}
