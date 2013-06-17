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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTween.Connection
{
    public class OAuthConsumer
    {
        public OAuthConsumer(string consumerKey, string consumerSecret)
        {
            this.Key = consumerKey;
            this.Secret = consumerSecret;
        }

        public string Key { get; set; }
        public string Secret { get; set; }
    }
}
