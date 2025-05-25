using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Setting Player")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerResPawnPoint;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float ReSpawnPlayerDelay;
    public PlayerController PlayerController { get => playerController; }
    
    [Header("Diamond Manager")]
    [SerializeField] private bool diamondHaveRandomLook;
    private int _diamondCollected;
    
    [Header("Setting ReSpawnPlayer")]
    public bool hasCheckPointActive;
    public Vector3 checkPoinRespawnPosition;
    
    public int diamondCollected { get => _diamondCollected; }
   

   private void Awake()
    {
        if (Instance == null)
        {
           Instance = this; 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ReSpawnPlayer()
    {
        if (hasCheckPointActive)
        {
            playerResPawnPoint.position = checkPoinRespawnPosition;
        }
        StartCoroutine(RespawnPlayerCoroutine());
    }

    IEnumerator RespawnPlayerCoroutine()
    {
        yield return new WaitForSeconds(ReSpawnPlayerDelay);
        GameObject newPlayer = Instantiate(playerPrefab, playerResPawnPoint.position, Quaternion.identity);
        newPlayer.name = "Player";
        playerController = newPlayer.GetComponent<PlayerController>();
    }
    
    public void AddDiamond() => _diamondCollected++;
    public bool DiamondHaveRandomLook() => diamondHaveRandomLook;
}
