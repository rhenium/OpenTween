// OpenTween - Client of Twitter
// Copyright (c) 2007-2011 kiri_feather (@kiri_feather) <kiri.feather@gmail.com>
//           (c) 2008-2011 Moz (@syo68k)
//           (c) 2008-2011 takeshik (@takeshik) <http://www.takeshik.org/>
//           (c) 2010-2011 anis774 (@anis774) <http://d.hatena.ne.jp/anis774/>
//           (c) 2010-2011 fantasticswallow (@f_swallow) <http://twitter.com/f_swallow>
//           (c) 2011      kim_upsilon (@kim_upsilon) <https://upsilo.net/~upsilon/>
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
// with this program. if (not, see <http://www.gnu.org/licenses/>, or write to
// the Free Software Foundation, Inc., 51 Franklin Street - Fifth Floor,
// Boston, MA 02110-1301, USA.

using System.Text.RegularExpressions;
using System.Web;
using System.Collections.Generic;
using System;

namespace OpenTween
{
    public class ShortUrl
    {
        private static string[] _ShortUrlService = {
            "http://t.co/",
            "http://tinyurl.com/",
            "http://is.gd/",
            "http://bit.ly/",
            "http://j.mp/",
            "http://goo.gl/",
            "http://htn.to/",
            "http://amzn.to/",
            "http://flic.kr/",
            "http://ux.nu/",
            "http://youtu.be/",
            "http://p.tl/",
            "http://nico.ms",
            "http://moi.st/",
            "http://snipurl.com/",
            "http://snurl.com/",
            "http://nsfw.in/",
            "http://icanhaz.com/",
            "http://tiny.cc/",
            "http://urlenco.de/",
            "http://linkbee.com/",
            "http://traceurl.com/",
            "http://twurl.nl/",
            "http://cli.gs/",
            "http://rubyurl.com/",
            "http://budurl.com/",
            "http://ff.im/",
            "http://twitthis.com/",
            "http://blip.fm/",
            "http://tumblr.com/",
            "http://www.qurl.com/",
            "http://digg.com/",
            "http://ustre.am/",
            "http://pic.gd/",
            "http://airme.us/",
            "http://qurl.com/",
            "http://bctiny.com/",
            "http://ow.ly/",
            "http://bkite.com/",
            "http://dlvr.it/",
            "http://ht.ly/",
            "http://tl.gd/",
            "http://feeds.feedburner.com/",
            "http://on.fb.me/",
            "http://fb.me/",
            "http://tinami.jp/",
        };

        private static bool _isresolve = true;
        private static bool _isForceResolve = true;
        private static Dictionary<string, string> urlCache = new Dictionary<string, string>();

        private static readonly object _lockObj = new object();

        public static bool IsResolve
        {
            get { return _isresolve; }
            set { _isresolve = value; }
        }

        public static bool IsForceResolve
        {
            get { return _isForceResolve; }
            set { _isForceResolve = value; }
        }

