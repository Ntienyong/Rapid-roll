using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using TMPro;
using System.IO;

public class MenuUIController : MonoBehaviour
{
    public TextMeshProUGUI highScoreTextUI;
    void Start()
    {
        LoadHighScore();
        highScoreTextUI.text = "HIGHSCORE: " + GameManager.highScore;
    }
   public void StartNew()
   {
       SceneManager.LoadScene(1);
   }
   public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if(File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            GameManager.highScore = data.score;
        }
    }

   public void Exit()
    {
#if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
