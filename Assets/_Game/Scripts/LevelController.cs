using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public string levelName;
    public int hasCleared;
    public UnityEngine.UI.Image key;
    void Start()
    {
        levelName = gameObject.name;
        key = GetComponent<UnityEngine.UI.Image>();
        hasCleared = PlayerPrefs.GetInt(levelName,0);
        key.color = (hasCleared == 1) ? Color.white : key.color;
    }
    public void LoadLevel()
    {
        PlayerPrefs.SetString("nivel", levelName);
        SceneManager.LoadScene("Procedural");
    }
}
