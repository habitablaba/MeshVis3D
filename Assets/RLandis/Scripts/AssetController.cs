using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class AssetController : MonoBehaviour
{
    [SerializeField] private UIDocument _uiDocument;
    [SerializeField] private ModelViewer _modelViewerPrefab;

    private ModelViewer _modelViewerInstance;

    private List<Object> _assets;
    private List<Mesh> _models;
    private List<Texture> _textures;
    private List<Material> _materials;
    private List<GameObject> _prefabs;

    private VisualElement _rootVisualElement;
    private DropdownField _prefabDropdown;
    private DropdownField _modelDropdown;
    private DropdownField _materialDropdown;
    private DropdownField _textureDropdown;

    private void Awake()
    {
        ReloadAssets();
    }

    private void OnEnable()
    {
        SetupUI();
    }

    private void OnDisable()
    {
        _prefabDropdown.UnregisterValueChangedCallback(OnPrefabSelectionChanged);
        _modelDropdown.UnregisterValueChangedCallback(OnModelSelectionChanged);
        _materialDropdown.UnregisterValueChangedCallback(OnMaterialSelectionChanged);
        _textureDropdown.UnregisterValueChangedCallback(OnTextureSelectionChanged);
    }

    private void Start()
    {
        _modelViewerInstance = Instantiate(_modelViewerPrefab);
    }

    /// <summary>
    /// All the resources are loaded, then all the prefabs are pulled from that.
    /// Viewing a model starts with selecting a prefab
    /// </summary>
    private void ReloadAssets()
    {
        _assets = Resources.LoadAll("MeshVis").ToList();
        _prefabs = _assets.Where(o => o is GameObject).Select(o => o as GameObject).ToList();
        _models = new List<Mesh>();
        _materials = new List<Material>();
        _textures = new List<Texture>();
    }

    private void SetupUI()
    {
        _rootVisualElement = _uiDocument.rootVisualElement;

        _prefabDropdown = _rootVisualElement.Q<DropdownField>("prefab-dropdown");
        _modelDropdown = _rootVisualElement.Q<DropdownField>("model-dropdown");
        _materialDropdown = _rootVisualElement.Q<DropdownField>("material-dropdown");
        _textureDropdown = _rootVisualElement.Q<DropdownField>("texture-dropdown");

        _prefabDropdown.Q<Label>().text = "Prefab";
        _modelDropdown.Q<Label>().text = "Mesh";
        _materialDropdown.Q<Label>().text = "Material";
        _textureDropdown.Q<Label>().text = "Texture";

        _prefabDropdown.RegisterValueChangedCallback(OnPrefabSelectionChanged);
        _modelDropdown.RegisterValueChangedCallback(OnModelSelectionChanged);
        _materialDropdown.RegisterValueChangedCallback(OnMaterialSelectionChanged);
        _textureDropdown.RegisterValueChangedCallback(OnTextureSelectionChanged);

        _materials = _assets.Where(o => o is Material).Select(o => o as Material).ToList();
        _textures = _assets.Where(o => o is Texture).Select(o => o as Texture).ToList();

        _prefabDropdown.choices = _prefabs.Select(o => o.name).ToList();
        _materialDropdown.choices = _materials.Select(o => o.name).ToList();
        _textureDropdown.choices = _textures.Select(o => o.name).ToList();

    }

    private void OnPrefabSelectionChanged(ChangeEvent<string> changeEvent)
    {
        var selection = _prefabs.FirstOrDefault(p => p.name == changeEvent.newValue);
        _modelViewerInstance.Show(selection);

        _models = selection.GetComponentsInChildren<MeshFilter>().Select(mf => mf.sharedMesh).ToList();
        _modelDropdown.choices = _models.Select(o => o.name).ToList();

        _modelDropdown.value = _models.First().name;
    }

    private void OnModelSelectionChanged(ChangeEvent<string> changeEvent)
    {
        var newModel = _models.FirstOrDefault(m => m.name == changeEvent.newValue);
        _modelViewerInstance.SetMesh(newModel);

        // todo; set this index to a material that is on the selected mesh
        _materialDropdown.SetValueWithoutNotify(string.Empty);
        _textureDropdown.SetValueWithoutNotify(string.Empty);
    }

    private void OnMaterialSelectionChanged(ChangeEvent<string> changeEvent)
    {
        _modelViewerInstance.SetMaterial(_materials.FirstOrDefault(m => m.name == changeEvent.newValue));
        // todo; set this index to a texture that is on the selected mesh
        _textureDropdown.SetValueWithoutNotify(string.Empty);
    }

    private void OnTextureSelectionChanged(ChangeEvent<string> changeEvent)
    {
        _modelViewerInstance.SetTexture(_textures.FirstOrDefault(t => t.name == changeEvent.newValue));
    }
}
