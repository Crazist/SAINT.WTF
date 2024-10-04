using Enums;
using UnityEngine;

namespace Logic.Configs
{
    public class Resource : MonoBehaviour
    {
        [SerializeField] private ResourceType _resourceType;

        public ResourceType ResourceType => _resourceType;
    }
}