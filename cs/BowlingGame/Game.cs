using System;
using BowlingGame.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
    public class Game
    {
        private int _score = 0;

        private int defaultPintsNumber = 10;
        private int pintsLeft;
        private bool firstStrike = true;

        public void Roll(int pins)
        {
            if (pins < 0 || pins > 10)
                throw new ArgumentException();

            if 

            _score += pins;
        }

        public int GetScore()
        {
            return _score;
        }
    }

    [TestFixture]
    public class Game_should : ReportingTest<Game_should>
    {

        private Game game;

        [SetUp]
        public void SetUp()
        {
            game = new Game();
        }

        [Test]
        public void HaveZeroScore_BeforeAnyRolls()
        {
            game
                .GetScore()
                .Should().Be(0);
        }

        [Test]
        public void HaveCorrectScore_AfterAnyRoll()
        {
            game.Roll(4);
            game.GetScore().Should().Be(4);
        }

        [Test]
        public void GetScore_ReturnsCorrectScore_AfterTwoRolls_WitnNoStrikes()
        {
            game.Roll(3);
            game.Roll(4);
            game.GetScore().Should().Be(7);
        }

        [Test]
        public void Roll_DoesntAcceptNegativeNumbers()
        {
            Action action = () => game.Roll(-1);
            action.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void Roll_DoesntAcceptNumbersMoreThatTen()
        {
            Action action = () => game.Roll(12);
            action.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void GetScore_AddsBonusScore_WhenSpare()
        {
            game.Roll(2);
            game.Roll(8);
            game.Roll(3);
            game.GetScore().Should().Be(16);
        }
    }
}
