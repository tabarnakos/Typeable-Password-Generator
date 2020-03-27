using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using KeePassLib.Utility;
using KeePassLib.Cryptography.PasswordGenerator;

namespace TypeablePasswordGenerator
{
    public class TypeablePasswordConfig
    {

		public List<string> l_charsets;
		public List<char> l_separators;
		public int i_password_length;
		public int i_min_sep_lenght;
		public int i_max_sep_lenght;

		/// <summary>
		/// Empty constructor, use default values
		/// </summary>
		public TypeablePasswordConfig()
		{
			l_charsets = new List<string>();
			l_separators = new List<char>();
			l_charsets.Add(PwCharSet.UpperCase);
			l_charsets.Add(PwCharSet.LowerCase);
			l_charsets.Add(PwCharSet.Digits);
			l_charsets.Add("#@$()&_=+-?!/%:'\" *");
			l_charsets.Add("£€¥¢©®™~¿[] {} <>^¡`;÷\\|¦¬×§¶°");

			l_separators.Add(' ');
			l_separators.Add('.');
			l_separators.Add('-');
			l_separators.Add('_');

			i_password_length = 50;
			i_min_sep_lenght = 2;
			i_max_sep_lenght = 3;
		}

		public static TypeablePasswordConfig FromFile(System.IO.Stream input_file)
		{
			return XmlUtilEx.Deserialize<TypeablePasswordConfig>(input_file);
		}

		public void ToFile(System.IO.Stream output_file)
		{
			XmlUtilEx.Serialize(output_file, this);
		}



	}
}
