using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicTacToeAttempt1.Controllers
{
    /// <summary>
    /// Game Outcome Class helps determine who won and at what winnning positions
    /// </summary>
    public class GameOutcome
    {
        public string outcome;
        public int[] winningPositions;
      
        /// <summary>
        /// A constructor 
        /// </summary>
        /// <param name="outcome"></param>
        /// <param name="winningPositions"></param>
        public GameOutcome(string outcome, int[] winningPositions)
        {
            this.outcome = outcome;
            this.winningPositions = winningPositions;
        }
        /// <summary>
        /// second constructor for tie outcome (no need for positions)
        /// </summary>
        /// <param name="outcome"></param>
        public GameOutcome(string outcome)
        {
            this.outcome = outcome;
        }
        /// <summary>
        /// method
        /// </summary>
        /// <returns> returns game outcome </returns>
        public string getOutcome()
        {
            return this.outcome;
        }
        /// <summary>
        /// method
        /// </summary>
        /// <returns> returns an array of winning positions </returns>
        public int[] getWinningPositions()
        {
            return this.winningPositions;
        }
    }
}
