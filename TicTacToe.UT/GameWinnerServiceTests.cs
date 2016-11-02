using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicTacToe.Services;

namespace TicTacToe.UT
{
    // 为一个“闯三关（tic-tac-toe)”游戏，创建一个服务来判断是否有一方玩家获胜了
    // 所谓获胜是指玩家将记号放在3个相邻的方块内，形成水平、垂直或者对角行
    [TestClass]
    public class GameWinnerServiceTests
    {
        // Test 001 如果任何一个玩家都未能在一行中放入3个标记，就没有获胜。
        //          方法返回一个空字符
        [TestMethod]
        public void NeitherPlayerHasThreeInARow()
        {
            IGameWinnerService gameWinnerService = new GameWinnerService();

            const char expected = ' ';
            var gameBoard = new char[3, 3]
                            {
                                {' ', ' ', ' '},
                                {' ', ' ', ' '},
                                {' ', ' ', ' '}
                            };

            var actual = gameWinnerService.Validate(gameBoard);

            Assert.AreEqual(expected, actual);
        }
    }
}