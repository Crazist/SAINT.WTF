using System.Collections.Generic;
using TMPro;

namespace Services
{
    public class UIService
    {
        private TMP_Text _messageText;

        private Dictionary<string, string> _messages = new Dictionary<string, string>();

        public void Initialize(TMP_Text messageText) => 
            _messageText = messageText;

        public void ShowBuildingStoppedProduction(string buildingName, string reason)
        {
            _messages[buildingName] = $"Здание {buildingName} остановило производство: {reason}";
            UpdateMessageDisplay();
        }

        public void ClearBuildingMessage(string buildingName)
        {
            if (_messages.ContainsKey(buildingName))
            {
                _messages.Remove(buildingName);
                UpdateMessageDisplay();
            }
        }

        private void UpdateMessageDisplay() => 
            _messageText.text = string.Join("\n", _messages.Values);
    }
}