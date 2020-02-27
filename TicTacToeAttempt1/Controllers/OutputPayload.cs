using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToeAttempt1.Controllers
{
    public class OutputPayload
    {
        /// <summary>
        /// integer indicating player's move,
        /// positions 0-8
        /// </summary>
        public int move { get; set; }

        /// <summary>
        /// Assigned symbol for Azure Player
        /// </summary>
        public string azurePlayerSymbol { get; set; }

        /// <summary>
        /// Assigned symbol for Human Player
        /// </summary>
        public string humanPlayerSymbol { get; set; }
        /// <summary>
        /// displays the winner, either "X", "O", "tie" or "inconclusive"
        /// </summary>
        public string winner { get; set; }
        /// <summary>
        /// lists positions where win happened
        /// </summary>
        public int[] winPositions { get; set; }
        /// <summary>
        /// presents current gameboard
        /// </summary>
        public string[] gameBoard { get; set; }
    }
}
