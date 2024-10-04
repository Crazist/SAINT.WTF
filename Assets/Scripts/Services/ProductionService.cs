using System.Collections;
using Enums;
using Infrastructure.Factory;
using Logic.Entities;
using UnityEngine;
using Zenject;

namespace Services
{
    public class ProductionService
    {
        private GameFactory _gameFactory;

        [Inject]
        private void Inject(GameFactory gameFactory) =>
            _gameFactory = gameFactory;

        public bool CanProduce(Warehouse inputWarehouse, Warehouse outputWarehouse, ResourceType[] requiredResources) =>
            inputWarehouse.CanProvideRequiredResources(requiredResources) &&
            outputWarehouse.CanStoreMoreItems();

        public void ProduceResource(Warehouse inputWarehouse, Warehouse outputWarehouse,
            ResourceType[] requiredResources)
        {
            foreach (var resourceType in requiredResources)
            {
                var resourceToMove = inputWarehouse.RemoveItemByType(resourceType);
               
                if (resourceToMove == null)
                {
                    Debug.LogWarning("Недостаточно ресурсов для производства.");
                    return;
                }
                
                Object.Destroy(resourceToMove.gameObject);
            }

            if (outputWarehouse.CanStoreMoreItems())
            {
                var newResource = _gameFactory.CreateResource(outputWarehouse.GetWarehouseConfig());

                if (newResource != null)
                {
                    outputWarehouse.AddItem(newResource);
                }
            }
        }
    }
}