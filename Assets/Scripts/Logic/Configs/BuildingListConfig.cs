using UnityEngine;

namespace Logic.Configs
{
    [CreateAssetMenu(fileName = "BuildingListConfig", menuName = "Configs/BuildingListConfig")]
    public class BuildingListConfig : ScriptableObject
    {
        public BuildingConfig[] Buildings;
    }
}