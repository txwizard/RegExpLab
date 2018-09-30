using System;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
	class Program
	{
		static void Main ( string [ ] args )
		{
			const String strRegExp = "^\\s*\n"
								     + "([a-zA-Z_]\\w+)                                 # function name\n"
									 + "\\s*\n"
									 + "(?:\\(|\\G)\n"
									 + "(\\s*([a-zA-Z_]\\w+)\\s+([a-zA-Z_]\\w+),?)*    # args\n"
									 + "\\)\n"
									 + "\\s*$";

			const string strTestCases = " _validName__ ( _TypeName _variable)\n"
								        + " _validName__ ( _TypeName1 _variable1 _TypeName2 _variable2)\n"
								        + " 7invalidName (_TypeName variable)\n"
										+ "badName_ (_pp8p_7 _s5de)\n"
										+ " Valid4Name_()\n"
										+ " validName(7BadType variable)\n"
										+ " validName_(InvalidParam)\n"
										+ " validName(Type1 arg1, Type2 arg2)";

			Regex regex = new Regex ( strRegExp , RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace );
			MatchCollection matchCollection = regex.Matches ( strTestCases );

			foreach ( Match match in matchCollection )
			{
				for ( int intGroupIndex = 0 ; intGroupIndex < match.Groups.Count ; intGroupIndex++ )
				{
					if ( intGroupIndex == 0 )
					{
						Console.WriteLine (
							@"(1)Full match: {0}" ,
							match.Groups [ intGroupIndex ] ,
							intGroupIndex == 0 ? string.Empty : Environment.NewLine );
					}
					else
					{
						Console.WriteLine (
							@"    Group {0}: {1}" ,
							intGroupIndex ,
							match.Groups [ intGroupIndex ] );
					}   // for (int i = 1; i <= matcher.groupCount(); i++) {
				}   // for ( int intGroupIndex = 0 ; intGroupIndex < match.Groups.Count ; intGroupIndex++ )
			}   // foreach ( Match match in matchCollection )

			Console.WriteLine ( @"Press the RETURN key to exit the program." );
			Console.ReadLine ( );
		}   // static void Main
	}   // class Program
}   // partial namespace ConsoleApp1