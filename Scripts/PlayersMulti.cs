﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayersMulti : MonoBehaviour

{
    public GameObject playerAbove,playerBelow;
    int totalSizePucksAbove;
    int totalSizePucksBelow;

    private List<GameObject> playerAboveList = new List<GameObject>();
    private List<GameObject> playerBelowList = new List<GameObject>();


    public float moveSpeed = 100f;
    public float turnSpeed = -150f;

    Player pickedPlayerAbove;
    Player pickedPlayerBelow;

    int pickedPlayerAboveIndex = 0;
    int pickedPlayerBelowIndex = 0;


    float blockY;
    private bool isAnimPlayed = false;

    [SerializeField] GameObject blockLeft, blockRight;
    bool isMovedAbove = false;
    bool isMovedBelow = false;
    private bool hasStartedAbove = false;
    private bool hasStartedBelow = false;
    private bool isFinishedAbove = false;
    private bool isFinishedBelow = false;

    public Text winText;
    public GameObject restartCanvas;
    public Sprite belowArrow, aboveArrow;

    // Start is called before the first frame update
    void Start()
    {
        blockY = blockLeft.transform.position.y;
        blockLeft.transform.position = new Vector2(Random.Range(-11f, -9.5f), blockLeft.transform.position.y);
        blockRight.transform.position = new Vector2(Random.Range(3f, 4.5f), blockLeft.transform.position.y);

        totalSizePucksAbove = Random.Range(3, 5);
        totalSizePucksBelow = Random.Range(2, 4);

        for (int i = 0; i < totalSizePucksAbove; i++)
        {
            Vector3 vector3 = new Vector3(transform.position.x + Random.Range(-2.21f, 2.21f), transform.position.y + Random.Range(1.5f, 4.5f), transform.position.z);
            GameObject playerAboveObj = Instantiate(playerAbove, vector3, transform.rotation) as GameObject;
            playerAboveObj.GetComponent<Player>().RotatePlayer(false,90);
            playerAboveList.Add(playerAboveObj);

        }
        for (int i = 0; i < totalSizePucksBelow; i++)
        {
            Vector3 vector3 = new Vector3(transform.position.x + Random.Range(-2.21f, 2.21f), transform.position.y + Random.Range(-4.56f, -1.56f), transform.position.z);
            GameObject playerBelowObj = Instantiate(playerBelow, vector3, transform.rotation) as GameObject;
            playerBelowObj.GetComponent<Player>().RotatePlayer(true,90);

            playerBelowList.Add(playerBelowObj);
        }
        ChoosePlayerBelow();
        ChoosePlayerAbove();
    }

    private void ChoosePlayerBelow()
    {
        for (int i = 0; i < playerBelowList.Count; i++)
        {
            if (i == 0)
            {
                pickedPlayerBelow = playerBelowList[i].GetComponent<Player>();
                pickedPlayerBelowIndex = 0;

            }

            if (i + 1 != playerBelowList.Count)
            {
                if (playerBelowList[i + 1].GetComponent<Player>().transform.position.y >= pickedPlayerBelow.transform.position.y)
                {
                    pickedPlayerBelow = playerBelowList[i + 1].GetComponent<Player>();
                    pickedPlayerBelowIndex = i + 1;
                }
                else
                {
                    pickedPlayerBelow = playerBelowList[i].GetComponent<Player>();
                    pickedPlayerBelowIndex = i;

                }
            }

        }
        for (int i = 0; i < playerBelowList.Count; i++)
        {
            if (playerBelowList[i].GetComponent<Player>() != pickedPlayerBelow)
            {
                playerBelowList[i].GetComponent<Player>().arrow.SetActive(false);
            }
        }

    }

    private void ChoosePlayerAbove()
    {
        for (int i = 0; i < playerAboveList.Count; i++)
        {
            if (i == 0)
            {
                pickedPlayerAbove = playerAboveList[i].GetComponent<Player>();
                pickedPlayerAboveIndex = 0;

            }

            if (i + 1 != playerAboveList.Count)
            {
                if (playerAboveList[i + 1].GetComponent<Player>().transform.position.y < pickedPlayerAbove.transform.position.y)
                {
                    pickedPlayerAbove = playerAboveList[i + 1].GetComponent<Player>();
                    pickedPlayerAboveIndex = i + 1;
                }
                else
                {
                    pickedPlayerAbove = playerAboveList[i].GetComponent<Player>();
                    pickedPlayerAboveIndex = i;

                }
            }

        }
        for (int i = 0; i < playerAboveList.Count; i++)
        {
            if (playerAboveList[i].GetComponent<Player>() != pickedPlayerAbove)
            {
                playerAboveList[i].GetComponent<Player>().arrow.SetActive(false);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!isAnimPlayed)
        {

            CheckForeignPucksForBelow();
            CheckForeignPucksForAbove();

            // First player
            UpdateBelow();
            // Second player
            UpdateAbove();


        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MultiGame");
        }
    }

    private void UpdateAbove()
    {
        if (isAllPucksBelowBlock())
        {
            finishGame(false);

        } else
        {
            LaunchOnTouchAbove();

            if (pickedPlayerAbove.arrow.activeSelf)
            {
               TouchControl(pickedPlayerAbove, false);
            }
        }
    
    }

    private void CheckForeignPucksForAbove()
    {
        for(int i=0; i<playerBelowList.Count; i++)
        {
            if (playerBelowList[i].transform.position.y >= blockY)
            {
                GameObject playerBelowObj = playerBelowList[i];
                playerBelowList.RemoveAt(i);

                playerBelowObj.GetComponent<Player>().RotatePlayer(false, 180);
                playerBelowObj.transform.Find("Arrow").GetComponent<SpriteRenderer>().sprite = aboveArrow;

                playerAboveList.Add(playerBelowObj);
            }
        }
        if (pickedPlayerBelow.transform.position.y >= blockY)
        {
            pickedPlayerBelow.arrow.SetActive(false);
            pickedPlayerBelowIndex = 0;
            PickNewPlayerBelow();
        }
    }


    // This method for ABOVE
    private bool isCurrentPuckBelowBlock()
    {
        if (pickedPlayerAbove.transform.position.y < blockY)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // This method for ABOVE
    private bool isAllPucksBelowBlock()
    {
        for (int i = 0; i < playerAboveList.Count; i++)
        {
            if (playerAboveList[i].transform.position.y >= blockY)
            {
                return false;
            }
        }
        return true;
    }

    
    private void PickNewPlayerAbove()
    {
        bool isEntered = false;
        isFinishedAbove = false;
        bool isFound = false;
        for(int i=0; i < playerAboveList.Count; i++)
        {
                if (!isEntered)
                {

                    if (i == pickedPlayerAboveIndex)
                    {
                        playerAboveList[i].GetComponent<Player>().arrow.SetActive(false);

                        if (i == playerAboveList.Count - 1)
                        {
                            for (int z = 0; z < playerAboveList.Count; z++)
                            {
                                if (!isFound)
                                {
                                    if (playerAboveList[z].GetComponent<Player>().transform.position.y >= blockY)
                                    {
                                    //   Debug.Log("Girdi1.");
                                    pickedPlayerAboveIndex = z;
                                    pickedPlayerAbove = playerAboveList[pickedPlayerAboveIndex].GetComponent<Player>();
                                        pickedPlayerAbove.arrow.SetActive(true);
                                        isFound = true;
                                    }
                                    else
                                    {
                                        playerAboveList[i].GetComponent<Player>().arrow.SetActive(false);

                                    }
                                }

                            }


                        }
                        else
                        {
                            bool isFirst = true;
                            if (playerAboveList[i+1].GetComponent<Player>().transform.position.y >= blockY)
                            {
                            pickedPlayerAboveIndex = i + 1;
                            pickedPlayerAbove = playerAboveList[pickedPlayerAboveIndex].GetComponent<Player>();
                                pickedPlayerAbove.arrow.SetActive(true);
                                isFound = true;

                            }
                            else
                            {
                                for (int k = 0; k < playerAboveList.Count; k++)
                                {

                                    if (!isFound)
                                    {
                                        if (playerAboveList[k].GetComponent<Player>().transform.position.y >= blockY)
                                        {
                                        pickedPlayerAboveIndex = k;

                                        pickedPlayerAbove = playerAboveList[pickedPlayerAboveIndex].GetComponent<Player>();
                                            pickedPlayerAbove.arrow.SetActive(true);
                                            isFound = true;
                                        }
                                        else
                                        {
                                            playerAboveList[i].GetComponent<Player>().arrow.SetActive(false);
                                        }

                                    }

                                }
                            }

                        }
                        isEntered = true;

                    }
                    
                }
        }
        if (!isEntered && playerBelowList.Count > 0 && playerAboveList.Count > 0)
        {
            pickedPlayerAboveIndex = 0;
            PickNewPlayerAbove();
        }
        if (!isFound)
        {
            isFinishedAbove = true;
        }
        else
        {
            isFinishedAbove = false;
        }

    }



    private void UpdateBelow()
    {
        if (isAllPucksAboveBlock())
        {
            finishGame(true);
        } else
        {
                LaunchOnTouchBelow();
                if (pickedPlayerBelow.arrow.activeSelf)
                {
                    TouchControl(pickedPlayerBelow, true);
                }
        }

 

      

     

    }

    private void finishGame(bool isBelow)
    {
        blockLeft.GetComponent<Animation>().Play();
        isAnimPlayed = true;
        restartCanvas.SetActive(true);
        pickedPlayerAbove.arrow.SetActive(false);
        pickedPlayerBelow.arrow.SetActive(false);

        if (isBelow)
        {
            winText.text = "Player 1 Won!";
        } else
        {
            winText.text = "Player 2 Won!";
        }
    }
    public void RestartScene()
    {
        SceneManager.LoadScene("MultiGame");
    }
    public void LoadStartMenuScene()
    {
        SceneManager.LoadScene("StartMenu");
    }
    private void CheckForeignPucksForBelow()
    {
        for (int i = 0; i < playerAboveList.Count; i++)
        {
            if (playerAboveList[i].transform.position.y < blockY)
            {
                GameObject playerAboveObj = playerAboveList[i];
                playerAboveList.RemoveAt(i);
                playerAboveObj.GetComponent<Player>().RotatePlayer(true, 180);
                playerAboveObj.transform.Find("Arrow").GetComponent<SpriteRenderer>().sprite = belowArrow;

                playerBelowList.Add(playerAboveObj);
            }
        }
        if (pickedPlayerAbove.transform.position.y < blockY)
        {
            pickedPlayerAbove.arrow.SetActive(false);
            pickedPlayerAboveIndex = 0;
            PickNewPlayerAbove();
        }
    }
    // This method for BELOW
    private bool isCurrentPuckAboveBlock()
    {
        if (pickedPlayerBelow.transform.position.y >= blockY)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // This method for BELOW
    private bool isAllPucksAboveBlock()
    {
        for (int i = 0; i < playerBelowList.Count; i++)
        {
            if (playerBelowList[i].transform.position.y < blockY)
            {
                return false;
            }
        }
        return true;
    }
    

    // This method for BELOW
    private void PickNewPlayerBelow()
    {
        bool isEntered = false;
        isFinishedBelow = false;
        bool isFound = false;
        for(int i=0; i<playerBelowList.Count; i++)
        {
            if (!isEntered)
            {

                if (i == pickedPlayerBelowIndex)
                {
                    playerBelowList[i].GetComponent<Player>().arrow.SetActive(false);

                    if (i == playerBelowList.Count - 1)
                    {
                        for (int z = 0; z < playerBelowList.Count; z++)
                        {
                            if (!isFound)
                            {
                                if (playerBelowList[z].GetComponent<Player>().transform.position.y < blockY)
                                {
                                    pickedPlayerBelowIndex = z;
                                    pickedPlayerBelow = playerBelowList[pickedPlayerBelowIndex].GetComponent<Player>();
                                    pickedPlayerBelow.arrow.SetActive(true);
                                    isFound = true;
                                }
                                else
                                {
                                    playerBelowList[i].GetComponent<Player>().arrow.SetActive(false);

                                }
                            }

                        }


                    }
                    else
                    {
                        bool isFirst = true;
                        if (playerBelowList[i + 1].GetComponent<Player>().transform.position.y < blockY)
                        {
                            pickedPlayerBelowIndex = i + 1;
                            pickedPlayerBelow = playerBelowList[pickedPlayerBelowIndex].GetComponent<Player>();
                            pickedPlayerBelow.arrow.SetActive(true);
                            isFound = true;

                        }
                        else
                        {
                            for (int k = 0; k < playerBelowList.Count; k++)
                            {

                                if (!isFound)
                                {
                                    if (playerBelowList[k].GetComponent<Player>().transform.position.y < blockY)
                                    {
                                        pickedPlayerBelowIndex = k;
                                        pickedPlayerBelow = playerBelowList[pickedPlayerBelowIndex].GetComponent<Player>();
                                        pickedPlayerBelow.arrow.SetActive(true);
                                        isFound = true;
                                    }
                                    else
                                    {
                                        playerBelowList[i].GetComponent<Player>().arrow.SetActive(false);
                                    }

                                }

                            }
                        }

                    }
                    isEntered = true;

                }
               
            }

        }

         if (!isEntered && playerBelowList.Count>0 && playerAboveList.Count > 0)
         {
            pickedPlayerBelowIndex = 0;
            PickNewPlayerBelow();
         }
        if (!isFound)
        {
            isFinishedBelow = true;
        }
        else
        {
            isFinishedBelow = false;
        }

    }



    private void LaunchOnTouchAbove()
    {
        Rect bounds;
        bounds = new Rect(0, Screen.height / 2, Screen.width, Screen.height / 2);

        // Touch
        if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled) && bounds.Contains(Input.mousePosition))
        {

            if (!hasStartedAbove)
            {
                if (!isMovedAbove)
                {
                    float fRotation = pickedPlayerAbove.rb.rotation * Mathf.Deg2Rad;
                    float fX = Mathf.Sin(fRotation);
                    float fY = Mathf.Cos(fRotation);
                    Vector2 v2 = new Vector2(fY * 10, fX * 10);
                    pickedPlayerAbove.rb.velocity = v2;

                    pickedPlayerAbove.arrow.SetActive(false);

                    hasStartedAbove = true;
                    isMovedAbove = true;

                    PickNewPlayerAbove();
                }
                else
                {
                    isMovedAbove = false;
                }

            }
            else
            {
                if (pickedPlayerAbove.arrow.activeSelf)
                {
                    float fRotation = pickedPlayerAbove.rb.rotation * Mathf.Deg2Rad;
                    float fX = Mathf.Sin(fRotation);
                    float fY = Mathf.Cos(fRotation);
                    Vector2 v2 = new Vector2(fY * 10, fX * 10);

                    pickedPlayerAbove.rb.velocity = v2;

                    pickedPlayerAbove.arrow.SetActive(false);
                    PickNewPlayerAbove();

                }
                else
                {
                    pickedPlayerAbove.rb.velocity = new Vector2(0, 0);
                    pickedPlayerAbove.arrow.SetActive(true);
                }

                hasStartedAbove = false;
            }
        }

    }

    private void LaunchOnTouchBelow()
    {
        Rect bounds = new Rect(0, blockY, Screen.width, Screen.height / 2);


        // Touch
        if (Input.touchCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled) && bounds.Contains(Input.mousePosition))
        {

            if (!hasStartedBelow)
            {
                if (!isMovedBelow)
                {
                    float fRotation = pickedPlayerBelow.rb.rotation * Mathf.Deg2Rad;
                    float fX = Mathf.Sin(fRotation);
                    float fY = Mathf.Cos(fRotation);
                    Vector2 v2 = new Vector2(fY * 10, fX * 10);
                    pickedPlayerBelow.rb.velocity = v2;
                    pickedPlayerBelow.arrow.SetActive(false);

                    hasStartedBelow = true;
                    isMovedBelow = true;

                    PickNewPlayerBelow();
                }
                else
                {
                    isMovedBelow = false;
                }

            }
            else
            {
                if (pickedPlayerBelow.arrow.activeSelf)
                {
                    float fRotation = pickedPlayerBelow.rb.rotation * Mathf.Deg2Rad;
                    float fX = Mathf.Sin(fRotation);
                    float fY = Mathf.Cos(fRotation);
                    Vector2 v2 = new Vector2(fY * 10, fX * 10);

                    pickedPlayerBelow.rb.velocity = v2;

                    pickedPlayerBelow.arrow.SetActive(false);
                    PickNewPlayerBelow();

                }
                else
                {
                    pickedPlayerBelow.rb.velocity = new Vector2(0, 0);
                    pickedPlayerBelow.arrow.SetActive(true);
                }

                hasStartedBelow = false;
            }

        }
    
        

    }
    private void TouchControl(Player secilmisPlayer,bool isSolo)
    {
        Rect bounds;
        if (isSolo)
        {
            bounds = new Rect(0, blockY, Screen.width, Screen.height / 2);
        }
        else
        {
            bounds = new Rect(0, Screen.height/2, Screen.width, Screen.height / 2);
        }
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && bounds.Contains(Input.mousePosition))
        {

            var touchDeltaPosition = Input.GetTouch(0).deltaPosition;
            float zVal = 0;
            if (isSolo)
            {
                zVal = Mathf.Clamp(touchDeltaPosition.x, -1, 1) * turnSpeed * Time.deltaTime;
            }
            else
            {
                zVal = - Mathf.Clamp(touchDeltaPosition.x, -1, 1) * turnSpeed * Time.deltaTime;
            }
            secilmisPlayer.transform.Rotate(0, 0,zVal);
            if (isSolo)
            {
                isMovedBelow = true;
            } else
            {
                isMovedAbove = true;
            }

        }
    }

}
