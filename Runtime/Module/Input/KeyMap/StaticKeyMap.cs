using UnityEngine;

namespace Motivation
{
    /// <summary>
    /// 静态的 <see cref="KeyMap"/>, 无法在运行时保存<br/>
    /// 适用于在编辑器内使用自己习惯的键位进行测试<br/>
    /// 如果需要制作如【自定义键盘绑定】的功能，需要自行继承 <see cref="DictionaryKeyMap"/> 并使用 json 等可持久化的数据形式进行存储
    /// </summary>
    [CreateAssetMenu(fileName = "KeyMap", menuName = "Motivation/KeyMap/Static")]
    public class StaticKeyMap : DictionaryKeyMap
    {
        [SerializeField, Header("按键映射")] private KeyPairs keyPairs;

        public override void Init()
        {
            ApplyKeyMap(keyPairs.GetKeyMap());
        }
    }
}