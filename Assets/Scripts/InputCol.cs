using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCol : MonoBehaviour
{
    public int col;

    public GameObject spawnCoin;
    public GameObject coinPlayer1;
    public GameObject coinPlayer2;

    public GameManager gameManager;
    
    private bool canSpawn = true;
    private bool canAddSpawn = true;

    private Queue<GameObject> spawnCoinQueue = new Queue<GameObject>();

    void OnMouseDown()
    {
        if (gameManager.IsGameActive() && gameManager.IsColValid(col))
        {
            if (canAddSpawn)
            {
                GameObject coinPlayer = GetCoinPlayer();
                StartCoroutine(AddSpawn(coinPlayer));
                gameManager.SelectCol(col);
                
            }
        }
    }
 
    void Update()
    {
       if (canSpawn && spawnCoinQueue.Count > 0)
       {
           StartCoroutine(SpawnCoin());
       }
    }

    public void AddComputerSpawn(char playerTurn)
    {
        StartCoroutine(AddSpawn(playerTurn == '1' ? coinPlayer1 : coinPlayer2));
        gameManager.SelectCol(col);
    }

    public IEnumerator SpawnCoin()
    {
        canSpawn = false;
        GameObject coinPlayer = Instantiate(spawnCoinQueue.Dequeue(), spawnCoin.transform.position, Quaternion.Euler(-90, 0, 0)) as GameObject;
        coinPlayer.gameObject.tag = "coins";
        yield return new WaitForSeconds((float)0.5);
        canSpawn  = true;
    }

    public IEnumerator AddSpawn(GameObject coinPlayer)
    {
        canAddSpawn = false;
        spawnCoinQueue.Enqueue(coinPlayer);
        yield return new WaitForSeconds((float)0.33);
        canAddSpawn  = true;

    }
    
    private GameObject GetCoinPlayer()
    {
        return gameManager.PlayerTurn() == '1' ? coinPlayer1 : coinPlayer2;
    }

}
