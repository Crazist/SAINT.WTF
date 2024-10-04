using Logic.Entities;
using UnityEngine;

namespace Logic.Configs
{
    [System.Serializable]
    public class WarehouseConfig
    {
        public Warehouse WarehousePrefab;
        public Resource ResourceItem;
        public Vector3 Position;
        public bool IsInputWarehouse;
    }
}