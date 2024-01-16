using Bingyan;
using UnityEngine;

namespace Motivation
{
    /// <summary>
    /// 一个简单的输入组件，使用Unity的旧Input System实现
    /// </summary>
    [CreateAssetMenu(fileName = "SimpleInputModule", menuName = "Motivation/Input/Simple", order = 0)]
    public class SimpleInputModule : KeyMapInputModule
    {
        [Header("配置")]
        [SerializeField, Title("接受键盘输入")] private bool enableKey = true;
        [SerializeField, Title("接受鼠标输入")] private bool enableMouse = true;

        public override void Process(float time)
        {
            if (enableKey)
            {
                foreach (var i in RequiredKeys)
                {
                    if (Input.GetKeyDown(i)) OnKeyDown(i);
                    if (Input.GetKey(i)) OnKey(i);
                    if (Input.GetKeyUp(i)) OnKeyUp(i);
                }
            }

            if (enableMouse)
            {
                ProcessMouse(0);
                ProcessMouse(1);
                ProcessMouse(2);
            }
        }

        private void ProcessMouse(int btn)
        {
            if (Input.GetMouseButtonDown(btn)) OnMouseDown(btn, Input.mousePosition);
            else if (Input.GetMouseButton(btn)) OnMouse(btn, Input.mousePosition);
            else if (Input.GetMouseButtonUp(btn)) OnMouseUp(btn, Input.mousePosition);
        }
    }
}