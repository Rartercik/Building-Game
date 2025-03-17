using UnityEngine;
using Game.BuildingComponents;
using Data;

namespace Game
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private Input _input;
        [SerializeField] private BuildingSpace _buildingSpace;
        [SerializeField] private Builder _builder;
        [SerializeField] private Building[] _buildings;

        private DataSaver _dataSaver;

        private readonly string _buildingsPath = "Buildings.json";

        private void Awake()
        {
            _dataSaver = new DataSaver();
            var data = LoadData(_dataSaver, _buildingsPath);
            _input.Initialize();
            _buildingSpace.Initialize();
            InitializePrefabs();
            _builder.Initialize(_buildings, data);
        }

        private void OnDestroy()
        {
            var data = _buildingSpace.GetBuildingsInfo();
            _dataSaver.Save(data, _buildingsPath);
        }

        private BuildingInfo[] LoadData(DataSaver dataSaver, string buildingsPath)
        {
            dataSaver = new DataSaver();
            var data = new BuildingInfo[0];
            if (dataSaver.CheckFileExists(_buildingsPath))
            {
                data = dataSaver.Load<BuildingInfo[]>(buildingsPath);
            }
            return data;
        }

        private void InitializePrefabs()
        {
            for (int i = 0; i < _buildings.Length; i++)
            {
                _buildings[i].InitializePrefab(i);
            }
        }
    }
}
