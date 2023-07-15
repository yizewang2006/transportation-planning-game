using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    public TMP_Dropdown qualityDropdown;
    public Slider ambientSlider;
    public AudioSource ambientSource;
    [ReadOnly][SerializeField] int qualityLevel;

    void Start()
    {
        ambientSlider.value = Mathf.InverseLerp(0, 0.5f, ambientSource.volume);
        // Set the initial value of the dropdown to match the current quality settings
        qualityDropdown.value = QualitySettings.GetQualityLevel();

        // Add listener to the dropdown's OnValueChanged event
        qualityDropdown.onValueChanged.AddListener(OnQualityDropdownChanged);
    }

    void Update()
    {
        ambientSource.volume = Mathf.Clamp(ambientSource.volume, 0, 0.5f);
        ambientSource.volume = Mathf.InverseLerp(0, 1, ambientSlider.value/2);
        qualityLevel = QualitySettings.GetQualityLevel();
    }

    void OnQualityDropdownChanged(int value)
    {
        // Change the game quality settings based on the dropdown value
        QualitySettings.SetQualityLevel(value);
    }
}
