using System;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Displaying models is done by creating a new game object
/// to act as an anchor for the model and any transform operations the user
/// may perform.
/// </summary>
public class ModelViewer : MonoBehaviour
{
    public enum Mode
    {
        None = -1,
        Transform,
        Rotate,
        Scale
    }

    private readonly Type[] _meshTypes = new Type[]
    {
        typeof(MeshFilter),
        typeof(MeshRenderer),
    };

    private readonly Type[] _containerTypes = new Type[]
    {
        typeof(BoxCollider),
    };

    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private UIDocument _uiDocument;

    private GameObject _modelContainer;
    private GameObject _modelObject;
    private VisualElement _rootVisualElement;
    private RadioButtonGroup _modeSelectRadio;
    private Mode _selectedMode = Mode.Transform;
    private Camera _mainCamera;
    private float _cameraToObjectDistance;
    private Button _resetButton;
    private Quaternion _initialRotation;


    public void Show(GameObject showThis)
    {
        NewModelContainer();
        _modelObject = Instantiate(showThis);
        _modelObject.transform.SetParent(_modelContainer.transform, false);
    }

    public void SetMaterial(Material material)
    {
        if (material == null) return;
    }

    public void SetTexture(Texture texture)
    {
        if (texture == null) return;
    }

    private void Awake()
    {
        // later, this may be load last model, etc.
        NewModelContainer();
        _mainCamera = Camera.main;

        _cameraToObjectDistance = _mainCamera.WorldToScreenPoint(_modelContainer.transform.position).z;
    }

    private void OnEnable()
    {
        SetupUI();
    }

    private void OnDisable()
    {
        _modeSelectRadio.UnregisterValueChangedCallback(OnModeSelectValueChanged);
    }

    private void OnMouseDown()
    {
        _initialRotation = _modelContainer.transform.rotation;
    }

    private void OnMouseDrag()
    {
        var mouseWorldPosition = _mainCamera.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            _cameraToObjectDistance));

        var objectToMouse = mouseWorldPosition - _modelContainer.transform.position;

        switch (_selectedMode)
        {
            case Mode.Transform:
                _modelContainer.transform.position += objectToMouse;
                break;
            case Mode.Rotate:
                // multiplying here to make rotation feel more snappy.
                var rotationVector = 10 * objectToMouse;
                // todo; there has to be a better way.
                var newRotation = Quaternion.Euler(
                    rotationVector.y,
                    -rotationVector.x,
                    rotationVector.z);
                _modelContainer.transform.rotation = _initialRotation * newRotation;
                break;
            case Mode.Scale:
                _modelContainer.transform.localScale = Vector3.one * objectToMouse.magnitude;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Nuke whatever we have and make a new one.
    /// todo; later, there could be options for loading from a template
    ///         container instead of just starting from scorched earth.
    /// todo; later still, there could be a container factory
    ///         that takes care of all of the lifecycle management.
    /// </summary>
    private void NewModelContainer()
    {
        Destroy(_modelContainer);
        _modelContainer = new GameObject("ModelContainer", _containerTypes);
        _modelContainer.transform.SetParent(transform, false);
        var collider = _modelContainer.GetComponent<BoxCollider>();
        collider.size = Vector3.one * 4;
        collider.center = Vector3.up;
    }

    private void SetupUI()
    {
        _rootVisualElement = _uiDocument.rootVisualElement;
        _modeSelectRadio = _rootVisualElement.Q<RadioButtonGroup>("mode-select-radio");
        _modeSelectRadio.Q<Label>().text = "Mode";
        _modeSelectRadio.RegisterValueChangedCallback(OnModeSelectValueChanged);
        _modeSelectRadio.SetValueWithoutNotify((int)_selectedMode);

        _resetButton = _rootVisualElement.Q<Button>("reset-view-button");

        _resetButton.clicked += OnResetButtonClicked;
    }

    private void OnResetButtonClicked()
    {
        _modelContainer.transform.position = Vector3.zero;
        _modelContainer.transform.rotation = Quaternion.identity;
        _modelContainer.transform.localScale = Vector3.one;
    }

    private void OnModeSelectValueChanged(ChangeEvent<int> changeEvent)
    {
        _selectedMode = (Mode)changeEvent.newValue;
    }
}
