using System;
using System.Collections.Generic;

namespace TicTacToe.Services
{
    public interface IGameWinnerService
    {
        char Validate(char[,] gameBoard);
    }

    public class GameWinnerService : IGameWinnerService
    {
        private const char SymbolForNoWinner = ' ';

        #region Implementation of IGameWinnerService

        public char Validate(char[,] gameBoard)
        {
            var checks = new List<Func<char[,], char>>
                         {
                             CheckForThreeInAColumn,
                             CheckForThreeInARow,
                             CheckForThreeInADiagonally,
                             CheckForThreeInACounterDiagonally
                         };

            var currentWinningSymbol = SymbolForNoWinner;

            foreach (var check in checks)
            {
                currentWinningSymbol = check(gameBoard);
                if (currentWinningSymbol != SymbolForNoWinner) break;
            }

            return currentWinningSymbol;
        }

        #endregion

        private static char CheckForThreeInAColumn(char[,] gameBoard)
        {
            var rowOneChar = gameBoard[0, 0];
            var rowTwoChar = gameBoard[1, 0];
            var rowThreeChar = gameBoard[2, 0];
            if (rowOneChar == rowTwoChar && rowTwoChar == rowThreeChar)
            {
                return rowOneChar;
            }
            return SymbolForNoWinner;
        }

        private static char CheckForThreeInARow(char[,] gameBoard)
        {
            var columnOneChar = gameBoard[0, 0];
            var columnTwoChar = gameBoard[0, 1];
            var columnThreeChar = gameBoard[0, 2];
            if (columnOneChar == columnTwoChar && columnTwoChar == columnThreeChar)
            {
                return columnOneChar;
            }
            return SymbolForNoWinner;
        }

        private static char CheckForThreeInADiagonally(char[,] gameBoard)
        {
            var cellOneChar = gameBoard[0, 0];
            var cellTwoChar = gameBoard[1, 1];
            var cellThreeChar = gameBoard[2, 2];

            if (cellOneChar == cellTwoChar && cellTwoChar == cellThreeChar)
                return cellOneChar;
            return SymbolForNoWinner;
        }

        private static char CheckForThreeInACounterDiagonally(char[,] gameBoard)
        {
            var cellOneChar = gameBoard[0, 2];
            var cellTwoChar = gameBoard[1, 1];
            var cellThreeChar = gameBoard[2, 0];

            if (cellOneChar == cellTwoChar && cellTwoChar == cellThreeChar)
                return cellOneChar;
            return SymbolForNoWinner;
        }
    }
}