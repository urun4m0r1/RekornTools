using UnityEngine;

namespace Urun4m0r1.RekornTools.DesignPatterns
{
    public class Poolable : MonoBehaviour
    {
        protected ObjectPool Pool { get; private set; }

        public virtual void Create(ObjectPool pool)
        {
            Pool = pool;
            Pool.RegisterResetAction(Push);
            gameObject.SetActive(false);
        }

        public void Push()
        {
            if (Pool) Pool.Push(this);
            else gameObject.SetActive(false);
        }

        //TODO: 인터페이스화
        //TODO: 생성 이후 초기화 인터페이스
        //TODO: 파괴 이전 작업 인터페이스
    }
}
