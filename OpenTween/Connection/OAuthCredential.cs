// OpenTween - Client of Twitter
// Copyright (c) 2013      rhenium (@cn) <https://rhe.jp/>
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

namespace OpenTween.Connection
{
    public class OAuthCredential
    {
        public OAuthCredential(string consumerKey, string consumerSecret, string token, string tokenSecret)
            : this(new OAuthConsumer(consumerKey, consumerSecret), token, tokenSecret)
        { }

        public OAuthCredential(OAuthConsumer consumer, string token, string tokenSecret)
        {
            this.Consumer = consumer;
            this.Token = token;
            this.TokenSecret = tokenSecret;
        }

        public OAuthConsumer Consumer { get; private set; }
        public string Token { get; set; }
        public string TokenSecret { get; set; }
    }
}
