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

using KeePass.Plugins;

namespace TypeablePasswordGenerator
{
	public sealed class TypeablePasswordGeneratorExt : Plugin
	{
		private IPluginHost m_host = null;
		private TypeablePasswordGenerator m_gen = null;

		public override bool Initialize(IPluginHost host)
		{
			Terminate();

			if(host == null) return false;

			m_host = host;

			m_gen = new TypeablePasswordGenerator();
			m_host.PwGeneratorPool.Add(m_gen);

			return true;
		}

		public override void Terminate()
		{
			if(m_host == null) return;

			m_host.PwGeneratorPool.Remove(m_gen.Uuid);

			m_gen = null;
			m_host = null;
		}
	}
}
