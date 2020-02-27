using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToeAttempt1.Controllers
{
    public class Heuristic
    {
        public string azurePlayerSymbol;
        public string humanPlayerSymbol;
        /// <summary>
        /// Construcor for Heuristic class
        /// </summary>
        /// <param name="azurePlayerSymbol"></param>
        /// <param name="humanPlayerSymbol"></param>
        public Heuristic(string azurePlayerSymbol, string humanPlayerSymbol)
        {
            this.azurePlayerSymbol = azurePlayerSymbol;
            this.humanPlayerSymbol = humanPlayerSymbol;
        }

        /// <summary>
        /// This method determines the win 
        /// </summary>
        /// <param name="gameboard"></param>
        /// <returns> returns GameOutcome class instance with winner's symbol or tie </returns>
        public GameOutcome DetermineWin(string[] gameboard)
        {
            // Array to store winning positions 
            int[] winningPositions = new int[3];
            for (int i = 0; i < 8; i++)
            {
                string line = "";
                switch (i)
                {
                    // row wins
                    case 0:
                        line = gameboard[0] + gameboard[1] + gameboard[2];
                        winningPositions[0] = 0; winningPositions[1] = 1; winningPositions[2] = 2;
                        break;
                    case 1:
                        line = gameboard[3] + gameboard[4] + gameboard[5];
                        winningPositions[0] = 3; winningPositions[1] = 4; winningPositions[2] = 5;
                        break;
                    case 2:
                        line = gameboard[6] + gameboard[7] + gameboard[8];
                        winningPositions[0] = 6; winningPositions[1] = 7; winningPositions[2] = 8;
                        break;
                    // column wins 
                    case 3:
                        line = gameboard[0] + gameboard[3] + gameboard[6];
                        winningPositions[0] = 0; winningPositions[1] = 3; winningPositions[2] = 6;
                        break;
                    case 4:
                        line = gameboard[1] + gameboard[4] + gameboard[7];
                        winningPositions[0] = 1; winningPositions[1] = 4; winningPositions[2] = 7;
                        break;
                    case 5:
                        line = gameboard[2] + gameboard[5] + gameboard[8];
                        winningPositions[0] = 2; winningPositions[1] = 5; winningPositions[2] = 8;
                        break;
                    //diagonal wins
                    case 6:
                        line = gameboard[0] + gameboard[4] + gameboard[8];
                        winningPositions[0] = 0; winningPositions[1] = 4; winningPositions[2] = 8;
                        break;
                    case 7:
                        line = gameboard[2] + gameboard[4] + gameboard[6];
                        winningPositions[0] = 2; winningPositions[1] = 4; winningPositions[2] = 6;
                        break;
                }
                // check for a win, return GameOutcome with the data
                if (line.Equals("XXX"))
                {
                   return new GameOutcome("X", winningPositions);                
                }
                else if (line.Equals("OOO"))
                {
                    return new GameOutcome("O", winningPositions);
                }
            }

            // check for tie 
            for (int i = 0; i < 9; i++)
            {
                // more moves to be made - inconclusive state
                if (gameboard[i] == "?")
                {
                    break;
                }
                // all positions filled, no winner - tie
                else if (i == 8)
                {
                    return new GameOutcome("tie");  
                }
            }
            return null;
        }
        /// <summary>
        /// computes Azure player move
        /// </summary>
        /// <param name="gameboard"></param>
        /// <returns> returns Azure player's move</returns>
        public int AzurePlayerMove(string[] gameboard)
        {
            // get number of empty cells on current board
            int emptyCells = freeCells(gameboard).Count;
            
            // check if no more moves to be made (game over)
            if (emptyCells == 0)
            {
                return -1;
            }

            // if board is empty pick first move randomly
            if (emptyCells == 9)
            {
                Random random = new Random();
                return random.Next(9);
            }
            else
            {
                // determine move and place move
                int[] move = minimax(gameboard, emptyCells, azurePlayerSymbol);
                return move[0];
            }
        }
        public int[] minimax(string[] gameboard, int depth, string playerSymbol)
        {
            // best cell to place a symbol
            int[] bestCell = new int[2];
            if (playerSymbol == azurePlayerSymbol)
            {
                bestCell[0] = -10;
                bestCell[1] = int.MinValue;
            }
            else
            {
                bestCell[0] = -10;
                bestCell[1] = int.MaxValue;
            }

            string nextPlayerSymbol = playerSymbol == "X" ? "O" : "X";

            List<int> emptyCells = freeCells(gameboard);
            if (depth == 0 || emptyCells.Count == 0)
            {
                int score = evaluate(gameboard);
                int[] result = { -10, score };
                return result;
            }
            for (int i = 0; i < emptyCells.Count; i++)
            {
                // use backtracking to determine the best move
                int cell = emptyCells[i];
                string[] potentialGameboard = new string[9];
                gameboard.CopyTo(potentialGameboard, 0);
                potentialGameboard[cell] = playerSymbol;
                int[] score = minimax(potentialGameboard, depth - 1, nextPlayerSymbol);
                score[0] = cell;

                if (playerSymbol == azurePlayerSymbol)
                {
                    // maximize
                    if (score[1] > bestCell[1])
                    {
                        bestCell[0] = score[0];
                        bestCell[1] = score[1];
                    }
                }
                else
                {
                    // minimize
                    if (score[1] < bestCell[1])
                    {
                        bestCell[0] = score[0];
                        bestCell[1] = score[1];
                    }
                }
            }
            return bestCell;
        }
        /// <summary>
        /// helper method, using DetermineWin, determine which symbol won and assigns appropriate score values
        /// </summary>
        /// <param name="gameboard"></param>
        /// <returns> </returns>
        public int evaluate(string[] gameboard)
        {
            int score = 0;
            GameOutcome outcome = DetermineWin(gameboard);
            if (outcome.outcome == azurePlayerSymbol)
            {
                // azure wins, score +10
                score = 10;
            }
            else if (outcome.outcome == humanPlayerSymbol)
            {
                // human wins, score -10
                score = -10;
            }
            else if (outcome.outcome == " tie")
            {
                // otherwise tie, score stays 0
                return score;
            }
            return score;
        }

        /// <summary>
        /// helper method to count how many empty cells are on the board
        /// </summary>
        /// <param name="gameboard"></param>
        /// <returns> number of empty cells on the board </returns>
        public List<int> freeCells(string[] gameboard)
        {
            List<int> freeCells = new List<int>();
            for (int i = 0; i < 9; i++)
            {
                if (gameboard[i] == "?")
                {
                    freeCells.Add(i);
                }
            }
            return freeCells;
        }
    }
}
