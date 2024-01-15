using System.Collections.Generic;
using System.Linq;
using Bingyan;
using UnityEngine;

namespace Motivation
{
    public class KeyMapInputModule : InputModule
    {
        [SerializeField, Title("按键映射")] protected KeyMap keymap;

        protected override void OnKey(KeyCode k)
            => base.OnKey(keymap.GetMappedKey(k));

        protected override void OnKeyDown(KeyCode k)
            => base.OnKeyDown(keymap.GetMappedKey(k));

        protected override void OnKeyUp(KeyCode k)
            => base.OnKeyUp(keymap.GetMappedKey(k));

        public override void InitKeys(IEnumerable<KeyCode> keys)
            => base.InitKeys(keys.Select(i => keymap.GetRawKey(i)));

        public override void OnKeyAdd(params KeyCode[] addedKeys)
            => base.OnKeyAdd(addedKeys.Select(i => keymap.GetRawKey(i)).ToArray());

        public override void OnKeyRemove(params KeyCode[] removedKeys)
            => base.OnKeyRemove(removedKeys.Select(i => keymap.GetRawKey(i)).ToArray());
    }
}