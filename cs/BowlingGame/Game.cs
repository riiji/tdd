using System;
using BowlingGame.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
    public class Game
    {
        private int _score = 0;

        public void Roll(int pins)
        {
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
        [Test]
        public void HaveZeroScore_BeforeAnyRolls()
        {
            new Game()
                .GetScore()
                .Should().Be(0);
        }

        [Test]
        public void HaveCorrectScore_AfterAnyRoll()
        {
            var game = new Game();
            game.Roll(4);
            game.GetScore().Should().Be(4);
        }



    }
}
