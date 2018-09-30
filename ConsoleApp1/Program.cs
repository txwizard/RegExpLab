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

			int intMatchNumber = 0;

			foreach ( Match match in matchCollection )
			{
				for ( int intGroupIndex = 0 ; intGroupIndex < match.Groups.Count ; intGroupIndex++ )
				{
					if ( intGroupIndex == 0 )
					{
						Console.WriteLine (
							@"{3}Full match {0} of {1}: {2}{3}" ,
							new object [ ]
							{
								++intMatchNumber ,								// Format Item 0: Full match {0}
								match.Groups.Count ,							// Format Item 1: of {1}:
								match.Groups [ intGroupIndex ] ,				// Format Item 2: : {2}
								Environment.NewLine								// Format Item 3: Platform-dependent newline
							} );
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

			Console.WriteLine (
				@"{0}Press the RETURN key to exit the program." ,
				Environment.NewLine );
			Console.ReadLine ( );
		}   // static void Main
	}   // class Program
}   // partial namespace ConsoleApp1