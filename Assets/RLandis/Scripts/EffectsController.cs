using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class EffectsController : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private Volume _effectsVolume;
    [SerializeField] private VolumeProfile _defaultProfile;
    [SerializeField] private List<VolumeProfile> _presetProfiles;

    private VisualElement _rootVisualElement;

    private void Awake()
    {
        SetupUI();
    }

    private void OnApplicationQuit()
    {
        _effectsVolume.profile = _defaultProfile;
    }

    private void SetupUI()
    {
        _rootVisualElement = _uiDocument.rootVisualElement;

        var defaultButton = _rootVisualElement.Q<Button>("default-button");
        var presetOneButton = _rootVisualElement.Q<Button>("preset-one-button");
        var presetTwoButton = _rootVisualElement.Q<Button>("preset-two-button");
        var presetThreeButton = _rootVisualElement.Q<Button>("preset-three-button");
        var presetFourButton = _rootVisualElement.Q<Button>("preset-four-button");

        defaultButton.clicked += OnDefaultPressed;
        presetOneButton.clicked += OnPresetOnePressed;
        presetTwoButton.clicked += OnPresetTwoPressed;
        presetThreeButton.clicked += OnPresetThreePressed;
        presetFourButton.clicked += OnPresetFourPressed;
    }

    private void OnDefaultPressed()
    {
        _effectsVolume.profile = _defaultProfile;
    }

    private void OnPresetOnePressed()
    {
        _effectsVolume.profile = _presetProfiles[0];
    }

    private void OnPresetTwoPressed()
    {
        _effectsVolume.profile = _presetProfiles[1];
    }

    private void OnPresetThreePressed()
    {
        _effectsVolume.profile = _presetProfiles[2];
    }

    private void OnPresetFourPressed()
    {
        _effectsVolume.profile = _presetProfiles[3];
    }
}
