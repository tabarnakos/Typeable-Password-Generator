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
			if(prf == null) { Debug.Assert(false); }
			else
			{
				Debug.Assert(prf.CustomAlgorithmUuid == Convert.ToBase64String(
					m_uuid.UuidBytes, Base64FormattingOptions.None));
			}

			// Starts with an upper-case letter
			ulong u = crsRandomSource.GetRandomUInt64();
			u %= (ulong)PwCharSet.UpperCase.Length;
			char chFirst = PwCharSet.UpperCase[(int)u];

			List<char> l = new List<char>();

			// Contains at least one digit
			u = crsRandomSource.GetRandomUInt64();
			u %= (ulong)PwCharSet.Digits.Length;
			l.Add(PwCharSet.Digits[(int)u]);

			// Contains at least one lower-case letter
			u = crsRandomSource.GetRandomUInt64();
			u %= (ulong)PwCharSet.LowerCase.Length;
			l.Add(PwCharSet.LowerCase[(int)u]);

			string strAlphaNumeric = PwCharSet.UpperCase +
				PwCharSet.LowerCase + PwCharSet.Digits;
			while(l.Count < 7)
			{
				u = crsRandomSource.GetRandomUInt64();
				u %= (ulong)strAlphaNumeric.Length;
				l.Add(strAlphaNumeric[(int)u]);
			}

			ShuffleList(l, crsRandomSource);

			return new ProtectedString(false, (new string(chFirst, 1)) +
				(new string(l.ToArray())));
		}

		private static void ShuffleList(List<char> l, CryptoRandomStream crs)
		{
			if(l == null) { Debug.Assert(false); return; }
			if(l.Count <= 1) return; // Nothing to shuffle

			for(int i = l.Count - 1; i >= 1; --i)
			{
				ulong u = crs.GetRandomUInt64();
				u %= (ulong)(i + 1);
				int j = (int)u;

				if(i == j) continue;

				char t = l[i];
				l[i] = l[j];
				l[j] = t;
			}
		}
	}
}
