using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets
{
    [RequireComponent(typeof(UIDocument))]
    public class MenubarBehaviour : MonoBehaviour
    {
        private UIDocument _uiDoc;
        private VisualElement _nativeMenubar;
        private List<Button> _menuButtons;
        private const string menubarBehaviour = "menubar-behaviour";

        public event EventHandler<bool> IsClickedChangedEvent;

        private bool _isClicked = false;
        public bool IsClicked
        {
            get => _isClicked;
            set
            {
                if(_isClicked != value)
                    IsClickedChangedEvent?.Invoke(this, value);
                _isClicked = value;
            }
        }

        private void Awake()
        {
            // Get ui document
            _uiDoc = GetComponent<UIDocument>();

            // Get menubar and register callback for mouse leave event
            _nativeMenubar = _uiDoc.rootVisualElement.Query<VisualElement>("Menubar");
            _nativeMenubar.RegisterCallback<MouseLeaveEvent>((eventArgs) => OnMouseLeftMenubar());

            // Get buttons and register callback for click event
            _menuButtons = _uiDoc.rootVisualElement.Query<Button>().ToList();
            _menuButtons.ForEach((button) => button.clicked += () => IsClicked = !IsClicked);

            // Register onvlaue changed event for IsClicked
            IsClickedChangedEvent += (sender, eventArgs) => ChangeMenubarBehaviour(eventArgs);
        }

        private void OnMouseLeftMenubar()
        {
            IsClicked = false;
            foreach (var submenuButton in _menuButtons)
                submenuButton.RemoveFromClassList(menubarBehaviour);
        }

        private void ChangeMenubarBehaviour(bool isMenuActivated)
        {
            if (isMenuActivated)
                _menuButtons.ForEach(button => { button.AddToClassList(menubarBehaviour); });
            else
                _menuButtons.ForEach(button => { button.RemoveFromClassList(menubarBehaviour); });
        }
    }
}
