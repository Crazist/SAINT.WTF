using System.Collections.Generic;
using Enums;
using Logic.Configs;
using UnityEngine;

namespace Logic.Entities
{
    public class Warehouse : MonoBehaviour
    {
        [SerializeField] private Transform _spawnStartTransform;

        private List<Resource> _storedItems = new List<Resource>();
        private WarehouseConfig _warehouseConfig;
        private BuildingConfig _buildingConfig;
        private int _capacity;

        public void Initialize(WarehouseConfig warehouseConfig, BuildingConfig buildingConfig, int capacity)
        {
            _buildingConfig = buildingConfig;
            _warehouseConfig = warehouseConfig;
            _capacity = capacity;
        }

        public void AddItem(Resource item)
        {
            if (_storedItems.Count < _capacity)
            {
                _storedItems.Add(item);
                ReorganizeStoredItems();
            }
        }

        public Resource RemoveItem()
        {
            if (_storedItems.Count > 0)
            {
                Resource item = _storedItems[0];
                _storedItems.RemoveAt(0);
                ReorganizeStoredItems();
                return item;
            }

            return null;
        }

        public Resource RemoveItemByType(ResourceType resourceType)
        {
            for (int i = 0; i < _storedItems.Count; i++)
            {
                if (_storedItems[i].ResourceType == resourceType)
                {
                    var item = _storedItems[i];
                    _storedItems.RemoveAt(i);
                    ReorganizeStoredItems();
                    return item;
                }
            }

            return null;
        }

        public bool CanAcceptResource(ResourceType resourceType)
        {
            if (_buildingConfig == null)
            {
                Debug.LogError("BuildingConfig не установлен для склада.");
                return false;
            }

            foreach (var requiredResource in _buildingConfig.RequiredResources)
            {
                if (requiredResource == resourceType)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasAvailableItem() => _storedItems.Count > 0;

        public WarehouseConfig GetWarehouseConfig() => _warehouseConfig;

        public bool CanStoreMoreItems() => _storedItems.Count < _capacity;

        public int GetStoredItemCount() => _storedItems.Count;

        public bool CanProvideRequiredResources(ResourceType[] requiredResources)
        {
            foreach (var resourceType in requiredResources)
            {
                if (!HasResourceType(resourceType))
                    return false;
            }

            return true;
        }

        private bool HasResourceType(ResourceType resourceType)
        {
            foreach (var item in _storedItems)
            {
                if (item.ResourceType == resourceType)
                    return true;
            }

            return false;
        }

        public void ReorganizeStoredItems()
        {
            for (int i = 0; i < _storedItems.Count; i++)
            {
                _storedItems[i].transform.position = GetNextFreePositionForIndex(i);
            }
        }

        private Vector3 GetNextFreePositionForIndex(int index)
        {
            Vector3 startPosition = _spawnStartTransform.position;
            return new Vector3(startPosition.x + (index * 0.2f), startPosition.y, startPosition.z);
        }
    }
}
