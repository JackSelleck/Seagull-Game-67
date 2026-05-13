using Scripts.Enemies;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class AnnoyanceBar : MonoBehaviour
{
    [SerializeField] private SpawnEnemy _spawnEnemy;
    [SerializeField] private Image _scrollBar;
    [SerializeField] private TextMeshProUGUI _annoyanceText;
    [Space]
    public UnityEvent OnAnnoyanceBarFilled;

    private void Awake()
    {
        if (_scrollBar)
            _scrollBar = GetComponent<Image>();

        if (_spawnEnemy == null)
        {
            _spawnEnemy = FindFirstObjectByType<SpawnEnemy>();
        }
    }

    public void OnAnnoyanceIncrease(float increaseAmount, float fullAmount)
    {
        _scrollBar.fillAmount += increaseAmount / 10;

        if (_scrollBar.fillAmount >= 1)
        {
            StartCoroutine(WaitThenUnfillBar(fullAmount));
        }
    }

    private IEnumerator WaitThenUnfillBar(float fullAmount)
    {
        yield return new WaitForSeconds(0.471f);
        _scrollBar.fillAmount = 0;
        _annoyanceText.text = "Annoyance: " + fullAmount;
        OnBarFilled(fullAmount);
    }

    private void OnBarFilled(float fullAmount)
    {
        _spawnEnemy.OnAnnoyanceIncrease(fullAmount);
        OnAnnoyanceBarFilled.Invoke();
    }

    private void OnDestroy()
    {
        OnAnnoyanceBarFilled.RemoveAllListeners();
    }
}
