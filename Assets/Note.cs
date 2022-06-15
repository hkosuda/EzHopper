/*
 * 【メモ】
 * ez_square2の難易度：
 *  序盤に半回転をはさみ，一番右のバーを渡っていく方法（01）
 *      半回転のあとの切り返しが難しい
 *  右側の長いバーを使い，右にクロスしつつ正面に抜けてゴールする方法（02）
 *      それなりに速度を稼がないといけない．
 *  右側の長いバーを使った後，左に斜行していく方法（03）
 *      切り返しやクロスの必要がないので，あまり難しくない．
 *  右側の長いバーを使い，短いバーに乗ったあと大きく左に切り返してゴールする方法（04）
 *      速度を稼がないといけないため，やや難しい．
 *  右側のトリプルのうちふたつを使い，その後フラットバーを使う方法（05）
 *      ・切り返しがきつい．
 *  正面のツリーの右側，フラットを使ったあと正面に抜けていく方法（06）
 *      ・おそらく最も簡単．
 *  正面のツリーの右側，ふたつのフラットを使ったあと，激しく視点を振り同じツリーのバーを使う方法（07）
 *      ・スピードをセーブしつつ，激しく視点を振らなければならない．しかし，コツをつかむと意外と簡単．
 *  正面のツリーで半回転をしたあと，正面に切り返し短いバーをふたつ使う方法（08）
 *      ・かなり難しい．急な切り返しのあとに視点を細かく振って加速しなければならない．
 *  左側の並行バーを使い，右に二回跳んだあと中央付近のトリプルの右側のバーを使い，その後同じツリーの二本のバーを使い，最後にゴール直前のバーを使う方法（09）
 *      ・かなり難しい．スピードに乗った状態で切り返しを決めなければならない．そもそもトリプルに乗るのも難しい．
 *  ・並行バーを使ったあと，クロスしつつｚ方向のバーを使いそのままゴールまでほぼ真っ直ぐ抜けていく方法（10）
 *      ・簡単．ただしスピードを要求される．
 *  上記の途中で，同じツリーでの切り返しを行ったと大きく右にシフトしそのまま中央を抜ける方法（11）
 *      ・右にシフトするときの正確なジャンプが結構難しい．
 *  左側を抜けつつ，途中で左に急に向きを変えてそのまま左前のツリーを使う方法（12）
 *      ・左にシフトしたあとの切り返しが若干難しいだけ．
 *  左側のフラットを使った後左に切り返し，同じツリーで半回転，再び左前の同じツリーを使う方法（13）
 *      ・ツリーのバーが二本とも短いのでなかなか難しい．
 *  左側のフラットを使った後，正面に抜けていく方法（14）
 *      ・切り返しも何もないシンプルな方法だが，短い間に大きく加速しなければならないため，非常に難しい．
 *  一番左をひたすら進む方法（15）
 *      ・切り返しが連続する，結構楽しい攻略法．切り返しが得意ならたいして苦労しない．
 *  左側を進んだのち，右に大きく切り返す方法（16）
 *      ・柱に多少体をこすってもなんとかなる．やや難しい．
 *      
 *      
 *  【難易度順】
 *  14  (16
 *  08  (15
 *  09  (14
 *  07  (13
 *  01  (12
 *  11  (11
 *  13  (10
 *  05  (9
 *  16  (8
 *  12  (7
 *  04  (6
 *  02  (5
 *  15  (4
 *  10  (3
 *  03  (2
 *  06  (1
 * 
 * 
 * 【残りの作業】
 * ・デモのかくつきの改善．
 * ・最初のマップに，おすすめバインド設定の追加．
 * ・設定をコピー，ペーストできる機能．
 * 
 * 【おすすめ設定について】
 * ・おすすめキーバインド，トグル設定
 *      バインド
 *          P : anchor set -f
 *          V : anchor back -m
 *          Z : ghost end -m
 *          Q : timer now -f
 *          R : replay -m
 *          L : recorder remove_last
 *      トグル
 *          O : observer start -f | observer end -f
 *          E : recorder start -f | recorder end -f
 *          Q : timer start -f | timer stop -f
 *          
 * ・おすすめ通知機能
 *      次のチェックポイントに到達したときに，回数に関するメッセージを表示する．
 *          invoke add on_map_changed "counter set 0"
 *          invoke add on_enter_next_checkpoint "chat/クリアに要した回数：%counter%"
 *          invoke add on_enter_next_checkpoint "counter set 0"
 *          
 *      ゴールに到達したときに，メッセージを表示する．
 *          invoke add on_map_chanted "counter "
 *          
 *  ・ネタ設定
 *      ミスするたびに，味方に罵られる
 *          invoke add on_course_out "delayedchat 0.5 1.5 ..."
 *          invoke add on_course_out "delayedchat 0.5 1.5 ?"
 *          ... など
 *      
 *      トグルで，クロスヘアを投げ物用に切り替える
 *          toggle crosshair_length 6 | crosshair length 999
 *          
 *  おすすめInvoke設定
 *      スタートからゴールまでのタイムアタック
 *          invoke add on_course_out "back 0 -m"
 *          invoke add on_exit_start "recorder start -f"
 *          invoke add on_enter_goal "recorder end -f"
 *          invoke add on_enter_goal "recorder save %map%_%now% -f"
 *          invoke add on_exit_start "timer start -f"
 *          invoke add on_enter_goal "timer stop -m"
 *          invoke add on_enter_goal "timer now -f"
 *          recorder_limit_time 599
 *      
 *      デフォルト設定
 *          invoke add on_course_out "back -m"
 *          
 *          invoke add on_exit_checkpoint "recorder start -f"
 *          invoke add on_enter_checkpoint "recorder end -f"
 *          invoke add on_enter_next_checkpoint "recorder save %map%_%now% -f"
 *          invoke add on_course_out "recorder stop -f"
 *          
 *          invoke add on_exit_checkpoint "timer start -f"
 *          invoke add on_enter_next_checkpoint "timer stop -m"
 *          invoke add on_enter_next_checkpoint "timer now -f"
 *          
 *          invoke add on_enter_checkpoint "ghost -m"
 *          invoke add on_exit_checkpoint "ghost end -m"
 *          
 *  【やるべきこと（残り）】
 *  ・おすすめ設定の配置
 *  ・遊び方の説明の改善（もっと動画を活用しよう）
 */
