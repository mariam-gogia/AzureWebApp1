using RestSdkLibrary;
using RestSdkLibrary.Models;
using Microsoft.Rest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FunctionTests
{
    [TestClass]
    public class FunctionalTest
    {
        [TestClass]
        public class Functional
        {
            ServiceClientCredentials _serviceClientCredentials;
            RestSdkLibraryClient _client;

            [TestInitialize]
            public void Initialize()
            {
                _serviceClientCredentials = new TokenCredentials("FakeTokenValue");

                _client = new RestSdkLibraryClient(
                    new Uri("https://tictactoehw1.azurewebsites.net/"), _serviceClientCredentials);
            }

            [TestMethod]
            public async Task DetectWinnerX()
            {
                // Arrange
                // board is arranged so that X has won the game
                InputPayload testInput = new InputPayload();
                testInput.AzurePlayerSymbol = "X";
                testInput.HumanPlayerSymbol = "O";
                testInput.Move = 8;
                testInput.GameBoard = new string[9] { "X", "X", "X", "O", "O", "?", "O", "?", "?" };

                // Act
                OutputPayload returnedPayload = await _client.PostAsync(testInput);
                // Assert
                CheckWinner(returnedPayload, "X");
            }

            [TestMethod]
            public async Task DetectWinnerO()
            {
                // Arrange
                // board is arranged so that O has won the game
                InputPayload testInput = new InputPayload();
                testInput.AzurePlayerSymbol = "X";
                testInput.HumanPlayerSymbol = "O";
                testInput.Move = 8;
                testInput.GameBoard = new string[9] { "O", "?", "X",
                                                      "?", "O", "X",
                                                      "?", "?", "O" };

                // Act
                OutputPayload returnedPayload = await _client.PostAsync(testInput);
                //Assert
                CheckWinner(returnedPayload, "O");
            }

            [TestMethod]
            public async Task DetectTie()
            {
                // Arrange
                // board is arranged for tie condition
                InputPayload testInput = new InputPayload();
                testInput.AzurePlayerSymbol = "X";
                testInput.HumanPlayerSymbol = "O";
                testInput.Move = 8;
                testInput.GameBoard = new string[9] { "X", "O", "X",
                                                      "O", "O", "X",
                                                      "O", "X", "O" };

                // Act
                OutputPayload returnedPayload = await _client.PostAsync(testInput);
                //Assert
                CheckWinner(returnedPayload, "tie"); //helper method
            }

            [TestMethod]
            public async Task DetectWinnerInconclusive()
            {
                // Arrange
                // board is arranged in a way that winner is to be determined
                InputPayload testInput = new InputPayload();
                testInput.AzurePlayerSymbol = "X";
                testInput.HumanPlayerSymbol = "O";
                testInput.Move = 8;
                testInput.GameBoard = new string[9] { "X", "?", "?",
                                                      "O", "?", "?",
                                                      "?", "?", "?" };

                // Act
                OutputPayload returnedPayload = await _client.PostAsync(testInput);
                CheckWinner(returnedPayload, "inconclusive");

            }

            // Assert:helper method for detecting winner
            public bool CheckWinner(OutputPayload returnedPayload, string winnerSymbol)
            {
                if (returnedPayload != null)
                {
                    Assert.IsTrue(returnedPayload.Winner.Contains(winnerSymbol));
                }
                return false;
            }

            [TestMethod]
            public async Task PlayerSymbol()
            {
                // Arrange
                InputPayload testInput = new InputPayload();
                testInput.AzurePlayerSymbol = "X";
                testInput.HumanPlayerSymbol = "O";
                testInput.Move = 1;
                testInput.GameBoard = new string[9] { "X", "X", "X", "O", "?", "?", "O", "?", "?" };

                // Act 
                OutputPayload returnedPayload = await _client.PostAsync(testInput);

                // Asert
                if (returnedPayload != null)
                {
                    //test if AzurePlayer and HumanPlayer have different symbols
                    Assert.AreNotEqual(testInput.AzurePlayerSymbol, returnedPayload.HumanPlayerSymbol);

                    //They should either have "X" or "O" if not 
                    //wrong symbols were assigned
                    if (returnedPayload.HumanPlayerSymbol != "O" &&
                        returnedPayload.HumanPlayerSymbol != "X")

                    {
                        Assert.Fail("Appropriate symbols were not assiged");
                    }

                    if (returnedPayload.AzurePlayerSymbol != "O" &&
                        returnedPayload.AzurePlayerSymbol != "X")
                    {
                        Assert.Fail("Appropriate symbols were not assigned");
                    }
                }
                else
                {
                    Assert.Fail("Both players have same symbols");
                }

            }

            [TestMethod]
            public async Task InvalidGameboardLength()
            {
                // Arrange
                InputPayload testInput = new InputPayload();
                testInput.AzurePlayerSymbol = "X";
                testInput.HumanPlayerSymbol = "O";
                testInput.Move = 1;
                testInput.GameBoard = new string[5] { "X", "?", "O", "?", "?" };

                // Act 
                HttpOperationResponse<OutputPayload> returnedPayload = await _client.PostWithHttpMessagesAsync(testInput);
                // Assert
                Assert.AreEqual(StatusCodes.Status400BadRequest, (int)returnedPayload.Response.StatusCode);

            }

            [TestMethod]
            public async Task InvalidGameboardState()
            {
                // Arrange - gameboard with 3 - X s and 1 O - badrequest
                InputPayload testInput = new InputPayload();
                testInput.AzurePlayerSymbol = "X";
                testInput.HumanPlayerSymbol = "O";
                testInput.Move = 1;
                testInput.GameBoard = new string[9] { "O", "?", "X",
                                                      "?", "?", "?",
                                                      "X", "X", "?"};
                // Act 
                HttpOperationResponse<OutputPayload> returnedPayload = await _client.PostWithHttpMessagesAsync(testInput);
                // Assert
                Assert.AreEqual(StatusCodes.Status400BadRequest, (int)returnedPayload.Response.StatusCode);
            }

            [TestMethod]
            public async Task InvalidGameboardState1()
            {
                // Arrange - gameboard with all Xs
                InputPayload testInput = new InputPayload();
                testInput.AzurePlayerSymbol = "X";
                testInput.HumanPlayerSymbol = "O";
                testInput.Move = 1;
                testInput.GameBoard = new string[9] { "X", "X", "X",
                                                      "X", "X", "X",
                                                      "X", "X", "X"};
                // Act 
                HttpOperationResponse<OutputPayload> returnedPayload = await _client.PostWithHttpMessagesAsync(testInput);
                // Assert
                Assert.AreEqual(StatusCodes.Status400BadRequest, (int)returnedPayload.Response.StatusCode);
            }

            [TestMethod]
            public async Task InvalidGameboardCharacter()
            {
                // Arrange - gameboard with '6'
                InputPayload testInput = new InputPayload();
                testInput.AzurePlayerSymbol = "X";
                testInput.HumanPlayerSymbol = "O";
                testInput.Move = 1;
                testInput.GameBoard = new string[9] { "O", "?", "X",
                                                      "?", "?", "?",
                                                      "?", "6", "?"};
                // Act 
                HttpOperationResponse<OutputPayload> returnedPayload = await _client.PostWithHttpMessagesAsync(testInput);
                // Assert
                Assert.AreEqual(StatusCodes.Status400BadRequest, (int)returnedPayload.Response.StatusCode);
            }

            [TestMethod]
            public async Task InvalidMove()
            {
                // Arrange - move 10
                InputPayload testInput = new InputPayload();
                testInput.AzurePlayerSymbol = "X";
                testInput.HumanPlayerSymbol = "O";
                testInput.Move = 10;
                testInput.GameBoard = new string[9] { "O", "?", "X",
                                                      "?", "?", "?",
                                                      "?", "X", "?"};
                // Act 
                HttpOperationResponse<OutputPayload> returnedPayload = await _client.PostWithHttpMessagesAsync(testInput);
                // Assert
                Assert.AreEqual(StatusCodes.Status400BadRequest, (int)returnedPayload.Response.StatusCode);
            }

            [TestMethod]
            public async Task InvalidEmptyBoard()
            {
                // Arrange - board filled with spaces
                InputPayload testInput = new InputPayload();
                testInput.AzurePlayerSymbol = "X";
                testInput.HumanPlayerSymbol = "O";
                testInput.Move = 10;
                testInput.GameBoard = new string[9] { "", "", "",
                                                      "", "", "",
                                                      "", "", ""};
                // Act 
                HttpOperationResponse<OutputPayload> returnedPayload = await _client.PostWithHttpMessagesAsync(testInput);
                // Assert
                Assert.AreEqual(StatusCodes.Status400BadRequest, (int)returnedPayload.Response.StatusCode);
            }

            [TestMethod]
            public async Task ValidEmptyBoard()
            {
                // Arrange
                InputPayload testInput = new InputPayload();
                testInput.AzurePlayerSymbol = "X";
                testInput.HumanPlayerSymbol = "O";
                testInput.Move = 8;
                testInput.GameBoard = new string[9] { "?", "?", "?", "?", "?", "?", "?", "?", "?" };

                // Act
                OutputPayload returnedPayload = await _client.PostAsync(testInput);

                string gameboartStr = "";
                // if board is valid Azure goes first therefore X appears on the board
                for (int i = 0; i < 9; i++)
                {
                    gameboartStr += returnedPayload.GameBoard[i];
                }
                // Assert
                Assert.IsTrue(gameboartStr.Contains("X"));
            }

            [TestMethod]
            public async Task InvalidEmptySpace()
            {
                // Arrange - filled with spaces
                InputPayload testInput = new InputPayload();
                testInput.AzurePlayerSymbol = "X";
                testInput.HumanPlayerSymbol = "O";
                testInput.Move = 10;
                testInput.GameBoard = new string[9] {"X", "O", "?",
                                                      "", "", "",
                                                      "?", "?", "?"};
                // Act 
                HttpOperationResponse<OutputPayload> returnedPayload = await _client.PostWithHttpMessagesAsync(testInput);
                // Assert
                Assert.AreEqual(StatusCodes.Status400BadRequest, (int)returnedPayload.Response.StatusCode);
            }
        }
    }
}






