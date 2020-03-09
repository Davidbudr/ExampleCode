using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelProgressor : MonoBehaviour
{
    public List<Enemy> Enemies = new List<Enemy>();
    [Tooltip("This is the upper end health of each tier")]
    public int[] Tiers = new int[3];

    [Space]
    [Header("Health Information")]
    public int PlayerMaxHealth;
    public int PlayerHealth;

    [Space]
    [Header("Display Information")]

    public GameObject EnemyObj;
    public GameObject EnemyHome;
    public Text PlayerHealthText;
    public Text EnemyHealthText;
    
    private void Start()
    {
        PlayerHealthText = GameObject.Find("PlayerHealth").GetComponent<Text>();
        EnemyHealthText = GameObject.Find("EnemyHealth").GetComponent<Text>();
        
        EnemyObj = Instantiate(Enemies[0].Prefab);
        EnemyObj.transform.position = new Vector2(4f, 2.5f);
        EnemyObj.GetComponent<EnemyScript>().HomePos = EnemyHome.transform.position;
    }
    void Update()
    {
        if (Enemies.Count > 0)
        {
            if (EnemyObj == null)
            {
                EnemyObj = Instantiate(Enemies[0].Prefab);
                EnemyObj.transform.position = new Vector2(4f, 2.5f);
                EnemyObj.GetComponent<EnemyScript>().HomePos = EnemyHome.transform.position;
            }
            PlayerHealthText.text = PlayerHealth + "/" + PlayerMaxHealth;
            EnemyHealthText.text = Enemies[0].Health.ToString();
            if (Enemies[0].Health <= 0)
            {
                Enemies.RemoveAt(0);
                Destroy(EnemyObj, 1f);
            }
        }
        if (Enemies.Count <= 0)
        {
            Invoke("win", 1.5f);
        }
        else if (PlayerHealth <= 0)
        {
            Invoke("lose", 1.5f);
        }
    }
    void win()
    {

        int _rank = rankreturn();
        GameObject g = new GameObject("Progressor", typeof(Completer));
        Completer gc = g.GetComponent<Completer>();
        gc.Rank = _rank;
        gc.LevelName = SceneManager.GetActiveScene().name;
        DontDestroyOnLoad(g);
        SceneManager.LoadScene("WinScene");
    }
    void lose()
    {

    }

    int rankreturn()
    {
        for(var i = 0; i < Tiers.Length; i++)
        {
            if (PlayerHealth <= Tiers[i])
            {
                return i + 1;
            }
        }

        return 3;
    }
}
[Serializable]
public class Enemy
{
    [Tooltip("1 = Red \n2 = Green \n3 = Yellow \n4 = Blue")]
    public int TypeID;
    public GameObject Prefab;
    public int Health;
}
