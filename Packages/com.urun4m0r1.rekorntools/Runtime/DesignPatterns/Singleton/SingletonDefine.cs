#nullable enable

using System.Threading;
using UnityEngine;

namespace Urun4m0r1.RekornTools.DesignPatterns
{
    public static class SingletonDefine
    {
        /// <summary>
        /// Singleton 인스턴스 생성/접근 시 스레드 안전성 모드
        /// <br/>성능 향상을 위해 <see cref="LazyThreadSafetyMode.PublicationOnly"/>로 설정
        /// <br/>성능 감소를 감수하고도 스레드 안전성을 보장하려면 <see cref="LazyThreadSafetyMode.ExecutionAndPublication"/>로 설정
        /// </summary>
        public static LazyThreadSafetyMode InstanceSafetyMode => LazyThreadSafetyMode.PublicationOnly;

        /// <summary>
        /// Singleton 인스턴스 생성 시 기본 public 생성자 허용 여부
        /// <br/>true로 생성 시 단일 인스턴스 생성을 보장하지 못함
        /// </summary>
        public static bool AllowDefaultPublicConstructor => false;

#if UNITY_EDITOR
        /// <summary>
        /// EditMode에서 인스턴스 생성 허용 여부
        /// <br/>해당 옵션 사용 시 <see cref="DestroySingletonInstancesOnLoad"/> 옵션도 같이 사용해야 함
        /// </summary>
        public static bool AllowEditModeInstanceCreation => true;

        /// <summary>
        /// EditMode에서 PlayMode로 전환 시 기존 Singleton 인스턴스 제거 여부
        /// <br/>Domain Reload가 비활성화 되어 있는 프로젝트에서 해당 옵션을 false로 설정 시 중복 인스턴스가 생성될 수 있음
        /// </summary>
        public static bool DestroySingletonInstancesOnLoad => true;

        /// <summary>
        /// <see cref="DestroySingletonInstancesOnLoad"/> 옵션 사용 시 Singleton 인스턴스 제거 시점
        /// <br/>대부분의 경우 <see cref="RuntimeInitializeLoadType.SubsystemRegistration"/> 옵션을 사용하면 됨
        /// </summary>
        public const RuntimeInitializeLoadType SingletonDestroyLoadType = RuntimeInitializeLoadType.SubsystemRegistration;
#endif // UNITY_EDITOR
    }
}
