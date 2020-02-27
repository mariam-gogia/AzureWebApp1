using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;

namespace TicTacToeAttempt1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExecutemoveController : ControllerBase
    {
        // POST: api/executemove
        /// <summary>
        /// checks validity of the input payload, sends input payload and receives outputPlayload
        /// </summary>
        /// <param name="inputPayload"></param>
        /// <returns> output payload </returns>
        [HttpPost]
        [ProducesResponseType(typeof(int),StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(OutputPayload), StatusCodes.Status200OK)]
        public ActionResult<OutputPayload> Post([FromBody] InputPayload inputPayload)
        {
            // check for invalid moves
            if (inputPayload.move < 0 || inputPayload.move > 8)
            {
                return BadRequest();
            }
            // check for valid assignment of player symbols
            if(inputPayload.humanPlayerSymbol == inputPayload.azurePlayerSymbol)
            {
                return BadRequest();
            }
            // check for valid board size
            if(inputPayload.gameBoard.Length != 9)
            {
                return BadRequest();
            }

            // check for empty string positions and if only symbols on the board are "X", "O" and "?"
            string gameboardStr= ""; //converting gameboard into string
            for (int i = 0; i < 9; i++)
            {
                if (inputPayload.gameBoard[i] == "")
                {
                    return BadRequest();
                }
                else
                {
                    gameboardStr += inputPayload.gameBoard[i];
                    if (gameboardStr[i] != 'X' &&
                        gameboardStr[i] != 'O' &&
                        gameboardStr[i] != '?')
                    {
                        return BadRequest();
                    }
                }
            }
            // counting numbers of "X" and "O" on the board
            int countX = gameboardStr.Split('X').Length - 1;
            int countY = gameboardStr.Split('O').Length - 1;
            int difference = (Math.Abs(countX - countY));
            // difference in numbers of "X" and "O" should not exceed 1
            if(difference > 1)
            {
                return BadRequest();
            }

            OutputPayload outputPayload = new OutputPayload();
            // ensuring on both payloads same symbols represent same players
            outputPayload.humanPlayerSymbol = inputPayload.humanPlayerSymbol;
            outputPayload.azurePlayerSymbol = inputPayload.azurePlayerSymbol;

            Heuristic heuristic = new Heuristic(inputPayload.azurePlayerSymbol, inputPayload.humanPlayerSymbol);
            GameOutcome inputBoardGameOutcome = heuristic.DetermineWin(inputPayload.gameBoard);
           
            
            if (inputBoardGameOutcome != null)
            {
                outputPayload.gameBoard = inputPayload.gameBoard;
                outputPayload.winner = inputBoardGameOutcome.getOutcome();
                outputPayload.winPositions = inputBoardGameOutcome.getWinningPositions();
                // tie, x, or o has already won
            } else
            {
                // input payload is valid
                int azureMoveCell = heuristic.AzurePlayerMove(inputPayload.gameBoard);
                
                // reflect azure move on the game board
                string[] newGameBoard = new string[9];
                inputPayload.gameBoard.CopyTo(newGameBoard, 0);
                newGameBoard[azureMoveCell] = inputPayload.azurePlayerSymbol;

                // check if there is a winner
                GameOutcome gameOutcome = heuristic.DetermineWin(newGameBoard);
                outputPayload.gameBoard = newGameBoard;
                outputPayload.move = azureMoveCell;
                if (gameOutcome != null)
                {
                    // this is either tie, x, or o
                    string gameOutcomeStr = gameOutcome.getOutcome();
                    if (gameOutcomeStr != "tie")
                    {
                        outputPayload.winPositions = gameOutcome.getWinningPositions();
                        outputPayload.winner = gameOutcomeStr;
                    }
                    if (gameOutcome.outcome == "tie")
                    {
                        outputPayload.winner = "tie";
                    }
                }
                else
                {
                    outputPayload.winner = "inconclusive";
                }
            }
            return outputPayload;
        }

    }
}
