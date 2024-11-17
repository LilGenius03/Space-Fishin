using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public Rigidbody rb;
    public PlayerMovement player_movement;
    public Transform planet;

    public int points;

    [SerializeField] TextMeshProUGUI score_ui;

    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
