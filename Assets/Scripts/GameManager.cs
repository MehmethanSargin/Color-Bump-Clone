using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Text currentLevelText, nextLevelText;
    private Image fill;

    private int level;
    private float startDistance, distance;

    private GameObject player, finish, hand;
    private TextMesh levelNumber;
    

    private void Awake()
    {
        currentLevelText = GameObject.Find("CurrentLevelText").GetComponent<Text>();
        nextLevelText = GameObject.Find("NextLevelText").GetComponent<Text>();
        fill = GameObject.Find("Fill").GetComponent<Image>();

        player = GameObject.Find("Player");
        finish = GameObject.Find("Finish");
        hand = GameObject.Find("Hand");

        levelNumber = GameObject.Find("LevelNumber").GetComponent<TextMesh>();
    }

    private void Start()
    {
        level = PlayerPrefs.GetInt("Level");

        levelNumber.text = "Level " + level;

        nextLevelText.text = level + 1 + "";
        currentLevelText.text = level.ToString();
        startDistance = Vector3.Distance(player.transform.position, finish.transform.position);
    }

    private void Update()
    {
        distance = Vector3.Distance(player.transform.position, finish.transform.position);
        if (player.transform.position.z < finish.transform.position.z)
        {
            fill.fillAmount = 1 - (distance / startDistance);
        }
    }

    public void RemoveUI()
    {
        hand.SetActive(false);
    }
}
