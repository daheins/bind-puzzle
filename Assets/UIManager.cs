using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text levelText;
    public Text victoryText;

    public static UIManager instance;

    private void Awake()
    {
        instance = this;
        victoryText.gameObject.SetActive(false);
    }
}
