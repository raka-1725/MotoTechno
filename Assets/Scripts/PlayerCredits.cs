using UnityEngine;

public class PlayerCredits : MonoBehaviour
{
    public static PlayerCredits Instance { get; private set; }
    public int credit;
    private int startcredit = 500;
    [SerializeField] private int maxCredits = 999999;


    private void Awake() 
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        credit = PlayerPrefs.GetInt("PlayerCredit", startcredit);
    }    
    public void addMoney(int add) 
    {
        credit += add;
        credit = Mathf.Clamp(credit, 0, maxCredits);
        PlayerPrefs.SetInt("PlayerCredit", credit);
    }

    public void subtractMoney(int subtract)
    {
        credit -= subtract;
        credit = Mathf.Clamp(credit, 0, maxCredits);
        PlayerPrefs.SetInt("PlayerCredit", credit);
    }
}

