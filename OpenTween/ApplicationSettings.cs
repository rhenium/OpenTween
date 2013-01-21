// OpenTween - Client of Twitter
// Copyright (c) 2012      kim_upsilon (@kim_upsilon) <https://upsilo.net/~upsilon/>
// All rights reserved.
// 
// This file is part of OpenTween.
// 
// This program is free software; you can redistribute it and/or modify it
// under the terms of the GNU General public License as published by the Free
// Software Foundation; either version 3 of the License, or (at your option)
// any later version.
// 
// This program is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General public License
// for more details.
// 
// You should have received a copy of the GNU General public License along
// with this program. If not, see <http://www.gnu.org/licenses/>, or write to
// the Free Software Foundation, Inc., 51 Franklin Street - Fifth Floor,
// Boston, MA 02110-1301, USA.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTween
{
    /// <summary>
    /// アプリケーション固有の情報を格納します
    /// </summary>
    /// <remarks>
    /// OpenTweenA の派生版を作る方法は OpenTween と同じように http://sourceforge.jp/projects/opentween/wiki/HowToFork を参照して下さい。
    /// </remarks>
    internal sealed class ApplicationSettings
    {
        //=====================================================================
        // フィードバック送信先
        // 異常終了時などにエラーログ等とともに表示されます。

        /// <summary>
        /// フィードバック送信先 (メール)
        /// </summary>
        public const string FeedbackEmailAddress = "contact@re4k.info";

        /// <summary>
        /// フィードバック送信先 (Twitter)
        /// </summary>
        public const string FeedbackTwitterName = "@re4k";

        //=====================================================================
        // Web サイト

        /// <summary>
        /// 「ヘルプ」メニューの「(アプリ名) ウェブサイト」クリック時に外部ブラウザで表示する URL
        /// </summary>
        public const string WebsiteUrl = "http://sourceforge.jp/projects/opentween/wiki/FrontPage";

        /// <summary>
        /// 「ヘルプ」メニューの「ショートカットキー一覧」クリック時に外部ブラウザで表示する URL
        /// </summary>
        /// <remarks>
        /// Tween の Wiki ページのコンテンツはプロプライエタリなため転載不可
        /// </remarks>
        public const string ShortcutKeyUrl = "http://sourceforge.jp/projects/tween/wiki/%E3%82%B7%E3%83%A7%E3%83%BC%E3%83%88%E3%82%AB%E3%83%83%E3%83%88%E3%82%AD%E3%83%BC";

        //=====================================================================
        // アップデートチェック関連

        /// <summary>
        /// 最新バージョンの情報を取得するためのURL
        /// </summary>
        /// <remarks>
        /// version.txt のフォーマットについては http://sourceforge.jp/projects/opentween/wiki/VersionTxt を参照。
        /// </remarks>
        public const string VersionInfoUrl = "http://re4k.info/build/OpenTweenA/version.txt";

        //=====================================================================
        // Twitter
        // https://dev.twitter.com/ から取得できます。

        /// <summary>
        /// Twitter 標準 コンシューマーキー
        /// </summary>
        public const string TwitterDefaultConsumerKey = "N7R7UE6UDjuiTgAWEhA";
        public const string TwitterDefaultConsumerSecret = "9zhzUB0HRECSBvAF8KWdT1WktnB92iG1wuBb048OhE";

        //=====================================================================
        // Lockerz (旧Plixi)
        // https://admin.plixi.com/Api.aspx から取得できます。

        /// <summary>
        /// Lockerz APIキー
        /// </summary>
        public const string LockerzApiKey = "13d9bba7-84a7-488b-a5cc-d71857e45675";

        //=====================================================================
        // Twitpic
        // http://dev.twitpic.com/apps/new から取得できます。

        /// <summary>
        /// Twitpic APIキー
        /// </summary>
        public const string TwitpicApiKey = "9c0f0653ca6fdded658eb98fad61fa80";

        //=====================================================================
        // TwitVideo
        // http://twitvideo.jp/api_forms/ から申請できます。

        /// <summary>
        /// TwitVideo コンシューマキー
        /// </summary>
        public const string TwitVideoConsumerKey = "7c4dc004a88e821b02c87a0cde2fa85c";

        //=====================================================================
        // yfrog
        // http://stream.imageshack.us/api/ から取得できます。

        /// <summary>
        /// yfrog APIキー
        /// </summary>
        public const string YfrogApiKey = "0VZ48S5Ifbb92a499ba7844a6dda9c3d2e6c87a2";

        //=====================================================================
        // Foursquare
        // https://developer.foursquare.com/ から取得できます。

        /// <summary>
        /// Foursquare Client Id
        /// </summary>
        public const string FoursquareClientId = "JHCXRTFQD2NBAJZHQ1ADQII3M5RLRMSW4DSNCC5TXQ13HNV3";

        /// <summary>
        /// Foursquare Client Secret
        /// </summary>
        public const string FoursquareClientSecret = "U2BVK1J4KVPFIUSZCCRNKN0MBN4KDEBSDRDTARCDTOT1DPA0";

        //=====================================================================
        // TINAMI
        // http://www.tinami.com/api/ から取得できます。

        /// <summary>
        /// TINAMI APIキー
        /// </summary>
        public const string TINAMIApiKey = "4fc9d8d562ab1";
    }
}
