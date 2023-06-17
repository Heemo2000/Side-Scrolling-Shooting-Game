using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : GenericSingleton<GameManager>
{
    [SerializeField]private Player playerPrefab;
    [SerializeField]private InputStick stick;
    public UnityEvent OnMainMenuLoad;
    public UnityEvent OnLevelStart;
    public UnityEvent OnLevelComplete;
    public UnityEvent OnLastLevelComplete;
    public UnityEvent OnGameOver;
    public bool IsGameEnded { get => _isGameEnded; }
    
    private bool _isGameEnded = false;

    private Player _player;
    private int _currentLevel = 1;

    // Start is called before the first frame update
    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(this);
        OnLevelStart.AddListener(SpawnPlayer);
        
        OnLevelComplete.AddListener(EndGame);
        OnLevelComplete.AddListener(CheckCurrentLevel);

        OnGameOver.AddListener(EndGame);
        SceneManager.sceneLoaded += OnSceneLoaded;
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
        playerInput.camera = Camera.main;
        _player.GetComponent<PlayerMovement>().InputManager.Stick = stick;
        _player.GetComponent<GenericAimHandler>().LookCamera = Camera.main;
    }

    public Vector3 GetPlayerPosition()
    {
        return _player.transform.position;
    }

    public void LoadNextLevel()
    {
        _currentLevel++;
        SceneLoader.Instance?.LoadScene("Level" + _currentLevel);
    }

    public void ReloadCurrentLevel()
    {
        SceneLoader.Instance?.LoadScene("Level" + _currentLevel);
    }

    private void CheckCurrentLevel()
    {
        if(_currentLevel == 5)
        {
            OnLastLevelComplete?.Invoke();
        }
    }

    private void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        if(scene.name.StartsWith("Level"))
        {
            OnLevelStart?.Invoke();
            SoundManager.Instance?.PlayMusic(SoundType.LevelTheme);
            _isGameEnded = false;
        }
        else
        {
            SoundManager.Instance?.PlayMusic(SoundType.MainMenuTheme);
            OnMainMenuLoad?.Invoke();
        }
    }
    
    private void OnDestroy() 
    {
        OnLevelStart.RemoveAllListeners();
        OnLevelComplete.RemoveAllListeners();    
        SceneManager.sceneLoaded += OnSceneLoaded;        
    }    
}
