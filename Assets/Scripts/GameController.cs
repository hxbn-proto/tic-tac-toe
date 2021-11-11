using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<Cell> cells;
    [SerializeField] private List<GameObject> combinationGrid;
    [SerializeField] private Text xScoreElement;
    [SerializeField] private Text oScoreElement;

    private bool playersTurn = true;
    bool nextSign = false; // X - true, O - false;
    private int xScore = 0;
    private int oScore = 0;
    private int[][] checkMatrix = new int[][] {
            new int[] {0, 1, 2},
            new int[] {3, 4, 5},
            new int[] {6, 7, 8},

            new int[] {0, 3, 6},
            new int[] {1, 4, 7},
            new int[] {2, 5, 8},
        };

    private void Start()
    {
        xScoreElement.text = xScore.ToString();
        oScoreElement.text = oScore.ToString();

        CleanGamePane();
    }

    private void CleanGamePane()
    {
        cells.ForEach(cell => cell.Reset());
        combinationGrid.ForEach(gridLine => gridLine.SetActive(false));
    }

    public void Clicked(Cell cell)
    {
        if (playersTurn)
        {
            // Player turn
            bool playerWins = Turn(cell);
            playersTurn = false;

            // Bot turn
            Cell botSelected = SelectAnotherCell(cell, cells);

            Turn(botSelected);
            playersTurn = true;

        }
    }

    private bool Turn(Cell cell)
    {
        nextSign = !nextSign;
        cell.Change(nextSign);

        int vectorWithCombination = checkCombination();

        if (vectorWithCombination >= 0)
        {
            bool playerWins = !cells[checkMatrix[vectorWithCombination][0]].currentState;

            RoundFinished(playerWins, vectorWithCombination);
            return true;
        }
        else if (cells.FindAll(cellv => cellv.available).Count == 0)
        {
            CleanGamePane();
            return true;
        }

        return false;
    }

    private int checkCombination()
    {
        for (int vectorsIndex = 0; vectorsIndex < 6; vectorsIndex++)
        {
            // Check horizontal rows
            if (CheckVector(checkMatrix[vectorsIndex]))
            {
                /*bool playerWins = !cells[checkMatrix[vectorsIndex][0]].currentState;*/

                return vectorsIndex;
            }
        }
        return -1;
    }

    private void RoundFinished(bool playerWins, int vectorIndex)
    {
        ShowCombinationAndClean(vectorIndex);

        if (playerWins)
        {
            xScore++;
        }
        else
        {
            oScore++;
        }

        xScoreElement.text = xScore.ToString();
        oScoreElement.text = oScore.ToString();

        Debug.Log("Round finished");
    }

    private void ShowCombinationAndClean(int vectorIndex)
    {
        combinationGrid[vectorIndex].SetActive(true);

        CleanGamePane();
    }

    private bool CheckVector(int[] indexes)
    {
        return !cells[indexes[0]].available && !cells[indexes[1]].available && !cells[indexes[2]].available
        && cells[indexes[0]].currentState == cells[indexes[1]].currentState && cells[indexes[1]].currentState == cells[indexes[2]].currentState;
    }

    private Cell SelectAnotherCell(Cell excluded, List<Cell> cells)
    {
        List<Cell> availableCells = cells.FindAll(cell => cell.available);
        availableCells.Remove(excluded);

        return availableCells[new System.Random().Next(availableCells.Count)];
    }
}
