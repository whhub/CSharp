namespace TicTacToe.Services
{
    public interface IGameWinnerService
    {
        char Validate(char[,] gameBoard);
    }

    public class GameWinnerService : IGameWinnerService
    {
        #region Implementation of IGameWinnerService

        public char Validate(char[,] gameBoard)
        {
            return ' ';
        }

        #endregion
    }
}