        public static string Resolve(string orgData, bool tcoResolve)
        {
            if (!_isresolve) return orgData;
            lock (_lockObj)
            {
                if (urlCache.Count > 500)
                {
                    urlCache.Clear(); //定期的にリセット
                }
            }

            List<string> urlList = new List<string>();
            MatchCollection m = Regex.Matches(orgData, "<a href=\"(?<svc>http://.+?/)(?<path>[^\"]+)?\"", RegexOptions.IgnoreCase);
            foreach (Match orgUrlMatch in m)
            {
                string orgUrl = orgUrlMatch.Result("${svc}");
                string orgUrlPath = orgUrlMatch.Result("${path}");
                if ((_isForceResolve || Array.IndexOf(_ShortUrlService, orgUrl) > -1) &&
                   !urlList.Contains(orgUrl + orgUrlPath) && orgUrl != "http://twitter.com/")
                {
                    if (!tcoResolve && (orgUrl == "http://t.co/" || orgUrl == "https://t.co")) continue;
                    lock (_lockObj)
                    {
                        urlList.Add(orgUrl + orgUrlPath);
                    }
                }
            }

            foreach (string orgUrl in urlList)
            {
                if (urlCache.ContainsKey(orgUrl))
                {
                    try
                    {
                        orgData = orgData.Replace("<a href=\"" + orgUrl + "\"", "<a href=\"" + urlCache[orgUrl] + "\"");
                    }
                    catch (Exception)
                    {
                        //Through
                    }
                }
                else
                {
                    try
                    {
                        //urlとして生成できない場合があるらしい
                        //string urlstr = new Uri(urlEncodeMultibyteChar(orgUrl)).GetLeftPart(UriPartial.Path);
                        string retUrlStr = "";
                        string tmpurlStr = new Uri(MyCommon.urlEncodeMultibyteChar(orgUrl)).GetLeftPart(UriPartial.Path);
                        HttpVarious httpVar = new HttpVarious();
                        retUrlStr = MyCommon.urlEncodeMultibyteChar(httpVar.GetRedirectTo(tmpurlStr));
                        if (retUrlStr.StartsWith("http"))
                        {
                            retUrlStr = retUrlStr.Replace("\"", "%22");  //ダブルコーテーションがあるとURL終端と判断されるため、これだけ再エンコード
                            orgData = orgData.Replace("<a href=\"" + tmpurlStr, "<a href=\"" + retUrlStr);
                            lock (_lockObj)
                            {
                                if (!urlCache.ContainsKey(orgUrl)) urlCache.Add(orgUrl, retUrlStr);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        //Through
                    }
                }
            }

            return orgData;
        }

        public static string ResolveMedia(string orgData, bool tcoResolve)
        {
            if (!_isresolve) return orgData;
            lock (_lockObj)
            {
                if (urlCache.Count > 500)
                    urlCache.Clear(); //定期的にリセット
            }
            
            Match m = Regex.Match(orgData, "(?<svc>https?://.+?/)(?<path>[^\"]+)?", RegexOptions.IgnoreCase);
            if (m.Success)
            {
                string orgUrl = m.Result("${svc}");
                string orgUrlPath = m.Result("${path}");
                if ((_isForceResolve ||
                    Array.IndexOf(_ShortUrlService, orgUrl) > -1) && orgUrl != "http://twitter.com/")
                {
                    if (!tcoResolve && (orgUrl == "http://t.co/" || orgUrl == "https://t.co/")) return orgData;
                    orgUrl += orgUrlPath;
                    if (urlCache.ContainsKey(orgUrl))
                    {
                        return orgData.Replace(orgUrl, urlCache[orgUrl]);
                    }
                    else
                    {
                        try
                        {
                            //urlとして生成できない場合があるらしい
                            //string urlstr = new Uri(urlEncodeMultibyteChar(orgUrl)).GetLeftPart(UriPartial.Path);
                            string retUrlStr = "";
                            string tmpurlStr = new Uri(MyCommon.urlEncodeMultibyteChar(orgUrl)).GetLeftPart(UriPartial.Path);
                            HttpVarious httpVar = new HttpVarious();
                            retUrlStr = MyCommon.urlEncodeMultibyteChar(httpVar.GetRedirectTo(tmpurlStr));
                            if (retUrlStr.StartsWith("http"))
                            {
                                retUrlStr = retUrlStr.Replace("\"", "%22");  //ダブルコーテーションがあるとURL終端と判断されるため、これだけ再エンコード
                                lock (_lockObj)
                                {
                                    if (!urlCache.ContainsKey(orgUrl)) urlCache.Add(orgUrl, orgData.Replace(tmpurlStr, retUrlStr));
                                }
                                return orgData.Replace(tmpurlStr, retUrlStr);
                            }
                        }
                        catch (Exception)
                        {
                            return orgData;
                        }
                    }
                }
            }

            return orgData;
        }
    }
}