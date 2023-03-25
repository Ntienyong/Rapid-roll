using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
public class GameManager : MonoBehaviour
{
    public GameObject[] platforms;
    public GameObject[] powerup;
    public GameObject player;
    public GameObject[] coins;
    public Transform platformParent;
    private Vector3 _playerStartPos;
    private Vector3 _spawnPos;
    private Vector3 _randomStartPos;
    public Vector3 _reSpawnPos;
    public static GameManager instance = null;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI playerLifeText;
    public TextMeshProUGUI gameoverText;
    public TextMeshProUGUI countDown;
    public TextMeshProUGUI highScoreText;
    public Button restartButton;
    public PlayerController _controlPlayer;
    private float _startSpawnTime = 1.0f;
    private float _contSpawnTime = 1.0f;
    private float _yStart = 4.5f;
    private float _xSpawnPosRange = 5.3f;
    private float _ySpawnPos = 5.5f;
    public int _countDown = 4;
    public bool startGame;
    public bool gameover;
    public bool gameActive;
    public int score;
    public static int highScore;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        gameActive = true;
        StartCoroutine(StartGametimer());
        StartGameMechanics();
        StartCoroutine(CalculateSpawnPos());
    }

    // Update is called once per frame
    void Update()
    {
        _reSpawnPos = platformParent.GetChild(2).transform.position;
        Vector3 _powerupSpawnPos = platformParent.GetChild(0).transform.position;
        LoadHighScore();
        if(score >= highScore)
        {
            SaveHighScore();
        }
        
        PauseGame();
    }

    void StartGameMechanics()
    {
        for (int i = 0; i < 4; i++)
        {
            _randomStartPos = new Vector3(Random.Range(-5.3f, 5.3f), _yStart, 0);
            Instantiate(platforms[0], _randomStartPos, transform.rotation, platformParent);
            _yStart -= 2.0f;
            if (i == 2)
            {
                _playerStartPos = _randomStartPos + Vector3.up;

            }

        }

        Instantiate(player, _playerStartPos, transform.rotation);
        InvokeRepeating("SpawnPlatforms", _startSpawnTime, _contSpawnTime);
    }

    public IEnumerator CalculateSpawnPos()
    {
        _spawnPos = new Vector3(Random.Range(-_xSpawnPosRange, _xSpawnPosRange), -_ySpawnPos, 0);
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(CalculateSpawnPos());

    }

    void SpawnPlatforms()
    {
        if (startGame == true)
        {
            //_spawnPos = new Vector3(Random.Range(-_xSpawnPosRange, _xSpawnPosRange), -_ySpawnPos, 0);

            Instantiate(platforms[0], _spawnPos, platforms[0].transform.rotation, platformParent);
        }
    }

    public IEnumerator SpawnBadPlatforms()
    {
        //_spawnPos = new Vector3(Random.Range(-_xSpawnPosRange, _xSpawnPosRange), -_ySpawnPos, 0);
        yield return new WaitForSeconds(Random.Range(2.0f, 15.0f));
        Instantiate(platforms[1], _spawnPos, platforms[1].transform.rotation);
        StartCoroutine("SpawnBadPlatforms");
    }

    public IEnumerator SpawnPowerup()
    {
        int powerupIndex = Random.Range(0, powerup.Length);
        yield return new WaitForSeconds(10.0f);
        Instantiate(powerup[powerupIndex], _spawnPos + new Vector3(0, 1.0f, 0), transform.rotation);
        StartCoroutine(SpawnPowerup());
    }

    public IEnumerator SpawnCoins()
    {
        int coinsIndex = Random.Range(0, coins.Length);
        while(true)
        {
            yield return new WaitForSeconds (Random.Range(5.0f, 15.0f));
            Instantiate(coins[coinsIndex], _spawnPos + new Vector3(Random.Range(-_xSpawnPosRange, _xSpawnPosRange), 0, 0), transform.rotation);
        }
        
    }

    public IEnumerator StartGametimer()
    {
        while (_countDown > 0)
        {
            yield return new WaitForSeconds(1.0f);
            _countDown--;
            countDown.text = " " + _countDown + " ";
            if (_countDown == 0)
            {
                countDown.gameObject.SetActive(false);
                startGame = true;
                StartCoroutine(CountScore());
                StartCoroutine(GameSpeedController());
                StartCoroutine(SpawnCoins());
                StartCoroutine("SpawnBadPlatforms");
                StartCoroutine(SpawnPowerup());
            }
        }
    }

    public IEnumerator CountScore()
    {
        yield return new WaitForSeconds(0.05f);
        score++;
        scoreText.text = "Score: " + score;
        StartCoroutine(CountScore());
    }

    public IEnumerator GameSpeedController()
    {
        int gameTime = 0;
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            gameTime++;

            if(gameTime >= 60 && !gameover)
            {
                Time.timeScale = 1.1f;
            }
            if(gameTime >= 120 && !gameover)
            {
                Time.timeScale = 1.2f;
            }
            if(gameTime >= 180 && !gameover)
            {
                Time.timeScale = 1.3f;
            }
            if(gameTime >= 240 && !gameover)
            {
                Time.timeScale = 1.4f;
            }
            if(gameTime >= 300 && !gameover)
            {
                Time.timeScale = 1.5f;
            }
            if(gameTime >= 360 && !gameover)
            {
                Time.timeScale = 1.6f;
            }
            if(gameTime >= 420 && !gameover)
            {
                Time.timeScale = 1.7f;
            }
            if(gameTime >= 580 && !gameover)
            {
                Time.timeScale = 1.8f;
            }
        }
    }

    public void GameOver()
    {
        restartButton.gameObject.SetActive(true);
        gameoverText.gameObject.SetActive(true);
        highScoreText.gameObject.SetActive(true);
        highScoreText.text = "HIGHSCORE: " + GameManager.highScore;
        gameover = true;
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Waterdrop");
    }

    void PauseGame()
    {
        if(Input.GetKeyDown(KeyCode.Space) && gameActive)
        {
            Time.timeScale = 0;
            gameActive = false;
        }

        else if(Input.GetKeyDown(KeyCode.Space) && !gameActive)
        {
            Time.timeScale = 1;
            gameActive = true;
        }
    }

    public IEnumerator ScoreXPU()
    {
        int secondsToWait = 15;
        while(secondsToWait > 0)
        {
            score += 100;
            yield return new WaitForSeconds(1.0f);
            secondsToWait--;

            if(secondsToWait == 0)
            {
                break;
            }
        }
            
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();
        data.score = score;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            highScore = data.score;
        }
    }
}

    [System.Serializable]
    class SaveData
    {
        public int score;
    }
