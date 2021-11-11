using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public bool available = false;
    public bool frozen = false;
    public bool currentState = false; // X - true, O - false
    [SerializeField] private GameController game;
    [SerializeField] private TMPro.TextMeshPro sign;

    public void Reset()
    {
        available = true;
        currentState = false;
        sign.text = "";
    }

    private void OnMouseDown()
    {
        if (available && !frozen)
        {
            game.Clicked(this);
        }
    }

    public void Change(bool newSign)
    {
        currentState = newSign;
        if (newSign)
        {
            sign.text = "x";
            sign.color = Color.black;
        }
        else
        {
            sign.text = "o";
            sign.color = Color.red;
        }
        available = false;
    }
}
