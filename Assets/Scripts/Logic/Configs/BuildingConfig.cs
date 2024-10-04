using Enums;
using Logic.Entities;
using UnityEngine;

namespace Logic.Configs
{
    [CreateAssetMenu(menuName = "Configs/BuildingConfig")]
    public class BuildingConfig : ScriptableObject
    {
        public BuildingType BuildingType;
        public Building BuildingPrefab;
        public Vector3 SpawnPosition;
        public int ProductionSpeed;
        public int StorageCapacity;

        public WarehouseConfig WarehouseInputConfig;
        public WarehouseConfig WarehouseOutputConfig;

        public ResourceType[] RequiredResources;
    }
}