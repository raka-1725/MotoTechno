using UnityEngine;

public class PlayerCredits : MonoBehaviour
{
    public static PlayerCredits Instance { get; private set; }
    public int credit;
    [SerializeField] private int maxCredits = 999999;


    private void Awake() 
    {
        Instance = this;    
    }    
    public void addMoney(int add) 
    {
        credit += add;
        credit = Mathf.Clamp(credit, 0, maxCredits);
    }

    public void subtractMoney(int subtract)
    {
        credit = Mathf.Clamp(credit, 0, maxCredits);
        credit -= subtract;
    }
}

