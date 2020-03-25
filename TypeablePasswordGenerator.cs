/*
  TypeablePasswordGenerator Plugin
  Copyright (C) 2020 Marc-André Harvey <TBD email address>

  This program is free software; you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation; either version 3 of the License, or
  (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with this program; if not, write to the Free Software
  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using KeePassLib;
using KeePassLib.Cryptography;
using KeePassLib.Cryptography.PasswordGenerator;
using KeePassLib.Security;

namespace TypeablePasswordGenerator
{
	public sealed class TypeablePasswordGenerator : CustomPwGenerator
	{
		/* Created using Visual Studio 2019 "Create GUID" feature */
		private static readonly PwUuid m_uuid = new PwUuid(new byte[] {
			0x21, 0xFD, 0x69, 0x52, 0xC4, 0x1D, 0x43, 0x93,
			0xA2, 0x0D, 0x9E, 0xBA, 0x9B, 0xB6, 0x45, 0x5C
		});
		public override PwUuid Uuid
		{
			get { return m_uuid; }
		}

		public override string Name
		{
			get { return "TypeablePasswordGenerator"; }
		}

		public override ProtectedString Generate(PwProfile prf,
			CryptoRandomStream crsRandomSource)
		{
			/* 
			 * Step #0 : input validation
			 */
			if (prf == null) { Debug.Assert(false); }
			else
			{
				Debug.Assert(prf.CustomAlgorithmUuid == Convert.ToBase64String(
					m_uuid.UuidBytes, Base64FormattingOptions.None));
			}

			/* 
			 * Step #1 : configuration
			 */
			List<string> l_charsets = new List<string>();
			List<char> l_separators = new List<char>();
			/* here we should parse the options regarding which character sets to use */
			/* let's just use some fixed values for now */
			l_charsets.Add(PwCharSet.UpperCase);
			l_charsets.Add(PwCharSet.LowerCase);
			l_charsets.Add(PwCharSet.Digits);
			l_charsets.Add("#@$()&_=+-?!/%:'\" *");
			l_charsets.Add("£€¥¢©®™~¿[] {} <>^¡`;÷\\|¦¬×§¶°");

			l_separators.Add(' ');
			l_separators.Add('.');
			l_separators.Add('-');
			l_separators.Add('_');

			int i_password_length = 50;
			int i_min_sep_lenght = 2;
			int i_max_sep_lenght = 3;

			/* 
			 * Step #2 : setup
			 */
			/* Pick a fixed separator for the whole password */
			int i_sep_lenght = GetRandInt(i_min_sep_lenght, i_max_sep_lenght+1, crsRandomSource);
			string s_separator = "";
			char c_separator = GetRandListElement(l_separators, crsRandomSource);
			while (s_separator.Length < i_sep_lenght)
			{
				s_separator += c_separator;
			}
			l_separators = null;


			/*
			 * Step #3 : iterative generation
			 */
			int i_generated_length = 0;
			List<string> l_charGroups = new List<string>(l_charsets.Count);
			while (l_charGroups.Count < l_charsets.Count)
			{
				l_charGroups.Add("");
			}

			while (i_generated_length + (s_separator.Length * (l_charsets.Count-1)) < i_password_length)
			{
				int i_charset = GetRandInt(l_charsets.Count, crsRandomSource);
				l_charGroups[i_charset] += GetRandCharFromString(l_charsets[i_charset], crsRandomSource);
				i_generated_length ++;
			}

			ShuffleList(l_charGroups, crsRandomSource);
			string s_generated_password = l_charGroups[0];
			for (int i=1; i < l_charGroups.Count; ++i)
			{
				s_generated_password += s_separator;
				s_generated_password += l_charGroups[i];
			}

			return new ProtectedString(false, s_generated_password);
		}

		private static void ShuffleList<T>(List<T> l, CryptoRandomStream crs)
		{
			if(l == null) { Debug.Assert(false); return; }
			if(l.Count <= 1) return; // Nothing to shuffle

			for(int i = l.Count - 1; i >= 1; --i)
			{
				ulong u = crs.GetRandomUInt64();
				u %= (ulong)(i + 1);
				int j = (int)u;

				if(i == j) continue;

				T t = l[i];
				l[i] = l[j];
				l[j] = t;
			}
		}

		/// <summary>
		/// Helper function that returns a random character from the given string.
		/// </summary>
		private static char GetRandCharFromString(string s, CryptoRandomStream crs)
		{
			if (s == null) { Debug.Assert(false); }
			int i = GetRandInt(s.Length, crs);
			return s[i];
		}
		/// <summary>
		/// Helper function that returns a random number between 0 and max-1.
		/// </summary>
		private static int GetRandInt(int max, CryptoRandomStream crs)
		{
			/* Returns a random number between 0 and max-1 */
			Debug.Assert(max != 0);
			ulong u = crs.GetRandomUInt64();
			u %= (ulong)max;
			return (int)u;
		}
		/// <summary>
		/// Helper function that returns a random number between min and max-1.
		/// </summary>
		private static int GetRandInt(int min, int max, CryptoRandomStream crs)
		{
			return min + GetRandInt(max-min, crs);
		}
		/// <summary>
		/// Helper function that returns a random element of the given list.
		/// </summary>
		private static T GetRandListElement<T>(List<T> l, CryptoRandomStream crs) 
		{
			Debug.Assert(l.Count != 0);
			return l[GetRandInt(l.Count, crs)];
		}

	}
}
