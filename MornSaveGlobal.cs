using MornGlobal;
using UnityEngine;

namespace MornLib
{
    [CreateAssetMenu(fileName = nameof(MornSaveGlobal), menuName = "Morn/" + nameof(MornSaveGlobal))]
    public sealed class MornSaveGlobal : MornGlobalBase<MornSaveGlobal>
    {
        [Tooltip("PlayerPrefsに保存する際のキー名")]
        public string CorePlayerPrefsKey = "SaveData";
        [Tooltip("セーブデータのファイル名（拡張子なし）")]
        public string CoreFileName = "SaveData";
        [Tooltip("セーブデータの拡張子")]
        public string CoreExtensionName = ".sav";
        [Tooltip("セーブデータを保存するディレクトリ名")]
        public string SaveDir = "Shared";
        /// <inheritdoc/>
        protected override string ModuleName => "MornSave";
    }
}