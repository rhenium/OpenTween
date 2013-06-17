// OpenTween - Client of Twitter
// Copyright (c) 2007-2011 kiri_feather (@kiri_feather) <kiri.feather@gmail.com>
//           (c) 2008-2011 Moz (@syo68k)
//           (c) 2008-2011 takeshik (@takeshik) <http://www.takeshik.org/>
//           (c) 2010-2011 anis774 (@anis774) <http://d.hatena.ne.jp/anis774/>
//           (c) 2010-2011 fantasticswallow (@f_swallow) <http://twitter.com/f_swallow>
//           (c) 2011      spinor (@tplantd) <http://d.hatena.ne.jp/spinor/>
//           (c) 2013      rhenium (@cn) <http://rhe.jp/>
// All rights reserved.
// 
// This file is part of OpenTween.
// 
// This program is free software; you can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the Free
// Software Foundation; either version 3 of the License, or (at your option)
// any later version.
// 
// This program is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY
// or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License
// for more details. 
// 
// You should have received a copy of the GNU General Public License along
// with this program. If not, see <http://www.gnu.org/licenses/>, or write to
// the Free Software Foundation, Inc., 51 Franklin Street - Fifth Floor,
// Boston, MA 02110-1301, USA.

using OpenTween.Connection;
using System;
using System.Collections.Generic; // for Dictionary<TKey, TValue>, List<T>, KeyValuePair<TKey, TValue>, SortedDictionary<TKey, TValue>
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace OpenTween
{
	/// <summary>
	/// OAuth認証を使用するHTTP通信。HMAC-SHA1固定
	/// </summary>
	/// <remarks>
	/// 使用前に認証情報を設定する。認証確認を伴う場合はAuthenticate系のメソッドを、認証不要な場合はInitializeを呼ぶこと。
	/// </remarks>
	public abstract class HttpConnectionOAuth : HttpConnection, IHttpConnection
	{
		/// <summary>
		/// OAuth署名のoauth_timestamp算出用基準日付（1970/1/1 00:00:00）
		/// </summary>
		private static readonly DateTime UnixEpoch = new DateTime( 1970, 1, 1, 0, 0, 0, DateTimeKind.Unspecified );

		/// <summary>
		/// OAuth署名のoauth_nonce算出用乱数クラス
		/// </summary>
		private static readonly Random NonceRandom = new Random();

		/// <summary>
		/// OAuthのアクセストークン
		/// </summary>
        public OAuthCredential Credential { get; private set; }

        /// <summary>
		/// Stream用のHttpWebRequest
		/// </summary>
		private HttpWebRequest streamReq = null;

		/// <summary>
		/// OAuth認証で指定のURLとHTTP通信を行い、結果を返す
		/// </summary>
		/// <param name="method">HTTP通信メソッド（GET/HEAD/POST/PUT/DELETE）</param>
		/// <param name="requestUri">通信先URI</param>
		/// <param name="param">GET時のクエリ、またはPOST時のエンティティボディ</param>
		/// <param name="content">[OUT]HTTP応答のボディデータ</param>
		/// <param name="headerInfo">[IN/OUT]HTTP応答のヘッダ情報。必要なヘッダ名を事前に設定しておくこと</param>
		/// <param name="callback">処理終了直前に呼ばれるコールバック関数のデリゲート 不要な場合はNothingを渡すこと</param>
		/// <returns>HTTP応答のステータスコード</returns>
		public HttpStatusCode GetContent( string method,
		                                  Uri requestUri,
		                                  Dictionary< string, string > param,
		                                  ref string content,
		                                  Dictionary< string, string > headerInfo,
		                                  CallbackDelegate callback )
		{
			// 認証済かチェック
			if (this.Credential == null)
				return HttpStatusCode.Unauthorized;

			HttpWebRequest webReq = this.CreateRequest( method, requestUri, param, false );
			// OAuth認証ヘッダを付加
            this.AppendOAuthInfo(webReq, param);

			HttpStatusCode code;
			if ( content == null )
				code = this.GetResponse( webReq, headerInfo, false );
			else
				code = this.GetResponse( webReq, out content, headerInfo, false );

			if ( callback != null )
			{
				StackFrame frame = new StackFrame( 1 );
				callback( frame.GetMethod().Name, code, content );
			}
			return code;
		}

		/// <summary>
		/// バイナリアップロード
		/// </summary>
		public HttpStatusCode GetContent( string method,
		                                  Uri requestUri,
		                                  Dictionary< string, string > param,
		                                  List< KeyValuePair< string, FileInfo > > binary, 
		                                  ref string content,
		                                  Dictionary< string, string > headerInfo,
		                                  CallbackDelegate callback )
		{
			// 認証済かチェック
			if (this.Credential == null)
				return HttpStatusCode.Unauthorized;

			HttpWebRequest webReq = this.CreateRequest( method, requestUri, param, binary, false );
			// OAuth認証ヘッダを付加
            this.AppendOAuthInfo(webReq, null);

			HttpStatusCode code;
			if ( content == null )
				code = this.GetResponse( webReq, headerInfo, false );
			else
				code = this.GetResponse( webReq, out content, headerInfo, false );

			if ( callback != null )
			{
				StackFrame frame = new StackFrame( 1 );
				callback( frame.GetMethod().Name, code, content );
			}
			return code;
		}

		/// <summary>
		/// OAuth認証で指定のURLとHTTP通信を行い、ストリームを返す
		/// </summary>
		/// <param name="method">HTTP通信メソッド（GET/HEAD/POST/PUT/DELETE）</param>
		/// <param name="requestUri">通信先URI</param>
		/// <param name="param">GET時のクエリ、またはPOST時のエンティティボディ</param>
		/// <param name="content">[OUT]HTTP応答のボディストリーム</param>
		/// <returns>HTTP応答のステータスコード</returns>
		public HttpStatusCode GetContent( string method,
		                                  Uri requestUri,
		                                  Dictionary< string, string > param,
		                                  ref Stream content,
		                                  string userAgent )
		{
			// 認証済かチェック
			if (this.Credential == null)
				return HttpStatusCode.Unauthorized;

			this.RequestAbort();
			this.streamReq = this.CreateRequest( method, requestUri, param, false );
			// User-Agent指定がある場合は付加
			if ( !string.IsNullOrEmpty( userAgent ) )
				this.streamReq.UserAgent = userAgent;

			// OAuth認証ヘッダを付加
            this.AppendOAuthInfo(this.streamReq, param);

			try
			{
				HttpWebResponse webRes = (HttpWebResponse)this.streamReq.GetResponse();
				content = webRes.GetResponseStream();
				return webRes.StatusCode;
			}
			catch ( WebException ex )
			{
				if ( ex.Status == WebExceptionStatus.ProtocolError )
				{
					HttpWebResponse res = (HttpWebResponse)ex.Response;
					return res.StatusCode;
				}
				throw;
			}
		}

		public void RequestAbort()
		{
			try
			{
				if ( this.streamReq != null )
				{
					this.streamReq.Abort();
					this.streamReq = null;
				}
			}
			catch ( Exception ) {}
		}

		#region "認証処理"
		/// <summary>
		/// OAuth認証のアクセストークン取得。PIN入力用の後段
		/// </summary>
		/// <remarks>
		/// 事前にAuthenticatePinFlowRequestを呼んで、ブラウザで認証後に表示されるPINを入力してもらい、その値とともに呼び出すこと
		/// </remarks>
		/// <param name="accessTokenUrl">アクセストークンの取得先URL</param>
		/// <param name="requestToken">AuthenticatePinFlowRequestで取得したリクエストトークン</param>
		/// <param name="pinCode">Webで認証後に表示されるPINコード</param>
		/// <returns>取得結果真偽値</returns>
        protected NameValueCollection GetAccessCredential(Uri accessTokenUri, string oauthVerifier)
		{
            var parameter = new Dictionary<string, string>() { { "oauth_verifier", oauthVerifier } };
            NameValueCollection ret;
            this.Credential = GetOAuthCredential(accessTokenUri, parameter, out ret);
            return ret;
		}

		/// <summary>
		/// OAuth認証のリクエストトークン取得。リクエストトークンと組み合わせた認証用のUriも生成する
		/// </summary>
		/// <param name="requestTokenUrl">リクエストトークンの取得先URL</param>
		/// <param name="authorizeUrl">ブラウザで開く認証用URLのベース</param>
		/// <param name="requestToken">[OUT]取得したリクエストトークン</param>
		/// <returns>取得結果真偽値</returns>
        protected NameValueCollection GetRequestCredential(Uri requestTokenUri, string oauthCallback = "oob")
		{
            var parameter = new Dictionary<string, string>() { { "oauth_callback", oauthCallback } };
            NameValueCollection ret;
            this.Credential = GetOAuthCredential(requestTokenUri, parameter, out ret);
            return ret;
		}

        private OAuthCredential GetOAuthCredential(Uri uri, Dictionary<string, string> parameter, out NameValueCollection ret)
        {
            var req = this.CreateRequest("POST", uri, parameter, false);
            AppendOAuthInfo(req, parameter);
            var header = new Dictionary<string, string>();
            string body;
            var code = GetResponse(req, out body, header, false);
            if (code == HttpStatusCode.OK)
            {
                ret = ParseQueryString(body);
                return new OAuthCredential(this.Credential.Consumer, ret["oauth_token"], ret["oauth_token_secret"]);
            }
            else
            {
                throw new Exception(body); // response body
            }
        }
		#endregion // 認証処理

		#region "OAuth認証用ヘッダ作成・付加処理"
		/// <summary>
		/// HTTPリクエストにOAuth関連ヘッダを追加
		/// </summary>
		/// <param name="webRequest">追加対象のHTTPリクエスト</param>
		/// <param name="query">OAuth追加情報＋クエリ or POSTデータ</param>
		/// <param name="token">アクセストークン、もしくはリクエストトークン。未取得なら空文字列</param>
		/// <param name="tokenSecret">アクセストークンシークレット。認証処理では空文字列</param>
        protected virtual void AppendOAuthInfo(HttpWebRequest req, Dictionary<string, string> parameter)
        {
            req.Headers.Add(HttpRequestHeader.Authorization, GetAuthorizationHeader(req.Method, req.RequestUri, parameter));
        }

        protected virtual string GetAuthorizationHeader(string method, Uri uri, Dictionary<string, string> parameters = null, string realm = null)
        {
            // OAuth共通情報取得
            var oauthParameter = this.GetOAuthParameter();
            // 署名の作成・追加
            oauthParameter.Add("oauth_signature", this.CreateSignature(Credential.TokenSecret, method, uri, oauthParameter, parameters));
            // HTTPリクエストのヘッダに追加
            var mg = string.Join(", ",
                oauthParameter
                .Where(item => item.Key.StartsWith("oauth_") || item.Key == "realm")
                .Select(item => item.Key + "=\"" + UrlEncode(item.Value) + "\""));
            if (!string.IsNullOrEmpty(realm))
                mg = "realm=\"" + realm + "\", " + mg;
            return "OAuth " + mg;
        }

		/// <summary>
		/// OAuthで使用する共通情報を取得する
		/// </summary>
		/// <param name="token">アクセストークン、もしくはリクエストトークン。未取得なら空文字列</param>
		/// <returns>OAuth情報のディクショナリ</returns>
		protected Dictionary< string, string > GetOAuthParameter()
		{
			Dictionary< string, string > parameter = new Dictionary< string, string >();
			parameter.Add( "oauth_consumer_key", this.Credential.Consumer.Key );
			parameter.Add( "oauth_signature_method", "HMAC-SHA1" );
			parameter.Add( "oauth_timestamp", Convert.ToInt64( ( DateTime.UtcNow - HttpConnectionOAuth.UnixEpoch ).TotalSeconds ).ToString() ); // epoch秒
			parameter.Add( "oauth_nonce", HttpConnectionOAuth.NonceRandom.Next( 123400, 9999999 ).ToString() );
			parameter.Add( "oauth_version", "1.0" );
            if (!string.IsNullOrEmpty(Credential.Token))
                parameter.Add("oauth_token", Credential.Token); // トークンがあれば追加
			return parameter;
		}

		/// <summary>
		/// OAuth認証ヘッダの署名作成
		/// </summary>
		/// <param name="tokenSecret">アクセストークン秘密鍵</param>
		/// <param name="method">HTTPメソッド文字列</param>
		/// <param name="uri">アクセス先Uri</param>
		/// <param name="oauthParameters">クエリ、もしくはPOSTデータ</param>
		/// <returns>署名文字列</returns>
		protected string CreateSignature( string tokenSecret, string method, Uri uri, Dictionary< string, string > oauthParameters, Dictionary<string,string> parameters )
		{
			// パラメタをソート済みディクショナリに詰替（OAuthの仕様）
            SortedDictionary<string, string> sorted = new SortedDictionary<string, string>(oauthParameters);
            if (parameters != null)
                foreach (var item in parameters)
                    sorted.Add(item.Key, item.Value);
			// URLエンコード済みのクエリ形式文字列に変換
			string paramString = this.CreateQueryString( sorted );
			// アクセス先URLの整形
			string url = string.Format( "{0}://{1}{2}", uri.Scheme, uri.Host, uri.AbsolutePath );
			// 署名のベース文字列生成（&区切り）。クエリ形式文字列は再エンコードする
			string signatureBase = string.Format( "{0}&{1}&{2}", method, this.UrlEncode( url ), this.UrlEncode( paramString ) );
			// 署名鍵の文字列をコンシューマー秘密鍵とアクセストークン秘密鍵から生成（&区切り。アクセストークン秘密鍵なくても&残すこと）
			string key = this.UrlEncode( this.Credential.Consumer.Secret ) + "&";
			if ( !string.IsNullOrEmpty( tokenSecret ) )
				key += this.UrlEncode( tokenSecret );
			// 鍵生成＆署名生成
			using ( HMACSHA1 hmac = new HMACSHA1( Encoding.ASCII.GetBytes( key ) ) )
			{
				byte[] hash = hmac.ComputeHash( Encoding.ASCII.GetBytes( signatureBase ) );
				return Convert.ToBase64String( hash );
			}
		}
		#endregion // OAuth認証用ヘッダ作成・付加処理

		/// <summary>
		/// 初期化。各種トークンの設定とユーザー識別情報設定
		/// </summary>
        /// <param name="credential">アクセスクレデンシャル</param>
		public HttpConnectionOAuth(OAuthCredential credential)
		{
            this.Credential = credential;
		}

		/// <summary>
		/// 初期化。各種トークンの設定とユーザー識別情報設定
		/// </summary>
		/// <param name="consumerKey">コンシューマー鍵</param>
		/// <param name="consumerSecret">コンシューマー秘密鍵</param>
		/// <param name="accessToken">アクセストークン</param>
		/// <param name="accessTokenSecret">アクセストークン秘密鍵</param>
		/// <param name="username">認証済みユーザー名</param>
		/// <param name="userIdentifier">アクセストークン取得時に得られるユーザー識別情報。不要なら空文字列</param>
        public HttpConnectionOAuth(OAuthConsumer consumer)
            : this(new OAuthCredential(consumer, null, null))
        { }
	}
}
