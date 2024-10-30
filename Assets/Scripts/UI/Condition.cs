using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float maxValue;
    public float startValue;
    public float passiveValue;
    public Image uiBar;

    private void Start()
    {
        curValue = startValue;
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }

    public float GetPercentage()
    {
        return curValue / maxValue;
    }
    
    public IEnumerator DecreaseTime(float duration)
    {
        // 아이템 지속 시간 카운트하는 코루틴
        curValue = duration;
        while (curValue >= 0)
        {
            yield return new WaitForSeconds(1.0f);
            Subtract(passiveValue);
        }
    }
}