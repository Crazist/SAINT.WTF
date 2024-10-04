using Enums;
using Logic.Configs;
using Services;
using UnityEngine;
using Zenject;

namespace Logic.Entities
{
    public class Building : MonoBehaviour
    {
        private Warehouse _inputWarehouse;
        private Warehouse _outputWarehouse;
        private ProductionService _productionService;
        private BuildingConfig _buildingConfig;
        private UIService _uiService;
        private ResourceType[] _requiredResources;

        private int _productionSpeed;
        private float _productionTimer;
        private bool _isProductionStopped;

        [Inject]
        private void Inject(ProductionService productionService, UIService uiService)
        {
            _uiService = uiService;
            _productionService = productionService;
        }

        public void Initialize(BuildingConfig buildingConfig, Warehouse inputWarehouse, Warehouse outputWarehouse, int productionSpeed,
            ResourceType[] requiredResources)
        {
            _buildingConfig = buildingConfig;
            _inputWarehouse = inputWarehouse;
            _outputWarehouse = outputWarehouse;
            _productionSpeed = productionSpeed;
            _requiredResources = requiredResources;
        }

        private void Update()
        {
            if (_inputWarehouse == null)
            {
                ProduceResource();
            }
            else if (_productionService.CanProduce(_inputWarehouse, _outputWarehouse, _requiredResources))
            {
                ProduceResource();
                _uiService.ClearBuildingMessage(_buildingConfig.BuildingType.ToString()); 
                _isProductionStopped = false; 
            }
            else
            {
                if (!_isProductionStopped)
                {
                    if (!_inputWarehouse.CanProvideRequiredResources(_requiredResources))
                    {
                        _uiService.ShowBuildingStoppedProduction(_buildingConfig.BuildingType.ToString(), "Недостаточно ресурсов в инпут складе");
                    }
                    else if (!_outputWarehouse.CanStoreMoreItems())
                    {
                        _uiService.ShowBuildingStoppedProduction(_buildingConfig.BuildingType.ToString(), "Нет места в аутпут складе");
                    }

                    _isProductionStopped = true;
                }
            }
        }

        private void ProduceResource()
        {
            _productionTimer += Time.deltaTime;

            if (_productionTimer >= _productionSpeed)
            {
                _productionService.ProduceResource(_inputWarehouse, _outputWarehouse, _requiredResources);
                _productionTimer = 0f;
            }
        }
    }
}