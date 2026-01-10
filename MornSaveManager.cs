using System;
using System.IO;
using UnityEngine;

namespace MornLib
{
    /// <summary> セーブデータの管理を行う抽象クラス </summary>
    public abstract class MornSaveManager<TSaveData> : MonoBehaviour where TSaveData : class
    {
        /// <summary> 現在のセーブデータ(内部用) </summary>
        [SerializeField, ReadOnly] private TSaveData _current;
        /// <summary> 現在のセーブデータ </summary>
        public TSaveData Current => _current ??= Load();

        /// <summary> セーブデータを新規作成する </summary>
        protected abstract TSaveData CreateInstance();

        /// <summary> セーブデータを引き継ぎつつ新規作成する </summary>
        protected abstract TSaveData CreateInstanceInitializingWith(TSaveData origin);

        /// <summary> セーブデータの読み込みを試みる </summary>
        protected abstract bool TryLoad(out TSaveData result);

        /// <summary> セーブデータのバインド処理を行う </summary>
        protected abstract void Bind(TSaveData data);

        /// <summary> セーブデータの保存を行う </summary>
        protected abstract void Save(TSaveData data);

        private void Awake()
        {
            MornDebugCore.RegisterGUI(
                "セーブマネージャ/データコピー",
                () =>
                {
                    if (GUILayout.Button("クリップボードにコピー"))
                    {
                        var saveDataString = JsonUtility.ToJson(_current);
                        GUIUtility.systemCopyBuffer = saveDataString;
                        MornSaveGlobal.Logger.Log("セーブデータをクリップボードに貼り付けました");
                    }
                },
                destroyCancellationToken);
            MornDebugCore.RegisterGUI(
                "セーブマネージャ/データ完全削除",
                () =>
                {
                    if (GUILayout.Button("Steam実績を含む完全初期化 & ゲーム終了"))
                    {
                        DeleteSaveData();
#if USE_STEAM
                        var success = SteamUserStats.ResetAllStats(true);
                        if (MornSteamManager.Initialized && success)
                        {
                            MornSaveLogger.Log("Steam実績をリセット");
                            SteamUserStats.StoreStats();
                        }
                        else
                        {
                            MornSaveLogger.Log("Steam実績のリセットに失敗");
                        }
#endif
                        MornApp.Quit();
                    }
                },
                destroyCancellationToken);
        }

        private void OnDestroy()
        {
            _current = null;
        }

        /// <summary> セーブデータを読み込む </summary>
        private TSaveData Load()
        {
            if (!TryLoad(out _current))
            {
                _current = CreateInstance();
                Save();
            }

            Bind(_current);
            return _current;
        }

        /// <summary> 現在のセーブデータを保存する </summary>
        public void Save()
        {
            Save(_current);
        }

        /// <summary> セーブデータをリセットする </summary>
        public void ResetData()
        {
            _current = CreateInstanceInitializingWith(Current);
            Bind(_current);
            DeleteSaveData();
            Save();
        }

        /// <summary> セーブデータを完全に削除する </summary>
        private static void DeleteSaveData()
        {
            // ファイルベースのセーブデータを削除
            try
            {
                var dirPath = MornSaveUtil.GetSaveDirPath();
                if (Directory.Exists(dirPath))
                {
                    Directory.Delete(dirPath, true);
                    MornSaveGlobal.Logger.Log($"セーブディレクトリを削除しました: {dirPath}");
                }
            }
            catch (Exception e)
            {
                MornSaveGlobal.Logger.LogError($"セーブディレクトリの削除に失敗: {e.Message}");
            }

            // PlayerPrefsを削除
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            MornSaveGlobal.Logger.Log("PlayerPrefsを削除しました");
        }
    }
}