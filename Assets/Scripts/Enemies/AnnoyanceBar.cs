using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class AnnoyanceBar : MonoBehaviour
{
    [SerializeField] private Image _scrollBar;
    [SerializeField] private TextMeshProUGUI _annoyanceText;

    private void Awake()
    {
        if (_scrollBar)
            _scrollBar = GetComponent<Image>();       
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
        yield return new WaitForSeconds(0.37f);
        _scrollBar.fillAmount = 0;
        _annoyanceText.text = "Annoyance: " + fullAmount;
    }
}
