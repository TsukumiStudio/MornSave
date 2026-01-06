namespace MornLib
{
    /// <summary> MornSaveモジュール用のログ出力クラス </summary>
    internal static class MornSaveLogger
    {
        /// <summary> Debug.Logラッパー </summary>
        public static void Log(string message)
        {
            MornSaveGlobal.I.Logger.LogInternal(message);
        }

        /// <summary> Debug.LogErrorラッパー </summary>
        public static void LogError(string message)
        {
            MornSaveGlobal.I.Logger.LogErrorInternal(message);
        }

        /// <summary> Debug.LogWarningラッパー </summary>
        public static void LogWarning(string message)
        {
            MornSaveGlobal.I.Logger.LogWarningInternal(message);
        }
    }
}