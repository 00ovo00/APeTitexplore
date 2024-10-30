using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollisionIndicator : MonoBehaviour
{
    public Image CollisionIndicatorImage;
    public TextMeshProUGUI CollisionIndicatorText;

    private void Start()
    {
        CharacterManager.Instance.Player.controller.wallCollisionEnter += ActivateIndicator;
        CharacterManager.Instance.Player.controller.wallCollisionExit += DeActivateIndicator;
        DeActivateIndicator();
    }
    
    private void OnDisable()
    {
        CharacterManager.Instance.Player.controller.wallCollisionEnter -= ActivateIndicator;
        CharacterManager.Instance.Player.controller.wallCollisionExit -= DeActivateIndicator;
    }

    private void ActivateIndicator()
    {
        CollisionIndicatorImage.enabled = true;
        CollisionIndicatorText.enabled = true;
    }
    private void DeActivateIndicator()
    {
        CollisionIndicatorImage.enabled = false;
        CollisionIndicatorText.enabled = false;
    }
}
