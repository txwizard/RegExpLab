using System;
using System.Text.RegularExpressions;

using WizardWrx;

namespace ConsoleApp1
{
	class Program
	{
		const string TEST_INPUT_1 = @"Login ID mtoll@firedrum.com in tracking Domain ID";
		const string TEST_INPUT_2 = @"Login ID Stephanie@Say2Sell.com in tracking Domain ID 1062(domain Name = SalesRelevance) is not marked as a Free Trial Tracking Lead. ALL processess will be skipped.";
		const string TEST_INPUT_3 = @"Login ID SalesTalk4CL@SalesRelevance.com in tracking Domain ID 1062(domain Name = SalesRelevance) is not marked as a Free Trial Tracking Lead. ALL processess will be skipped.";

		static string [ ] s_BackReference_Test_Cases = new string [ ]
		{
			TEST_INPUT_1 ,
			TEST_INPUT_2 ,
			TEST_INPUT_3
		};

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

			try
			{
				Regex regex = new Regex (
					strRegExp , 
					RegexOptions.Multiline
					| RegexOptions.IgnorePatternWhitespace );
				MatchCollection matchCollection = regex.Matches ( strTestCases );

				int intMatchNumber = 0;

				foreach ( Match match in matchCollection )
				{
					for ( int intGroupIndex = ArrayInfo.ARRAY_FIRST_ELEMENT ;
							  intGroupIndex < match.Groups.Count ;
							  intGroupIndex++ )
					{
						if ( intGroupIndex == RegExpSupport.REGEXP_FIRST_MATCH )
						{
							Console.WriteLine (
								@"{3}Full match {0} of {1}: {2}{3}" ,
								new object [ ]
								{
								++intMatchNumber ,								// Format Item 0: Full match {0}
								match.Groups.Count ,							// Format Item 1: of {1}:
								match.Groups [ intGroupIndex ] ,				// Format Item 2: : {2}
								Environment.NewLine                             // Format Item 3: Platform-dependent newline
								} );
						}   // TRUE block, if ( intGroupIndex == RegExpSupport.REGEXP_FIRST_MATCH )
						else
						{
							Console.WriteLine (
								@"    Group {0}: {1}" ,
								intGroupIndex ,
								match.Groups [ intGroupIndex ] );
						}   // FALSE block, if ( intGroupIndex == RegExpSupport.REGEXP_FIRST_MATCH )
					}   // for ( int intGroupIndex = ARRAY_FIRST_ELEMENT ; intGroupIndex < match.Groups.Count ; intGroupIndex++ )
				}   // foreach ( Match match in matchCollection )

				PerformAnserTokenMatch ( );
				PerformTextExtractionViaBackReference ( );
			}
			catch ( Exception exAllKinds )
			{
				Console.WriteLine (
					@"{0}An {1} Exception arose. Details follow.{0}" ,			// Format Control String
					Environment.NewLine ,                                       // Format Item 0: {0}An  AND	follow.{0}
					exAllKinds.GetType ( ).FullName );                          // Format Item 1: An {1} Exception arose.
				Console.WriteLine ( $"    Message:    {exAllKinds.Message}" );
				Console.WriteLine ( $"    HResult:    {exAllKinds.HResult}" );
				Console.WriteLine ( $"    TargetSite: {exAllKinds.TargetSite}" );
				Console.WriteLine ( $"    Source:     {exAllKinds.Source}" );
				Console.WriteLine ( $"    Message:    {exAllKinds.StackTrace.Replace ( Environment.NewLine , $"{Environment.NewLine}                " )}" );
			}

			Console.WriteLine (
				@"{0}Press the RETURN key to exit the program." ,
				Environment.NewLine );
			Console.ReadLine ( );
		}   // static void Main


