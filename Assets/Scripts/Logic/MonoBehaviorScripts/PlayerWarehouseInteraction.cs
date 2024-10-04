using System.Collections;
using System.Collections.Generic;
using Logic.Configs;
using Logic.Entities;
using UnityEngine;

namespace Logic.MonoBehaviorScripts
{
    public class PlayerWarehouseInteraction : MonoBehaviour
    {
        private const string WarehouseTag = "Warehouse";
        private const int MaxCarryCapacity = 4;

        [SerializeField] private Transform _itemCarryPosition;
        [SerializeField] private float _carrySpacing = 0.1f;
        [SerializeField] private float _moveDuration = 0.3f;
        [SerializeField] private float _delayBetweenItems = 0.2f;

        private List<Resource> _carriedItems = new List<Resource>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(WarehouseTag))
            {
                Warehouse warehouse = other.GetComponent<Warehouse>();
                if (warehouse != null)
                {
                    HandleWarehouseInteraction(warehouse);
                }
            }
        }

        private void HandleWarehouseInteraction(Warehouse warehouse)
        {
            if (!warehouse.GetWarehouseConfig().IsInputWarehouse && warehouse.HasAvailableItem())
            {
                StartCoroutine(CollectAllItemsFromWarehouse(warehouse));
            }
            else if (_carriedItems.Count > 0 && warehouse.GetWarehouseConfig().IsInputWarehouse)
            {
                StartCoroutine(MoveItemsToWarehouse(warehouse));
            }
        }

        private IEnumerator CollectAllItemsFromWarehouse(Warehouse warehouse)
        {
            int itemsToCollect = Mathf.Min(MaxCarryCapacity - _carriedItems.Count, warehouse.GetStoredItemCount());

            for (int i = 0; i < itemsToCollect; i++)
            {
                StartCoroutine(MoveItemToPlayerWithDelay(warehouse, i * _delayBetweenItems));

                yield return new WaitForSeconds(_delayBetweenItems);
            }
        }

        private IEnumerator MoveItemToPlayerWithDelay(Warehouse warehouse, float delay)
        {
            yield return new WaitForSeconds(delay);

            Resource resource = warehouse.RemoveItem();

            if (resource != null)
            {
                _carriedItems.Add(resource);

                Vector3 localEndPosition = new Vector3(0, _carriedItems.Count * _carrySpacing, 0);

                yield return MoveResource(resource, _itemCarryPosition, _moveDuration);

                Transform transform1;
                (transform1 = resource.transform).SetParent(_itemCarryPosition, true);
                transform1.localPosition = localEndPosition;
                transform1.localRotation = Quaternion.identity;
            }
        }

        private IEnumerator MoveItemsToWarehouse(Warehouse warehouse)
        {
            List<Resource> remainingItems = new List<Resource>();

            while (_carriedItems.Count > 0 && warehouse.CanStoreMoreItems())
            {
                Resource resource = _carriedItems[^1];
                _carriedItems.RemoveAt(_carriedItems.Count - 1);

                if (warehouse.CanAcceptResource(resource.ResourceType))
                {
                    resource.transform.SetParent(null);

                    yield return MoveResource(resource, warehouse.transform, _moveDuration);

                    warehouse.AddItem(resource);
                    resource.transform.localRotation = Quaternion.identity;
                }
                else
                {
                    remainingItems.Add(resource);
                }

                yield return new WaitForSeconds(_delayBetweenItems);
            }

            _carriedItems.AddRange(remainingItems);
            RecalculateResources();
        }

        private void RecalculateResources()
        {
            for (int i = 0; i < _carriedItems.Count; i++)
            {
                Vector3 localEndPosition = new Vector3(0, (i + 1) * _carrySpacing, 0);
                _carriedItems[i].transform.localPosition = localEndPosition;
            }
        }


        private IEnumerator MoveResource(Resource resource, Transform targetTransform, float duration)
        {
            float elapsedTime = 0f;
            Vector3 startPosition = resource.transform.position;

            while (elapsedTime < duration)
            {
                resource.transform.position =
                    Vector3.Lerp(startPosition, targetTransform.position, elapsedTime / duration);

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            resource.transform.position = targetTransform.position;
        }
    }
}