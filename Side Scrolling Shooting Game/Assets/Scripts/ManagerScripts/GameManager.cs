using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
public class GameManager : GenericSingleton<GameManager>
{
    [SerializeField]private Player playerPrefab;
    [SerializeField]private Camera mainCamera;
    public UnityEvent OnLevelStart;
    public UnityEvent OnLevelComplete;
    public UnityEvent OnGameOver;

    private bool _isGameEnded = false;

    public bool IsGameEnded { get => _isGameEnded; }

    private Player _player;
    private int _currentLevel = 1;

    // Start is called before the first frame update
    protected override void Awake() {
        base.Awake();
    }
    void Start()
    {
        OnLevelStart.AddListener(SpawnPlayer);
        OnLevelStart?.Invoke();
        OnLevelComplete.AddListener(EndGame);
        OnGameOver.AddListener(EndGame);
        SoundManager.Instance?.PlayMusic(SoundType.LevelTheme);
    }

    private void EndGame()
    {
        _isGameEnded = true;
    }
    private void SpawnPlayer()
    {
        Transform levelStartingPoint = GameObject.Find("Starting Point").transform;
        _player = Instantiate(playerPrefab,levelStartingPoint.transform.position,Quaternion.identity);      
        PlayerInput playerInput = _player.GetComponent<PlayerInput>();
        playerInput.camera = mainCamera;
    }

    public Vector3 GetPlayerPosition()
    {
        return _player.transform.position;
    }

    public void LoadNextLevel()
    {
        SceneLoader.Instance?.LoadScene("Level" + (_currentLevel + 1));
        _currentLevel++;
    }

    public void ReloadCurrentLevel()
    {
        SceneLoader.Instance?.LoadScene("Level" + _currentLevel);
    }
    private void OnDestroy() 
    {
        OnLevelStart.RemoveAllListeners();
        OnLevelComplete.RemoveAllListeners();    
    }    
}
