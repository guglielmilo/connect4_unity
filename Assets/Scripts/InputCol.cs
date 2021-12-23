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

    private int spawnNumber = 0;

    void OnMouseDown()
    {
        Debug.Log("InputCol:" + col + " canSpawn:" + canSpawn);
        if (gameManager.IsColValid(col))
        {
            if (canAddSpawn)
            {
                StartCoroutine(AddSpawn());
                gameManager.SelectCol(col);
            }
        }
        else
        {
            Debug.LogWarning("InputCol:" + col + " invalid");
        }
    }
 
    void Update()
    {
       if (canSpawn && spawnNumber > 0)
       {
           StartCoroutine(SpawnCoin());
       }
    }

    public IEnumerator SpawnCoin()
    {
        canSpawn = false;
        spawnNumber--;
        Instantiate(GetCoinPlayer(), spawnCoin.transform.position, Quaternion.Euler(-90, 0, 0));
        yield return new WaitForSeconds((float)0.5);
        canSpawn  = true;
    }

    public IEnumerator AddSpawn()
    {
        canAddSpawn = false;
        spawnNumber++;
        yield return new WaitForSeconds((float)0.33);
        canAddSpawn  = true;

    }
    
    private GameObject GetCoinPlayer()
    {
        return gameManager.PlayerTurn() == '1' ? coinPlayer1 : coinPlayer2;
    }

}
