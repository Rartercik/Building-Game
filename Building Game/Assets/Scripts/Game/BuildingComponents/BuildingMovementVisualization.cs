using System.Linq;
using UnityEngine;

namespace Game.BuildingComponents
{
    public class BuildingMovementVisualization : MonoBehaviour
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private SpriteRenderer[] _renderers;
        [SerializeField] private Color _availableColor;
        [SerializeField] private Color _outOfSpaceColor;
        [SerializeField] private Color _unavailableColor;

        private Color[] _defaultColors;

        private void OnEnable()
        {
            _defaultColors = _renderers.Select(renderer => renderer.color).ToArray();
        }

        public void UpdateState(Vector2 position, Vector2 alignedPosition, bool withinSpace, bool available)
        {
            _transform.position = withinSpace ? alignedPosition : position;

            var color = available ? _availableColor : _unavailableColor;
            if (withinSpace == false) color = _outOfSpaceColor;

            foreach (var renderer in _renderers)
            {
                renderer.color = color;
            }
        }

        public void SetBuildingFinished()
        {
            for (int i = 0; i < _renderers.Length; i++)
            {
                _renderers[i].color = _defaultColors[i];
            }
        }
    }
}
