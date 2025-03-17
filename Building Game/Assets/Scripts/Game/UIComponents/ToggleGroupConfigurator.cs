using UnityEngine;
using UnityEngine.UI;

namespace Game.UIComponents
{
    public class ToggleGroupConfigurator : MonoBehaviour
    {
        [SerializeField] private ToggleGroup _toggleGroup;

        private bool _toggled;

        private void Awake()
        {
            _toggleGroup.allowSwitchOff = true;
            _toggleGroup.SetAllTogglesOff();
        }

        public void SetToggled()
        {
            if (_toggled) return;

            _toggled = true;
            _toggleGroup.allowSwitchOff = false;
        }
    }
}
