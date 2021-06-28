using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnPlaceScript : MonoBehaviour
{
    public int score = 0;
    public Text scoreText;
    public GameObject[] blocks;

    // Start is called before the first frame update
    void Start()
    {
        SpawnBlock();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnBlock()
    {
        Instantiate(blocks[Random.Range(0, blocks.Length)], transform.position, Quaternion.identity);
        scoreText.text = "Score: " + score;
    }
}
