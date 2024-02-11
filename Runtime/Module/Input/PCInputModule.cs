using System.Linq;
using System.Collections.Generic;
using Bingyan;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Motivation
{
    /// <summary>
    /// 键鼠输入组件，由新输入系统构建
    /// </summary>
    [CreateAssetMenu(fileName = "PCInputModule", menuName = "Motivation/Input/PC", order = 1)]
    public class PCInputModule : KeyMapInputModule
    {
        [Header("配置")]
        [SerializeField, Title("接受键盘输入")] private bool enableKey = true;
        [SerializeField, Title("接受鼠标输入")] private bool enableMouse = true;

        private HashSet<Key> requiredKeys;
        private Keyboard keyboard;
        private Mouse mouse;

        public override void OnAdd(Motivator m)
        {
            base.OnAdd(m);
            keyboard = Keyboard.current;
            mouse = Mouse.current;
        }

        public override void Process(float time)
        {
            if (enableKey)
            {
                foreach (var i in requiredKeys)
                {
                    if (keyboard[i].wasPressedThisFrame) OnKeyDown(KeyUtils.KeyToKeyCode(i));
                    if (keyboard[i].isPressed) OnKey(KeyUtils.KeyToKeyCode(i));
                    if (keyboard[i].wasReleasedThisFrame) OnKeyUp(KeyUtils.KeyToKeyCode(i));
                }
            }

            if (enableMouse)
            {
                ProcessMouse(mouse.leftButton, 0);
                ProcessMouse(mouse.rightButton, 1);
                ProcessMouse(mouse.middleButton, 2);
            }
        }

        private void ProcessMouse(ButtonControl btn, int idx)
        {
            var pos = mouse.position.value;
            if (btn.wasPressedThisFrame) OnMouseDown(idx, pos);
            if (btn.isPressed) OnMouse(idx, pos);
            if (btn.wasReleasedThisFrame) OnMouseUp(idx, pos);
        }

        public override void InitKeys(IEnumerable<KeyCode> keys)
        {
            base.InitKeys(keys);
            requiredKeys = RequiredKeys.Select(i => KeyUtils.KeyCodeToKey(i)).ToHashSet();
        }

        public override void OnKeyAdd(params KeyCode[] addedKeys)
        {
            base.OnKeyAdd(addedKeys);
            requiredKeys = RequiredKeys.Select(i => KeyUtils.KeyCodeToKey(i)).ToHashSet();
        }

        public override void OnKeyRemove(params KeyCode[] removedKeys)
        {
            base.OnKeyRemove(removedKeys);
            requiredKeys = RequiredKeys.Select(i => KeyUtils.KeyCodeToKey(i)).ToHashSet();
        }
    }
}