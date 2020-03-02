# AzureWebApp1
TicTacToe .Net Core
Project 

.NET Core based Web API project implementing Tic-Tac-Toe heuristic.

Application is deployed to Microsoft App Service and can be found at:
https://tictactoehw1.azurewebsites.net

Framework: 
Visual Studio 2019, .NET Core 3.1. C#

Description:

The project’s REST interface executeMove implements HTTP POST verb.

The inbound payload is a JSON object (example):
	{
	    “move”: 1,
	    “azurePlayerSymbol”: “X”,
	    “humanPlayerSymbol”: “O”
	    “gameboard” : [“X","O","?","?","?","?","?","?","?"] 
	}

move: can only be in a range of 1-8
? = empty space on the board 
azurePlayerSymbol and humanPlayerSymbol must always be opposites of each other
invalid gameboard returns HTTP Status Bad Request (400).

The output payload example:
 {
	    “move”: 7,
	    “azurePlayerSymbol”: “X”,
	    “humanPlayerSymbol”: “O”
	    “winner”: “X”,
	    “winPositions”: [
		1,
		4,
		7
	],
	    “gameboard”: ["O","X","O","?","X","O","?","X","?"] 
       }

•	Swagger documentation of the REST interface provided explicitly documents executemove interface along with the structures of input and output payloads

•	Project includes client SDK of the REST interface which is utilized by MS Tests. MS Tests test the functionality of the REST interface


	

	


