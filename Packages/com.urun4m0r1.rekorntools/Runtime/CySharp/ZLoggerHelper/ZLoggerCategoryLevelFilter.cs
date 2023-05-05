#nullable enable

using System;
using Microsoft.Extensions.Logging;
using UnityEngine;

namespace Urun4m0r1.RekornTools.ZLoggerHelper
{
    [CreateAssetMenu(
        fileName = nameof(ZLoggerCategoryLevelFilter)
      , menuName = Define.AssetMenuPath + nameof(ZLoggerCategoryLevelFilter)
      , order = Define.AssetMenuOrder
    )]
    public class ZLoggerCategoryLevelFilter : ScriptableObject
    {
        public virtual Func<string, LogLevel, bool> GetCategoryLevelFilter()
        {
            return (categoryName, logLevel) => true;
        }
    }
}
