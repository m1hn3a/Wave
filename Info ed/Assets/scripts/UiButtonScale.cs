using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 normalScale = Vector3.one;
    public Vector3 hoverScale = new Vector3(1.15f, 1.15f, 1f);
    public float speed = 10f;

    private bool hovering = false;

    void Update()
    {
        Vector3 target = hovering ? hoverScale : normalScale;
        transform.localScale = Vector3.Lerp(transform.localScale, target, Time.deltaTime * speed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
    }
}
