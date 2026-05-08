namespace Code.Scripts.Src.Types
{
    public interface ILevelUpUpgrade
    {
        public string NameIndicator { get; }
        public string DescriptionIndicator { get; }
        public string ImageName { get; }
    }

    public class DoubleBullet : ILevelUpUpgrade
    {
        public string NameIndicator { get; } = "LevelUpUpgrade/DoubleBullets/Name";
        public string DescriptionIndicator { get; } = "LevelUpUpgrade/DoubleBullets/Description";
        public string ImageName { get; } = "";
    }
}
