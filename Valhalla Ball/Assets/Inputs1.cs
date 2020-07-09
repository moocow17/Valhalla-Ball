// GENERATED AUTOMATICALLY FROM 'Assets/Inputs1.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Inputs1 : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Inputs1()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Inputs1"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""2db1e1d5-a6ad-4db7-b5d6-4406e8dc74ed"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""931ae437-7364-4fe9-8d18-3274c4eea19e"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Aim"",
                    ""type"": ""Value"",
                    ""id"": ""17a13a3d-ef8f-409a-9d2d-a3caa67b4622"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""L Trigger"",
                    ""type"": ""Button"",
                    ""id"": ""b139ed24-c916-4a0d-9bd8-251339725458"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""L Bumper"",
                    ""type"": ""Button"",
                    ""id"": ""79d9af02-0ace-47dc-9351-49f963c4d2e2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""R Trigger"",
                    ""type"": ""Button"",
                    ""id"": ""51fcbb06-f0a8-4eb2-9a08-57152c05c8a2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""R Bumper"",
                    ""type"": ""Button"",
                    ""id"": ""23a861d4-b61a-45b9-82f7-892ff954b69a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""X (xbox) Button"",
                    ""type"": ""Button"",
                    ""id"": ""ee66d279-448b-4a06-aa75-5d3ee0aab4e6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""A (xbox) Button"",
                    ""type"": ""Button"",
                    ""id"": ""4e37daed-0193-483b-b96b-c6a13933bc8e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9063145e-1a09-4045-a5ae-e213a92e4b9c"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4a5db305-0c71-4181-a2bc-5006c68ed8f4"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Aim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ee5f077f-cdd0-44b1-8bc6-f57bc4d8994b"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""L Bumper"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""93d0925c-206c-458b-91cd-7c88e68f0b6e"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""R Trigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3c1eae73-19d2-439c-9ecf-88c68ceca99d"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""R Bumper"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7891c2f5-9b5e-4901-8dfc-32dba5e7e8be"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""L Trigger"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d9cf370b-272a-499c-94f3-93e61a1e3b7f"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""X (xbox) Button"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0b2d83ee-ced2-41d1-9d54-31a9326135fa"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""A (xbox) Button"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Aim = m_Player.FindAction("Aim", throwIfNotFound: true);
        m_Player_LTrigger = m_Player.FindAction("L Trigger", throwIfNotFound: true);
        m_Player_LBumper = m_Player.FindAction("L Bumper", throwIfNotFound: true);
        m_Player_RTrigger = m_Player.FindAction("R Trigger", throwIfNotFound: true);
        m_Player_RBumper = m_Player.FindAction("R Bumper", throwIfNotFound: true);
        m_Player_XxboxButton = m_Player.FindAction("X (xbox) Button", throwIfNotFound: true);
        m_Player_AxboxButton = m_Player.FindAction("A (xbox) Button", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Aim;
    private readonly InputAction m_Player_LTrigger;
    private readonly InputAction m_Player_LBumper;
    private readonly InputAction m_Player_RTrigger;
    private readonly InputAction m_Player_RBumper;
    private readonly InputAction m_Player_XxboxButton;
    private readonly InputAction m_Player_AxboxButton;
    public struct PlayerActions
    {
        private @Inputs1 m_Wrapper;
        public PlayerActions(@Inputs1 wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Aim => m_Wrapper.m_Player_Aim;
        public InputAction @LTrigger => m_Wrapper.m_Player_LTrigger;
        public InputAction @LBumper => m_Wrapper.m_Player_LBumper;
        public InputAction @RTrigger => m_Wrapper.m_Player_RTrigger;
        public InputAction @RBumper => m_Wrapper.m_Player_RBumper;
        public InputAction @XxboxButton => m_Wrapper.m_Player_XxboxButton;
        public InputAction @AxboxButton => m_Wrapper.m_Player_AxboxButton;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMove;
                @Aim.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @Aim.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @Aim.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAim;
                @LTrigger.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLTrigger;
                @LTrigger.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLTrigger;
                @LTrigger.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLTrigger;
                @LBumper.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLBumper;
                @LBumper.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLBumper;
                @LBumper.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnLBumper;
                @RTrigger.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRTrigger;
                @RTrigger.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRTrigger;
                @RTrigger.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRTrigger;
                @RBumper.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRBumper;
                @RBumper.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRBumper;
                @RBumper.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnRBumper;
                @XxboxButton.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnXxboxButton;
                @XxboxButton.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnXxboxButton;
                @XxboxButton.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnXxboxButton;
                @AxboxButton.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAxboxButton;
                @AxboxButton.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAxboxButton;
                @AxboxButton.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnAxboxButton;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Aim.started += instance.OnAim;
                @Aim.performed += instance.OnAim;
                @Aim.canceled += instance.OnAim;
                @LTrigger.started += instance.OnLTrigger;
                @LTrigger.performed += instance.OnLTrigger;
                @LTrigger.canceled += instance.OnLTrigger;
                @LBumper.started += instance.OnLBumper;
                @LBumper.performed += instance.OnLBumper;
                @LBumper.canceled += instance.OnLBumper;
                @RTrigger.started += instance.OnRTrigger;
                @RTrigger.performed += instance.OnRTrigger;
                @RTrigger.canceled += instance.OnRTrigger;
                @RBumper.started += instance.OnRBumper;
                @RBumper.performed += instance.OnRBumper;
                @RBumper.canceled += instance.OnRBumper;
                @XxboxButton.started += instance.OnXxboxButton;
                @XxboxButton.performed += instance.OnXxboxButton;
                @XxboxButton.canceled += instance.OnXxboxButton;
                @AxboxButton.started += instance.OnAxboxButton;
                @AxboxButton.performed += instance.OnAxboxButton;
                @AxboxButton.canceled += instance.OnAxboxButton;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnAim(InputAction.CallbackContext context);
        void OnLTrigger(InputAction.CallbackContext context);
        void OnLBumper(InputAction.CallbackContext context);
        void OnRTrigger(InputAction.CallbackContext context);
        void OnRBumper(InputAction.CallbackContext context);
        void OnXxboxButton(InputAction.CallbackContext context);
        void OnAxboxButton(InputAction.CallbackContext context);
    }
}
