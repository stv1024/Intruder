using Fairwood.Math;
using UnityEngine;

namespace Assets.Scripts.Game
{
    /// <summary>
    /// 附在GameManager上
    /// </summary>
    public class EffectManager : MonoBehaviour
    {
        static EffectManager _instance;
        public static EffectManager Instance
        {
            get { return _instance ?? (_instance = GameManager.Instance.GetComponent<EffectManager>()); }
        }

        public GameObject ExplodeEffectPrefab;

        public static void CreateExplodeEffect(Vector2 pos)
        {
            var go = PrefabHelper.InstantiateAndReset(Instance.ExplodeEffectPrefab, GameManager.Container);
            go.transform.localPosition = pos;
            Destroy(go, 1);
        }
    }
}