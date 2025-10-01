using Scellecs.Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Gameplay.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct StartBattleComponent : IComponent
    {
        [SerializeField] private Button _startBattleButton;

        public void SubscribeOnButton(UnityAction action)
        {
            _startBattleButton.onClick.AddListener(action);
        }

        public void UnsubscribeFromButton(UnityAction action)
        {
            _startBattleButton.onClick.RemoveListener(action);
        }
    }
}