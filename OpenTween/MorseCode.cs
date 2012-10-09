// OpenTween - Client of Twitter
// Copyright (c) 2012      re4k (@re4k) <http://re4k.info/>
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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace OpenTween
{
    public class TupleList<T1, T2> : List<Tuple<T1, T2>>
    {
        public void Add(T1 item1, T2 item2)
        {
            this.Add(new Tuple<T1, T2>(item1, item2));
        }
    }

    public static class MorseCode
    {
        public static string ParseJapaneseMorseCode(string str)
        {
            return str
                .Replace("&nbsp;", " ")
                .Split(' ')
                .Select(_ =>
                {
                    if (JapaneseMorseCodeList.Exists(__=>__.Item2 == _))
                    {
                        return JapaneseMorseCodeList.First(__ => __.Item2 == _).Item1.ToString();
                    }
                    else
                    {
                        return _;
                    }
                })
                .Aggregate(new StringBuilder(), (sb, _) => sb.Append(_))
                .ToString();
        }

        public static string ToJapaneseMorseCode(string str)
        {
            return string.Join(" ", str
                .ToCharArray()
                .Select(_ =>
                {
                    if (JapaneseMorseCodeList.Exists(__ => string.Compare(__.Item1.ToString(), _.ToString(), CultureInfo.CurrentCulture, CompareOptions.IgnoreKanaType) == 0))
                    {
                        return JapaneseMorseCodeList.First(__ => string.Compare(__.Item1.ToString(), _.ToString(), CultureInfo.CurrentCulture, CompareOptions.IgnoreKanaType) == 0).Item2;
                    }
                    else
                    {
                        return _.ToString();
                    }
                })
                .Aggregate(new List<string>(), (ls, _) => { ls.Add(_); return ls; }));
                
        }

        public static bool IsJapaneseMorseCode(string str)
        {
            return str
                .Replace("&nbsp;", " ")
                .Split(' ')
                .All(_ => JapaneseMorseCodeList.Exists(__ => __.Item2 == _));
        }

        private static readonly TupleList<char, string> JapaneseMorseCodeList = new TupleList<char, string>
        {
            {'イ', "・－"},
            {'ロ', "・－・－"},
            {'ハ', "－・・・"},
            {'ニ', "－・－・"},
            {'ホ', "－・・"},
            {'ヘ', "・"},
            {'ト', "・・－・・"},
            {'チ', "・・－・"},
            {'リ', "－－・"},
            {'ヌ', "・・・・"},
            {'ル', "－・－－・"},
            {'ヲ', "・－－－"},
            {'ワ', "－・－"},
            {'カ', "・－・・"},
            {'ヨ', "－－"},
            {'タ', "－・"},
            {'レ', "－－－"},
            {'ソ', "－－－・"},
            {'ツ', "・－－・"},
            {'ネ', "－－・－"},
            {'ナ', "・－・"},
            {'ラ', "・・・"},
            {'ム', "－"},
            {'ウ', "・・－"},
            {'ヰ', "・－・・－"},
            {'ノ', "・・－－"},
            {'オ', "・－・・・"},
            {'ク', "・・・－"},
            {'ヤ', "・－－"},
            {'マ', "－・・－"},
            {'ケ', "－・－－"},
            {'フ', "－－・・"},
            {'コ', "－－－－"},
            {'エ', "－・－－－"},
            {'テ', "・－・－－"},
            {'ア', "－－・－－"},
            {'サ', "－・－・－"},
            {'キ', "－・－・・"},
            {'ユ', "－・・－－"},
            {'メ', "－・・・－"},
            {'ミ', "・・－・－"},
            {'シ', "－－・－・"},
            {'ヱ', "・－－・・"},
            {'ヒ', "－－・・－"},
            {'モ', "－・・－・"},
            {'セ', "・－－－・"},
            {'ス', "－－－・－"},
            {'ン', "・－・－・"},
            {'゛', "・・"},
            {'゜', "・・－－・"},
            {'ー', "・－－・－"},
            {'、', "・－・－・－"},
            {'」', "・－・－・・"},
            {'（', "－・－－・－"},
            {'）', "・－・・－・"},
            //数字
            {'1',"・－－－－"},
            {'2',"・・－－－"},
            {'3',"・・・－－"},
            {'4',"・・・・－"},
            {'5',"・・・・・"},
            {'6',"－・・・・"},
            {'7',"－－・・・"},
            {'8',"－－－・・"},
            {'9',"－－－－・"},
            {'0',"－－－－－"},
            //その他記号
            {'@', "・－－・－・"},
            //濁点半濁点付き文字
            {'ガ', "・－・・ ・・"},
            {'ギ', "－・－・・ ・・"},
            {'グ', "・・・－ ・・"},
            {'ゲ', "－・－－ ・・"},
            {'ゴ', "－－－－ ・・"},
            {'ザ', "－・－・－ ・・"},
            {'ジ', "－－・－・ ・・"},
            {'ズ', "－－－・－ ・・"},
            {'ゼ', "・－－－・ ・・"},
            {'ゾ', "－－－・ ・・"},
            {'ダ', "－・ ・・"},
            {'ヂ', "・・－・ ・・"},
            {'ヅ', "・－－・ ・・"},
            {'デ', "・－・－－ ・・"},
            {'ド', "・・－・・ ・・"},
            {'バ', "－・・・ ・・"},
            {'ビ', "－－・・－ ・・"},
            {'ブ', "－－・・ ・・"},
            {'ベ', "・ ・・"},
            {'ボ', "－・・ ・・"},
            {'パ', "－・・・ ・・－－・"},
            {'ピ', "－－・・－ ・・－－・"},
            {'プ', "－－・・ ・・－－・"},
            {'ペ', "・ ・・－－・"},
            {'ポ', "－・・ ・・－－・"},
            {'ヴ', "・・－ ・・"},
            //小さい文字
            {'ァ', "－－・－－"},
            {'ィ', "・－"},
            {'ゥ', "・・－"},
            {'ェ', "－・－－－"},
            {'ォ', "・－・・・"},
            {'ヵ', "・－・・"},
            {'ヶ', "－・－－"},
            {'ッ', "・－－・"},
            {'ャ', "・－－"},
            {'ュ', "－・・－－"},
            {'ョ', "－－"},
            {'ヮ', "－・－"}
        };
    }
}
