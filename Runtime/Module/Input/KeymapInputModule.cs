using System.Collections.Generic;
using System.Linq;
using Bingyan;
using UnityEngine;

namespace Motivation
{
    public class KeyMapInputModule : InputModule
    {
        [SerializeField, Title("按键映射")] protected KeyMap keymap;

        public override void OnAdd(Motivator m)
        {
            base.OnAdd(m);
            keymap?.Init();
        }

        protected override void OnKey(KeyCode k)
            => base.OnKey(GetMappedKey(k));

        protected override void OnKeyDown(KeyCode k)
            => base.OnKeyDown(GetMappedKey(k));

        protected override void OnKeyUp(KeyCode k)
            => base.OnKeyUp(GetMappedKey(k));

        public override void InitKeys(IEnumerable<KeyCode> keys)
            => base.InitKeys(keys.Select(i => GetRawKey(i)));

        public override void OnKeyAdd(params KeyCode[] addedKeys)
            => base.OnKeyAdd(addedKeys.Select(i => GetRawKey(i)).ToArray());

        public override void OnKeyRemove(params KeyCode[] removedKeys)
            => base.OnKeyRemove(removedKeys.Select(i => GetRawKey(i)).ToArray());

        protected KeyCode GetMappedKey(KeyCode key)
            => keymap ? keymap.GetMappedKey(key) : key;

        protected KeyCode GetRawKey(KeyCode key)
            => keymap ? keymap.GetRawKey(key) : key;
    }
}