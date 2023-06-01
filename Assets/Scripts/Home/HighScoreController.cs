using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreController : MonoBehaviour
{
    [SerializeField] private Text highScore;
    private Transform panel;
    void Awake()
    {
        panel = transform.GetChild(1);
        highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString("#,0").Replace(",", ".");
    }
    private void OnEnable()
    {
        panel.DOScale(Vector3.one, 0.3f);
    }
    private void OnDisable()
    {
        panel.DOScale(Vector3.zero, 0.3f);
    }
}
