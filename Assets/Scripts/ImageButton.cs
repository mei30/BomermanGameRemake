using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ImageButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    void Start()
    {
    }

    // Hover enter
    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    // Hover exit
    public void OnPointerExit(PointerEventData eventData)
    {
    }

    // Mouse down (pressed)
    public void OnPointerDown(PointerEventData eventData)
    {
    }

    // Mouse up (released)
    public void OnPointerUp(PointerEventData eventData)
    {
        // Optional: delay or load immediately
        GameManager.Instance.StartGame();
    }
}
