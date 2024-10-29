using System;
using System.Collections;
using UnityEngine;

public interface IDamagable
{
    void TakePhysicalDamage(int damageAmount);
}

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public PlayerController playerController;
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition size { get { return uiCondition.size; } }

    public event Action OnTakeDamage;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        // 지속적으로 체력을 감소시킴
        health.Subtract(health.passiveValue * Time.deltaTime);

        if(health.curValue <= 0.0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    private void Die()
    {
        GameManager.Instance.GameOver(); 
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        health.Subtract(damageAmount);
        OnTakeDamage?.Invoke();
    }
    
    public IEnumerator ScaleChange(float duration, GameObject useButton)
    {
        // 플레이어 크기와 카메라 위치 일시적으로 변경
        Vector3 originalScale = playerController.size.localScale;
        Vector3 originalPos = playerController.mainCamera.localPosition;
        playerController.mainCamera.transform.localPosition = new Vector3(originalPos.x, originalPos.y * 1.5f, originalPos.z);
        transform.localScale = originalScale * 2;
        StartCoroutine(size.DecreaseTime(duration));    // 지속 시간 적용하는 코루틴 시작
        yield return new WaitForSeconds(duration);

        // 지속 시간 종료되고 원상태로 복구
        transform.localScale = originalScale;
        playerController.mainCamera.transform.localPosition = new Vector3(originalPos.x, originalPos.y, originalPos.z);
        CharacterManager.Instance.Player.isBig = false;
        useButton.SetActive(true);
    }
}