# ライブラリの解説
## 収録されているライブラリ
* CmdSys
* Messaging
## CmdSys
### 概要
オブジェクト等の動作をコマンドで制御するための機能を提供します
### 詳細
ゲーム等の毎フレームUpdateを呼ぶようなプログラムでは逐次処理をしたいときに以下のようなコードになりがちです
```c#
void Update() {
    var frameCount = GetFrameCount();
    if(frameCount == 0) {
        m_delta_angle = 0.0f;
        m_delta_speed = -0.2f;
        m_deletion_resistance = 30;
    }
    if (frameCount < 20) {
        m_delta_angle += (Mathf.PI / 6.0f) / 20;
        m_delta_speed += -0.2f / 20;
    }
    if (frameCount == 40) {
        m_deletion_resistance = 0;
    }
}
```
この程度であればまだ全然読めますが、大規模になってくるとだんだん読めなくなっていくと思います。
できれば分岐を最小限に抑えて書きたいところですよね。
そういう時にこのCmdSysを使えば、以下のように書けます。

<span style="color: red; ">※この例で書かれているコマンドが実際のサンプルで実装されているとは限りません。ご注意ください</span>

```c#
void Start()　{
    scheduler.EnqueueCommand(new SetVariable<float>("deltaAngle", 0.0f));
    scheduler.EnqueueCommand(new SetVariable<float>("deltaSpeed", -0.2f));
    scheduler.EnqueueCommand(new SetVariable<int>("deletionResistance", 30));
    scheduler.EnqueueCommand(new AddVariablesOverFrames
        (
            20, // 20フレームかける
            new List<(string, object, Type)>()
            {
                ("deltaAngle", Mathf.PI / 6.0f, typeof(float)),
                ("deltaSpeed", -0.2f, typeof(float)),
            }
        )
    );
    scheduler.EnqueueCommand(new Wait(20));
    scheduler.EnqueueCommand(new SetVariable<int>("deletionResistance", 0));
}

void Update()　{
    // nothing to do.
}
```
先ほどのコードと見比べたときに、それぞれの操作の意図がよりわかりやすくなり、読みやすくなっていると感じると思います。

また、コマンドのパターンを複数用意しておいて、それをランダムに並べるだけでほぼ破綻なくランダムな敵の動きを実装できます。便利ですね。

## Messaging
### 概要
モジュール間の通信を、それぞれにほとんど依存せずに行う機能を提供します。
### 詳細
スコアを計算する機能を実装する場合を考えてみましょう。
何も工夫せずに実装しようとすると以下のようなコードになると思います
```c#
public class ScoreManager {
    public int score { get; private set; }
    public float scoreRate { get; set; }  = 1.0f;

    public void AddScore(int basePoint) {
        score += (int)(basePoint * scoreRate);
    }
}

public class Enemy {
    public ScoreManager scoreManager { private get; set; }

    public void Dead() {
        scoreManager.AddScore(1000);

        var rate = scoreManager.scoreRate;
        rate += 0.01f;
        scoreManager.scoreRate = rate;
    }
}
```
この実装ではEnemyがScoreManagerに強く依存してしまっています。
Messagingを使えば、以下のようにそういった依存性を弱めることができます
```c#
// GameMainのようなクラスがMessageTray<string>を持っていることを想定

public class ScoreManager {
    public int score { get; private set; }
    public float scoreRate { get; private set; } = 1.0f;
    private Dictionary<string, (int, float)> m_score_additions =
        new Dictionary<string, (int, float)>() {
            {"EnemyDead", (1000, 0.01)}
        };

    public void Update() {
        var instance = GameMain.Instance;
        while (instance.messageTray.TryQuery("AddScore", out string scoreAdditionalType)) {
            if(m_score_additions.contains(scoreAdditionalType)) {
                var scoreAdd = m_score_additions[scoreAdditionalType];
                m_addScore(scoreAdd.Item1);
                m_addScoreRate(scoreAdd.Item2);
            }
        }
    }

    private void m_addScore(int basePoint) {
        score += (int)(basePoint * scoreRate);
    }

    private void m_addScoreRate(float addition) {
        scoreRate += addition;
    }
}

public class Enemy {
    public void Dead() {
        var instance = GameMain.Instance;
        instance.messageTray.Post("AddScore", "EnemyDead");
    }
}
```
Messagingを利用することで、送信側は何を使えるかだけを気にすればよく、スコアの仕組みやどこに送るのかを意識する必要はなく、受信側はメッセージをどう処理するかだけを気にすればよく、だれがそのメッセージを送ったのか等を意識する必要がなくなります。

このように役割を徹底的に分離できるようになり、結果としてコードの保守性や拡張性が向上します

また、スコア処理の他にも「効果音再生」「エフェクト表示」「実績解除」などをメッセージに紐づけることもでき、モジュール間の結合度を下げながら機能追加を行えます。