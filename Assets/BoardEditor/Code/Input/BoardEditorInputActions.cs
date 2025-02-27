//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/BoardEditor/Code/Input/BoardEditorInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @BoardEditorInputActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @BoardEditorInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""BoardEditorInputActions"",
    ""maps"": [
        {
            ""name"": ""Editor"",
            ""id"": ""2308c149-f66b-4db7-89c9-79b3a0466357"",
            ""actions"": [
                {
                    ""name"": ""Place"",
                    ""type"": ""Button"",
                    ""id"": ""ee3e2101-40d6-4deb-8d96-298ec925307c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Remove"",
                    ""type"": ""Button"",
                    ""id"": ""1e1017f4-cfa7-4f1d-bdb6-e9cb364e4e18"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""e5d0bc6d-ab7e-4086-8965-eb7f95c85016"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""f03c6a66-d079-4c28-a328-a94862d493ea"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Place"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e774803d-d00d-4e95-a723-4354c955b11d"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Remove"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""182b5ea5-059d-46f1-8b3a-b9bd9fd7b9c0"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""BoardEditor"",
            ""bindingGroup"": ""BoardEditor"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Editor
        m_Editor = asset.FindActionMap("Editor", throwIfNotFound: true);
        m_Editor_Place = m_Editor.FindAction("Place", throwIfNotFound: true);
        m_Editor_Remove = m_Editor.FindAction("Remove", throwIfNotFound: true);
        m_Editor_Zoom = m_Editor.FindAction("Zoom", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Editor
    private readonly InputActionMap m_Editor;
    private List<IEditorActions> m_EditorActionsCallbackInterfaces = new List<IEditorActions>();
    private readonly InputAction m_Editor_Place;
    private readonly InputAction m_Editor_Remove;
    private readonly InputAction m_Editor_Zoom;
    public struct EditorActions
    {
        private @BoardEditorInputActions m_Wrapper;
        public EditorActions(@BoardEditorInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Place => m_Wrapper.m_Editor_Place;
        public InputAction @Remove => m_Wrapper.m_Editor_Remove;
        public InputAction @Zoom => m_Wrapper.m_Editor_Zoom;
        public InputActionMap Get() { return m_Wrapper.m_Editor; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(EditorActions set) { return set.Get(); }
        public void AddCallbacks(IEditorActions instance)
        {
            if (instance == null || m_Wrapper.m_EditorActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_EditorActionsCallbackInterfaces.Add(instance);
            @Place.started += instance.OnPlace;
            @Place.performed += instance.OnPlace;
            @Place.canceled += instance.OnPlace;
            @Remove.started += instance.OnRemove;
            @Remove.performed += instance.OnRemove;
            @Remove.canceled += instance.OnRemove;
            @Zoom.started += instance.OnZoom;
            @Zoom.performed += instance.OnZoom;
            @Zoom.canceled += instance.OnZoom;
        }

        private void UnregisterCallbacks(IEditorActions instance)
        {
            @Place.started -= instance.OnPlace;
            @Place.performed -= instance.OnPlace;
            @Place.canceled -= instance.OnPlace;
            @Remove.started -= instance.OnRemove;
            @Remove.performed -= instance.OnRemove;
            @Remove.canceled -= instance.OnRemove;
            @Zoom.started -= instance.OnZoom;
            @Zoom.performed -= instance.OnZoom;
            @Zoom.canceled -= instance.OnZoom;
        }

        public void RemoveCallbacks(IEditorActions instance)
        {
            if (m_Wrapper.m_EditorActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IEditorActions instance)
        {
            foreach (var item in m_Wrapper.m_EditorActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_EditorActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public EditorActions @Editor => new EditorActions(this);
    private int m_BoardEditorSchemeIndex = -1;
    public InputControlScheme BoardEditorScheme
    {
        get
        {
            if (m_BoardEditorSchemeIndex == -1) m_BoardEditorSchemeIndex = asset.FindControlSchemeIndex("BoardEditor");
            return asset.controlSchemes[m_BoardEditorSchemeIndex];
        }
    }
    public interface IEditorActions
    {
        void OnPlace(InputAction.CallbackContext context);
        void OnRemove(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
    }
}
