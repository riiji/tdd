using System;
using BowlingGame.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace BowlingGame
{
    public class Game
    {
        private readonly Frame[] frames = new Frame[10];
        private int currentFrameNumber = 0;

        public Game()
        {
            for (int i = 0; i < frames.Length; ++i)
                frames[i] = new Frame();
        }

        public void Roll(int pins)
        {
            if (pins < 0 || pins > 10)
                throw new ArgumentException();

            Frame currentFrame = frames[currentFrameNumber];

            bool isFrameEnded = false;

            if (currentFrame.IsFirstThrow)
            {
                currentFrame.FirstThrow = pins;

                if (currentFrameNumber > 0 && (frames[currentFrameNumber - 1].State == FrameState.Spare ||
                                               frames[currentFrameNumber - 1].State == FrameState.Strike))
                {
                    if (frames[currentFrameNumber - 1].State == FrameState.Strike)
                        if (currentFrameNumber > 1 && frames[currentFrameNumber - 2].State == FrameState.Strike)
                        {
                            frames[currentFrameNumber - 2].Bonus += pins;
                        }
                    frames[currentFrameNumber - 1].Bonus += pins;
                }

                currentFrame.IsFirstThrow = false;
            }
            else
            {
                if (pins + currentFrame.FirstThrow > 10)
                    throw new ArgumentException();

                currentFrame.SecondThrow = pins;

                if (currentFrameNumber > 0 && frames[currentFrameNumber - 1].State == FrameState.Strike)
                {
                    frames[currentFrameNumber - 1].Bonus += pins;
                }

                isFrameEnded = true;
            }

            if (currentFrame.FirstThrow == 10)
            {
                currentFrame.State = FrameState.Strike;
                isFrameEnded = true;
            }
            else if (currentFrame.FirstThrow + currentFrame.SecondThrow == 10)
                currentFrame.State = FrameState.Spare;

            if (isFrameEnded)
                currentFrameNumber++;
        }

        public int GetScore()
        {
            int score = 0;

            foreach (var frame in frames)
            {
                score += frame.FirstThrow;
                score += frame.SecondThrow;
                score += frame.Bonus;
            }

            return score;
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

        [Test]
        public void GetScore_AddsBonusScore_WhenStrike()
        {
            game.Roll(10);
            game.Roll(8);
            game.GetScore().Should().Be(26);
        }

        [Test]
        public void GetScore_AddsBonusScoreTwice_WhenStrike()
        {
            game.Roll(10);
            game.Roll(2);
            game.Roll(4);
            game.GetScore().Should().Be(22);
        }

        [Test]
        public void GetScore_AddsBonusScore_WhenTwoStrikes()
        {
            game.Roll(10);
            game.Roll(10);
            game.Roll(4);
            game.GetScore().Should().Be(42);
        }

        [Test]
        public void Roll_DoesntAcceptRollsMoreThanPintsLeft()
        {
            Action action = () =>
            {
                game.Roll(5);
                game.Roll(8);
            };
           action.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void GetScore_AddsBonusScore_WhenThreeStrikes()
        {
            game.Roll(10);
            game.Roll(10);
            game.Roll(10);
            game.Roll(2);
            game.GetScore().Should().Be(66);
        }

        [Test]
        public void GetScore_AddsBonusScore_WhenSpareThenStrike()
        {
            game.Roll(2);
            game.Roll(8);
            game.Roll(10);
            game.Roll(2);
            game.GetScore().Should().Be(34);
        }

        [Test]
        public void Roll_ThrowsGameEndedException_WhenMaxRollsReached()
        {
            Action action = () =>
            {
                for (int i = 0; i < 13; i++)
                {
                    game.Roll(10);
                }
            };
            action.ShouldThrow<IndexOutOfRangeException>();
        }

        [Test]
        public void GetScore_ReturnsCorrectScore_When12Strikes()
        {
            for (int i = 0; i < 12; i++)
            {
                game.Roll(10);
            }
            game.GetScore().Should().Be(300);
        }
    }
}
