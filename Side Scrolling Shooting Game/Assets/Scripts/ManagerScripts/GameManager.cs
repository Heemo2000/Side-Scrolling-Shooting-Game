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

    private void OnDestroy() 
    {
        OnLevelStart.RemoveAllListeners();
        OnLevelComplete.RemoveAllListeners();    
    }    
}
