using Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Infrastructure.Factory
{
    public class UIFactory
    {
        private const string UIRoot = "UI/UIRoot";
        private const string JoystickPath = "UI/Joystick";
        private const string MessageTextPath = "UI/MessageText"; 
        public  Canvas UI { get; private set; }
        public FixedJoystick Joystick { get; private set; } 
        
        private Transform _uiRoot;
        private AssetProvider _assetProvider;
        private UIService _uiService;

        [Inject]
        private void Inject(AssetProvider assetProvider, UIService uiService)
        {
            _uiService = uiService;
            _assetProvider = assetProvider;
        }

        public void CreateUiRoot()
        { 
            UI = _assetProvider.InstantiateAsset<Canvas>(UIRoot);
            _uiRoot = UI.gameObject.transform;
        }
        
        public void CreateJoystick() => 
            Joystick = _assetProvider.InstantiateAsset<FixedJoystick>(JoystickPath, _uiRoot);
       
        public void CreateMessageText()
        {
            var text = _assetProvider.InstantiateAsset<TMP_Text>(MessageTextPath, _uiRoot);
            text.transform.SetParent(_uiRoot, false);
            _uiService.Initialize(text);
        }
    }
}