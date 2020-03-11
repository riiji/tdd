namespace BowlingGame
{
    public class Frame
    {
        public FrameState State { get; set; } = FrameState.None;
        public int FirstThrow { get; set; }
        public int SecondThrow { get; set; }
        public int Bonus { get; set; }
        public bool IsFirstThrow { get; set; } = true;
    }
}