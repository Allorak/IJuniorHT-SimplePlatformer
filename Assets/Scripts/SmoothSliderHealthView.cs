using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SmoothSliderHealthView : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private float _animationDuration;
    [SerializeField] private Health _health;

    private void OnEnable()
    {
        _health.HealthChanged += OnHealthChanged;
    }

    private void OnDisable()
    {
        _health.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged(float newHealth, float maxHealth)
    {
        _slider.DOValue(newHealth / maxHealth, _animationDuration);
    }
}
