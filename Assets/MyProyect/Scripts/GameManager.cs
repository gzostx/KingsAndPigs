using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private bool diamondHaveRandomLook;
    public PlayerController PlayerController { get => playerController; }
    private int _diamondCollected; 
    
    public int DiamondCollected { get => _diamondCollected; }
   

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
    public void AddDiamond() => _diamondCollected++;
    public bool DiamondHaveRandomLook() => diamondHaveRandomLook;
}
