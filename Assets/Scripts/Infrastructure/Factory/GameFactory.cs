using Logic.Configs;
using Logic.Entities;
using Services;
using UnityEngine;
using Zenject;

namespace Infrastructure.Factory
{
    public class GameFactory
    {
        private const string Player = "Player";
        private const string BuildingsConfig = "ScriptableObjects/BuildingListConfig";

        private AssetProvider _assetProvider;

        [Inject]
        private void Inject(AssetProvider assetProvider) =>
            _assetProvider = assetProvider;

        public void CreatePlayer() =>
            _assetProvider.InstantiateAsset<GameObject>(Player);

        public void CreateBuildings()
        {
            var buildingsConfig = _assetProvider.LoadAsset<BuildingListConfig>(BuildingsConfig);

            foreach (var buildingConfig in buildingsConfig.Buildings)
            {
                CreateBuildingWithWarehouses(buildingConfig);
            }
        }

        public Resource CreateResource(WarehouseConfig warehouseConfig, Transform parent = null) =>
            Object.Instantiate(warehouseConfig.ResourceItem, parent);

        private void CreateBuildingWithWarehouses(BuildingConfig config)
        {
            Building buildingComponent = CreateBuilding(config.BuildingPrefab, config.SpawnPosition);

            Warehouse inputWarehouse = null;

            if (config.RequiredResources.Length > 0)
            {
                inputWarehouse = CreateWarehouse(config, config.WarehouseInputConfig,
                    config.WarehouseInputConfig.Position, config.StorageCapacity);
            }

            Warehouse outputWarehouse = CreateWarehouse(config, config.WarehouseOutputConfig,
                config.WarehouseOutputConfig.Position, config.StorageCapacity);

            buildingComponent.Initialize(config, inputWarehouse, outputWarehouse, config.ProductionSpeed,
                config.RequiredResources);
        }

        private Building CreateBuilding(Building prefab, Vector3 position)
        {
            Building buildingInstance = _assetProvider.InstantiateAsset<Building>(prefab.name, null);
            buildingInstance.transform.position = position;
            return buildingInstance;
        }

        private Warehouse CreateWarehouse(BuildingConfig buildingConfig, WarehouseConfig warehouseConfig, Vector3 position, int capacity)
        {
            Warehouse warehouseComponent = Object.Instantiate(warehouseConfig.WarehousePrefab, null);
            warehouseComponent.transform.position = position;
            warehouseComponent.Initialize(warehouseConfig, buildingConfig, capacity);
            return warehouseComponent;
        }
    }
}