        private static void PerformAnserTokenMatch ( )
		{
			const string MATCH_EXPRESSION = @"$$answer$$";
			const string REPLACEMENT_STRING = @"Answer_1_Text";

			string strTestInput = "					<tr>" + Environment.NewLine
								  + "						<td style=\"width: 60%;\">" + Environment.NewLine
								  + "							<p class=\"FormLabel\">" + Environment.NewLine
								  + "								$$Question$$" + Environment.NewLine
								  + "							</p>" + Environment.NewLine
								  + "						</td>" + Environment.NewLine
								  + "						<td style=\"width: 60%;\">" + Environment.NewLine
								  + "                            < p class=\"FormInput\">" + Environment.NewLine
								  + "								<input name=\"$$Answer$$\" style=\"width: 200px\" type=\"text\" />" + Environment.NewLine
								  + "							</p>" + Environment.NewLine
								  + "						</td>" + Environment.NewLine
								  + "					</tr>" + Environment.NewLine;

			Console.WriteLine ( $"{Environment.NewLine}NVMethod PerformAnserTokenMatch Begin:{Environment.NewLine}" );

			Console.WriteLine ( $"    MATCH_EXPRESSION   = {MATCH_EXPRESSION}" );
			Console.WriteLine ( $"    strTestInput       = {strTestInput}" );
			Console.WriteLine ( $"    REPLACEMENT_STRING = {REPLACEMENT_STRING}" );
			Regex regex = new Regex (
				Regex.Escape ( MATCH_EXPRESSION ) ,
				RegexOptions.IgnoreCase
				| RegexOptions.Compiled );
			string strTestOutput = regex.Replace (
				strTestInput ,
				REPLACEMENT_STRING );
			Console.WriteLine ( $"    strTestOutput      = {strTestOutput}" );

			Console.WriteLine ( $"{Environment.NewLine}NVMethod PerformAnserTokenMatch Done!{Environment.NewLine}" );
		}   // private static void PerformAnserTokenMatch


		private static void PerformTextExtractionViaBackReference ( )
		{
			const string MATCH_EXPRESSION = @"Login ID (.*?) in tracking Domain ID";

			string strMyName = System.Reflection.MethodBase.GetCurrentMethod ( ).Name;
			Console.WriteLine (
				@"{0}Test Routine {1} for match expression {2} Begin:" ,        // Format Control String
				Environment.NewLine ,                                           // Format Item 0: {0}Test
				strMyName ,                                                     // Format Item 1: Routine {1}
				MATCH_EXPRESSION.QuoteString ( ) );                             // Format Item 2: for match expression {2}

			Regex regex = new Regex (
				MATCH_EXPRESSION ,
				RegexOptions.Compiled );

			for ( int intJ = ArrayInfo.ARRAY_FIRST_ELEMENT ; 
				      intJ < s_BackReference_Test_Cases.Length ;
					  intJ++ )
			{
				Console.WriteLine (
					@"{3}    Test Case {0} of {1}: Input  = {2}" ,				// Format Control String
					ArrayInfo.OrdinalFromIndex ( intJ ) ,                       // Format Item 0: Test Case {0}
					s_BackReference_Test_Cases.Length ,                         // Format Item 1: of {1}:
					s_BackReference_Test_Cases [ intJ ] ,                       // Format Item 2: Input = {2}
					Environment.NewLine );                                      // Format Item 3: Bookends the string

				MatchCollection matches = regex.Matches ( s_BackReference_Test_Cases [ intJ ] );
				string strMatchedText = matches [ RegExpSupport.REGEXP_FIRST_MATCH ].Groups [ RegExpSupport.REGEXP_FIRST_SUBMATCH ].Value;

				Console.WriteLine (
					@"                      Output = {0}" ,						// Format Control String
					strMatchedText );                                           // Format Item 0: Output = {0}"
			}   // for ( int intJ = ArrayInfo.ARRAY_FIRST_ELEMENT ; intJ < s_BackReference_Test_Cases.Length ; intJ++ )

			Console.WriteLine (
				@"{0}Test Routine {1} Done!" ,                                  // Format Control String
				Environment.NewLine ,											// Format Item 0: {0}Test
				strMyName );                                                    // Format Item 1: Routine {1}
		}   // private static void PerformTextExtractionViaBackReference
	}   // class Program
}   // partial namespace ConsoleApp1