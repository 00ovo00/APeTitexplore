using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.HID;

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

    public event Action onTakeDamage;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
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

    public void Die()
    {
        Debug.Log("플레이어가 죽었다.");
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        health.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    }
    
    public IEnumerator ScaleChange(float duration, GameObject useButton)
    {
        Vector3 originalScale = playerController.size.localScale;
        Vector3 originalPos = playerController.mainCamera.localPosition;
        playerController.mainCamera.transform.localPosition = new Vector3(originalPos.x, originalPos.y * 1.5f, originalPos.z);
        transform.localScale = originalScale * 2;
        StartCoroutine(size.DecreaseTime(duration));

        yield return new WaitForSeconds(duration);

        transform.localScale = originalScale;
        playerController.mainCamera.transform.localPosition = new Vector3(originalPos.x, originalPos.y, originalPos.z);
        CharacterManager.Instance.Player.isBig = false;
        useButton.SetActive(true);
    }
}