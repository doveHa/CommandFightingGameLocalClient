public interface ICharacterSkill
{
    public const string Name = "";
    public void Run();
    bool HasHit { get; set; }
    public int Damage { get; set; }
}