# MornSave

## 概要

セーブデータ管理システム。ファイル/PlayerPrefsに自動保存するライブラリ。

## 依存関係

| 種別 | 名前 |
|------|------|
| 外部パッケージ | Steam（オプション: USE_STEAM定義時） |
| Mornライブラリ | MornLib |

## 使い方

### カスタムセーブマネージャーの作成

```csharp
public class GameSaveManager : MornSaveManager<GameSaveData>
{
    protected override GameSaveData CreateInstance()
    {
        return new GameSaveData();
    }

    protected override bool TryLoad(out GameSaveData result)
    {
        // ロード処理
    }

    protected override void Save(GameSaveData data)
    {
        // セーブ処理
    }
}
```

### 使用方法

```csharp
// データの取得（自動読み込み）
var data = saveManager.Current;

// 手動保存
saveManager.Save();

// リセット
saveManager.ResetData();
```
