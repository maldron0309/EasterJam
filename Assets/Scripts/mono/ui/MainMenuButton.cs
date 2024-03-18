using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuButton : Selectable
{
    [SerializeField] private Image _highlightImage;
    [SerializeField] private TMP_Text _buttonText;
    [SerializeField] private Image _confirmationKeyImage;

    [SerializeField] private Color _defaultColour;
    [SerializeField] private Color _selectedColour;

    [SerializeField] private UnityEvent _onClick;

    private bool _hasBeenClicked;
    private bool _isHighlighted;
    private bool _isHoveredOver;

    private void Update()
    {
        if (_hasBeenClicked) return;

        if (_isHoveredOver && !_isHighlighted) HandleHighlight();

        if (!_isHoveredOver && _isHighlighted) HandleUnhighlight();
    }

    private void HandleHighlight()
    {
        _isHighlighted = true;
        _highlightImage.gameObject.SetActive(true);
        _highlightImage.transform.localScale = new Vector2(0f, 1f);
        _buttonText.color = _selectedColour;
        _confirmationKeyImage.enabled = true;

        LeanTween.scaleX(_highlightImage.gameObject, 1f, 0.05f).setEase(LeanTweenType.easeInOutBounce);
    }

    private void HandleUnhighlight()
    {
        _isHighlighted = false;
        _highlightImage.transform.localScale = Vector2.one;
        _buttonText.color = _defaultColour;
        _confirmationKeyImage.enabled = false;

        LeanTween
            .scaleX(_highlightImage.gameObject, 0f, 0.1f)
            .setEase(LeanTweenType.easeOutBounce)
            .setOnComplete(() => _highlightImage.gameObject.SetActive(false));
    }

    private void HandleClick()
    {
        if (!_isHighlighted) return;
        _hasBeenClicked = true;

        _buttonText.gameObject.SetActive(false);
        _confirmationKeyImage.gameObject.SetActive(false);

        LeanTween.alpha(_highlightImage.rectTransform, 0f, 0.7f);

        LeanTween
            .scaleX(_highlightImage.gameObject, 0f, 1f)
            .setEase(LeanTweenType.easeInOutBounce)
            .setOnComplete(() => _onClick.Invoke());
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        HandleClick();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        _isHoveredOver = true;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        _isHoveredOver = false;
    }
}