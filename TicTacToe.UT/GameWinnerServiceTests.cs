using Microsoft.VisualStudio.TestTools.UnitTesting;
using TicTacToe.Services;

namespace TicTacToe.UT
{
    // 为一个“闯三关（tic-tac-toe)”游戏，创建一个服务来判断是否有一方玩家获胜了
    // 所谓获胜是指玩家将记号放在3个相邻的方块内，形成水平、垂直或者对角行
    [TestClass]
    public class GameWinnerServiceTests
    {
        private const char PlayerSymbol = 'X';
        private IGameWinnerService _gameWinnerService;

        [TestInitialize]
        public void SetupUnitTests()
        {
            _gameWinnerService = new GameWinnerService();
        }

        // Test 001 如果任何一个玩家都未能在一行中放入3个标记，就没有获胜。
        //          方法返回一个空字符
        [TestMethod]
        public void NeitherPlayerHasThreeInARow()
        {
            const char expected = ' ';
            var gameBoard = new[,]
                            {
                                {' ', ' ', ' '},
                                {' ', ' ', ' '},
                                {' ', ' ', ' '}
                            };

            var actual = _gameWinnerService.Validate(gameBoard);

            Assert.AreEqual(expected, actual);
        }

        // Test 002 如果一位玩家的符号出现在最上方水平行的所有3个单元格中，则返回这位玩家的符号
        //          将其作为获胜的符号
        [TestMethod]
        public void PlayerWithAllSpacesInTopRowIsWinner()
        {
            const char expected = PlayerSymbol;
            var gameBoard = new[,]
                            {
                                {expected, expected, expected},
                                {' ', ' ', ' '},
                                {' ', ' ', ' '}
                            };

            var actual = _gameWinnerService.Validate(gameBoard);

            Assert.AreEqual(expected, actual);
        }

        // Test 003 如果一位玩家的符号占据了第一列的所有三行， 则返回这位玩家的符号，表明他已获胜
        [TestMethod]
        public void PlayerWithAllSpacesInFirstColumnIsWinner()
        {
            const char expected = PlayerSymbol;
            var gameBoard = new[,]
                            {
                                {expected, ' ', ' '},
                                {expected, ' ', ' '},
                                {expected, ' ', ' '}
                            };

            var actual = _gameWinnerService.Validate(gameBoard);

            Assert.AreEqual(expected, actual);
        }

        // Test 004 如果一位玩家占据了对角行上的3个单元格，他就是胜者
        [TestMethod]
        public void PlayerWithThreeInARowDiagonallyDownAndToRightIsWinner()
        {
            const char expected = PlayerSymbol;
            var gameBoard = new[,]
                            {
                                {expected, ' ', ' '},
                                {' ', expected, ' '},
                                {' ', ' ', expected}
                            };

            var actual = _gameWinnerService.Validate(gameBoard);

            Assert.AreEqual(expected, actual);
            
        }

        // Test 005 如果一位玩家占据了斜对角线上的3个单元格，他就是胜者
        [TestMethod]
        public void PlayerWithThreeInARowDiagonallyDownAndToLeftIsWinner()
        {
            const char expected = PlayerSymbol;
            var gameBoard = new[,]
                            {
                                {' ', ' ', expected},
                                {' ', expected, ' '},
                                {expected , ' ', ' '}
                            };

            var actual = _gameWinnerService.Validate(gameBoard);

            Assert.AreEqual(expected, actual);
            
        }
    }
}