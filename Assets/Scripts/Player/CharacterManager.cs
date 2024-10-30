public class CharacterManager : Singleton<CharacterManager>
{
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }
    private Player _player;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}