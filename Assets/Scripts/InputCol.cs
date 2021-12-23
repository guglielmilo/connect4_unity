using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCol : MonoBehaviour
{
    public int col;

    public GameManager gameManager;

    void OnMouseDown()
    {
        Debug.Log("Col:" + col);
        gameManager.SelectCol(col);
    }
}
