using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitSelectionManagerUI : MonoBehaviour
{
    [SerializeField] private RectTransform selectionAreaRect;
    [SerializeField] private Canvas canvas;

    private void Start()
    {
        UnitSelectionManager.Instance.OnSelectionAreaStart += UnitSelectionManager_OnSelectionAreaStart;
        UnitSelectionManager.Instance.OnSelectionAreaEnd += UnitSelectionManager_OnSelectionAreaEnd;
    }

    private void Update()
    {
        if (selectionAreaRect.gameObject.activeSelf)
        {
            UpdateVisual();
        }
    }

    private void UnitSelectionManager_OnSelectionAreaStart(object sender, EventArgs e)
    {
        selectionAreaRect.gameObject.SetActive(true);
        UpdateVisual();
    }

    private void UnitSelectionManager_OnSelectionAreaEnd(object sender, EventArgs e)
    {
        selectionAreaRect.gameObject.SetActive(false);
    }

    private void UpdateVisual()
    {
        Rect rect = UnitSelectionManager.Instance.GetSelectionAreaRect();
        float canvasScale = canvas.transform.localScale.x;
        selectionAreaRect.anchoredPosition = new Vector2(rect.x, rect.y) / canvasScale;
        selectionAreaRect.sizeDelta = new Vector2 (rect.width, rect.height) / canvasScale;
    }
}
