using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public float fallTime = 0.5f;
    private float previousTime;
    public static int height = 20;
    public static int width = 10;
    public Vector3 rotationPoint;
    public static Transform[,] grid = new Transform[width, height];

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            transform.position += new Vector3(-1, 0, 0);
            if(!ValidMode())
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position += new Vector3(1, 0, 0);
            if (!ValidMode())
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        }

        if(Input.GetKey(KeyCode.S))
        {
            if (Time.time - previousTime > (fallTime/10))
            {
                transform.position += new Vector3(0, -1, 0);
                previousTime = Time.time;
                if (!ValidMode())
                {
                    transform.position += new Vector3(0, 1, 0);
                    this.enabled = false;
                    AddToBoard();
                    FindObjectOfType<SpawnPlaceScript>().SpawnBlock();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint),new Vector3(0,0,1), 90);
            if(!ValidMode())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), -90);
            }
        }

        if (Time.time - previousTime > fallTime)
        {
            transform.position += new Vector3(0, -1, 0);
            previousTime = Time.time;
            if (!ValidMode())
            {
                transform.position += new Vector3(0, 1, 0);
                this.enabled = false;
                AddToBoard();
                FindObjectOfType<SpawnPlaceScript>().SpawnBlock();
            }
        }
    }

    bool ValidMode()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            if (roundedX < 0 || roundedY < 0 || roundedX >= width || roundedY >= height)
            {
                return false;
            }

            if(grid[roundedX,roundedY] != null)
            {
                return false;
            }
        }
        return true;
    }

    void AddToBoard()
    {
        foreach (Transform children in transform)
        {
            int roundedX = Mathf.RoundToInt(children.transform.position.x);
            int roundedY = Mathf.RoundToInt(children.transform.position.y);

            grid[roundedX, roundedY] = children;
            if(roundedY>=height-1)
            {
                EndGame();
            }
        }

        CheckLines();
    }

    bool HaveLine(int i)
    {
        for(int j=0; j<width; j++)
        {
            if(grid[j,i]==null)
            {
                return false;
            }
        }
        return true;
    }

    void DeleteLine(int i)
    {
        for (int j = 0; j < width; j++)
        {
            Destroy(grid[j, i].gameObject);
            grid[j,i] = null;
        }
    }

    void MoveDown(int i)
    {
        for (int j = i; j < height; j++)
        {
            for(int a=0; a<width; a++)
            {
                if(grid[a,j]!=null)
                {
                    grid[a, j-1] = grid[a, j];
                    grid[a, j] = null;
                    grid[a, j - 1].transform.position += new Vector3(0, -1, 0);
                }
            }
        }
    }

    void CheckLines()
    {
        for(int i=height-1; i>=0; i--)
        {
            if(HaveLine(i))
            {
                DeleteLine(i);
                MoveDown(i);
                FindObjectOfType<SpawnPlaceScript>().score += 10;
            }
        }
    }

    void EndGame()
    {
        SceneManager.LoadScene("Menu");
    }
}
