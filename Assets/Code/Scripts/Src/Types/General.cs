namespace Code.Scripts.Src.Types
{
    public struct TimePeriod
    {
        public int Start { get; set; }
        public int End { get; set; }
    }

    public enum AnimatorDirection
    {
        Left,
        Right,
    }

    public enum GameOverStatus
    {
        Failed,
        Success,
        Aborted,
    }
}
