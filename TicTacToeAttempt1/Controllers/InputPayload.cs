using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToeAttempt1.Controllers
{
    public class InputPayload
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
        /// An array presenting the gameboard
        /// </summary>
        public string[] gameBoard { get; set; }
    }
}
