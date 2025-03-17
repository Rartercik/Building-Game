using UnityEngine;
using UnityEngine.UI;
using Game.BuildingComponents;

namespace Game.UIComponents
{
    public class BuildingSelector : MonoBehaviour
    {
        [SerializeField] private Toggle _toggle;
        [SerializeField] private Image _selectingImage;
        [SerializeField] private ToggleGroupConfigurator _toggleGroupConfigurator;
        [SerializeField] private Builder _builder;
        [SerializeField] private Building _building;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _deselectedColor;

        private void OnEnable()
        {
            _toggle.onValueChanged.AddListener(OnToggleValueChanged);
        }

        private void OnDisable()
        {
            _toggle.onValueChanged.RemoveListener(OnToggleValueChanged);
        }

        private void OnToggleValueChanged(bool isOn)
        {
            _selectingImage.color = isOn ? _selectedColor : _deselectedColor;

            if (isOn == false) return;
            _toggleGroupConfigurator.SetToggled();
            _builder.ChooseBuilding(_building);
        }
    }
}
