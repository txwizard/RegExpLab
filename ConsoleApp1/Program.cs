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
			const string CMDARG_DO_BASIC_MATCHING = @"Basic_Matching";
			const string CMDARG_MATCH_ANSWER_TEXT = @"Match_Answer_Text";
			const string CMDARG_EXTRACT_VIA_BACKREF = @"Extract_Via_BackRef";
			const string CMDARG_PARSE_USERNOTES = @"Parse_UserNote";

			try
			{
				if ( args.Length == MagicNumbers.ZERO )
				{
					DoBasicRegExpOperations ( );
					PerformAnserTokenMatch ( );
					PerformTextExtractionViaBackReference ( );
				}   // TRUE (degenerate case) block, if ( args.Length == MagicNumbers.ZERO )
				else
				{
					switch ( args [ ArrayInfo.ARRAY_FIRST_ELEMENT ] )
					{
						case CMDARG_DO_BASIC_MATCHING:
							DoBasicRegExpOperations ( );
							break;
						case CMDARG_MATCH_ANSWER_TEXT:
							PerformAnserTokenMatch ( );
							break;
						case CMDARG_EXTRACT_VIA_BACKREF:
							PerformTextExtractionViaBackReference ( );
							break;
						case CMDARG_PARSE_USERNOTES:
							PerformUserNoteParsing ( );
							break;
					}   // switch ( args [ ArrayInfo.ARRAY_FIRST_ELEMENT ] )
				}   // FALSE (standard case) block, if ( args.Length == MagicNumbers.ZERO )
			}
			catch ( Exception exAllKinds )
			{
				Console.WriteLine (
					@"{0}An {1} Exception arose. Details follow.{0}" ,          // Format Control String
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


		private static void DoBasicRegExpOperations ( )
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
		}


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
					@"{3}    Test Case {0} of {1}: Input  = {2}" ,              // Format Control String
					ArrayInfo.OrdinalFromIndex ( intJ ) ,                       // Format Item 0: Test Case {0}
					s_BackReference_Test_Cases.Length ,                         // Format Item 1: of {1}:
					s_BackReference_Test_Cases [ intJ ] ,                       // Format Item 2: Input = {2}
					Environment.NewLine );                                      // Format Item 3: Bookends the string

				MatchCollection matches = regex.Matches ( s_BackReference_Test_Cases [ intJ ] );
				string strMatchedText = matches [ RegExpSupport.REGEXP_FIRST_MATCH ].Groups [ RegExpSupport.REGEXP_FIRST_SUBMATCH ].Value;

				Console.WriteLine (
					@"                      Output = {0}" ,                     // Format Control String
					strMatchedText );                                           // Format Item 0: Output = {0}"
			}   // for ( int intJ = ArrayInfo.ARRAY_FIRST_ELEMENT ; intJ < s_BackReference_Test_Cases.Length ; intJ++ )

			Console.WriteLine (
				@"{0}Test Routine {1} Done!" ,                                  // Format Control String
				Environment.NewLine ,                                           // Format Item 0: {0}Test
				strMyName );                                                    // Format Item 1: Routine {1}
		}   // private static void PerformTextExtractionViaBackReference


		private static void PerformUserNoteParsing ( )
		{
			string strAllUserNotes = System.IO.File.ReadAllText ( @"D:\Source_Code\Visual_Studio\Projects\SalesTalk\ImportEngine1\NOTES\UserNote_Sample.TXT" );

			Console.WriteLine ( @"Input File Name            = D:\Source_Code\Visual_Studio\Projects\SalesTalk\ImportEngine1\NOTES\UserNote_Sample.TXT{0}" , Environment.NewLine );
			Console.WriteLine ( $"Input Text Character Count = {strAllUserNotes.Length}" );
			Console.WriteLine ( $"Nonbreaking Space Count    = {strAllUserNotes.CountCharacterOccurrences ( SpecialCharacters.NONBREAKING_SPACE_CHAR )}{Environment.NewLine}" );

			// Match something like ~04/07/2022 22:28:

			Regex regex = new Regex ( @"~(\d\d\/\d\d\/\d\d\d\d \d\d:\d\d): " , RegexOptions.Singleline );
			MatchCollection matchCollection = regex.Matches ( strAllUserNotes );

			for ( int intJ = ArrayInfo.ARRAY_FIRST_ELEMENT ;
					  intJ < matchCollection.Count ;
					  intJ++ )
			{
				string strInBetween = ExtractTextBetweenMatches ( matchCollection , intJ , strAllUserNotes );
				string strUserNate = TrimTrailingNBSP ( strInBetween );
				string strNoteDate = matchCollection [ intJ ].Groups [ RegExpSupport.REGEXP_FIRST_SUBMATCH ].Value;

				Console.WriteLine ( $"Match # {ArrayInfo.OrdinalFromIndex ( intJ ),2:N0}: {matchCollection [ intJ ]}" );
				Console.WriteLine ( $"            Position (Index) where match found = {matchCollection [ intJ ].Index}" );
				Console.WriteLine ( $"            Length of matched text             = {matchCollection [ intJ ].Length}" );
				Console.WriteLine ( $"            Group 0, the whole match           = {matchCollection [ intJ ].Groups [ RegExpSupport.REGEXP_FIRST_MATCH ].Value}" );
				Console.WriteLine ( $"            Group 1, the first submatch        = {strNoteDate}" );
				Console.WriteLine ( $"            Text between end of match and next = {strInBetween}" );
				Console.WriteLine ( $"            Text between end of match and next = {strUserNate}" );

				if ( DateTime.TryParseExact ( strNoteDate , @"MM/dd/yyyy HH:mm" , System.Globalization.CultureInfo.CurrentUICulture , System.Globalization.DateTimeStyles.None , out DateTime dtmLocalMountainTZTimeStamp ) )
				{
					Console.WriteLine ( $"            Note date {strNoteDate} parsed into {dtmLocalMountainTZTimeStamp.ToString ( @"MM/dd/yyyy HH:mm:ss" )} Mountan Time.{Environment.NewLine}" );
				}
				else
				{
					Console.WriteLine ( $"            Note date {strNoteDate} cannot be parsed under the specified conditions.{Environment.NewLine}" );
				}
		}   // for ( int intJ = ArrayInfo.ARRAY_FIRST_ELEMENT ; intJ < matchCollection.Count ; intJ++ )
	}   // private static void PerformUserNoteParsing


		/// <summary>
		/// Given the System.Text.RegularExpression.Match at index <paramref name="pintMatchIndex"/>
		/// in System.Text.RegularExpression.MatchCollection <paramref name="prxpMatchCollection"/>,
		/// return the substring that follows the matching text in string <paramref name="pstrInputString"/>
		/// up to the beginning of the next match, or the rest of the string inthe case of the last match.
		/// </summary>
		/// <param name="prxpMatchCollection">
		/// Pass in a reference to the System.Text.RegularExpression.MatchCollection attached to a
		/// System.Text.RegularExpression.Regex that has one or more matches.
		/// </param>
		/// <param name="pintMatchIndex">
		/// Pass in an integer that represents the index of the Match in <paramref name="prxpMatchCollection"/>
		/// for which to return the text that follows the text that matched it.
		/// This integer must be greater than or equal to zero and less than the
		/// Count property on MatchCollection <paramref name="prxpMatchCollection"/>.
		/// </param>
		/// <param name="pstrInputString">
		/// Pass in a reference to the string that was fed into the <paramref name="prxpMatchCollection"/>
		/// constructor. This value cnnot be NULL or the empty string. It is the
		/// caller's responsiblity to pass in the same string that was passed
		/// into the constructor that created the <paramref name="prxpMatchCollection"/>
		/// because this function makes no efrort to use Reflection to test it.
		/// </param>
		/// <returns>
		/// There are two possible return values:
		/// <list type="number">
		/// <item>
		/// When <paramref name="pintMatchIndex"/> is the index of the the last
		/// Match in <paramref name="prxpMatchCollection"/>, the return value is
		/// a substring comprising the text that begins immediately after the
		/// last character of the Value of the Match at index
		/// <paramref name="pintMatchIndex"/> in the collection and continues to
		/// the end of string <paramref name="pstrInputString"/>.
		/// </item>
		/// <item>
		/// In all other cases, the return value is a substring comprising the
		/// text that begins immediately after the last character of the Value
		/// of the Match at index <paramref name="pintMatchIndex"/> in the
		/// collection and continues up to the character at the position that
		/// corresponds to the Index of the next Match in the collection.
		/// </item>
		/// </list>
		/// </returns>
		private static string ExtractTextBetweenMatches (
			MatchCollection prxpMatchCollection ,
			int pintMatchIndex ,
			string pstrInputString )
		{
			Match rxpCurrentMatch = prxpMatchCollection [ pintMatchIndex ];
			int intPosFollowingText = rxpCurrentMatch.Index + rxpCurrentMatch.Length;

			if ( pintMatchIndex + ArrayInfo.ARRAY_NEXT_INDEX < prxpMatchCollection.Count )
			{
				Match rxpNextMatch = prxpMatchCollection [ pintMatchIndex + ArrayInfo.ARRAY_NEXT_INDEX ];
				return pstrInputString.Substring ( intPosFollowingText , rxpNextMatch.Index - intPosFollowingText );
			}   // TRUE (More matches follow the match specified by the index.) block, if ( intJ + ArrayInfo.ARRAY_NEXT_INDEX < matchCollection.Count )
			else
			{
				return pstrInputString.Substring ( intPosFollowingText );
			}   // FALSE (The index specifies the last match) block, if ( intJ + ArrayInfo.ARRAY_NEXT_INDEX < matchCollection.Count )
		}   // private static string ExtractTextBetweenMatches


		/// <summary>
		/// When the last character of string <paramref name="pstrStringToTrim"/>
		/// is a nonbreaking space, remove it. Otherwise, return a copy of the
		/// input string.
		/// </summary>
		/// <param name="pstrStringToTrim">
		/// <para>
		/// This string is anticipated to end with an unwanted nonbreaking
		/// space.
		/// </para>
		/// <para>
		/// The empty string is a degenerate case, which returns another empty
		/// string.
		/// </para>
		/// <para>
		/// A null reference returns another null reference. Give me nothing and
		/// I return nothing.
		/// </para>
		/// </param>
		/// <returns>
		/// The return value is the input string, minus its last character when
		/// the last character is a nonbreaking space. Otherwise, the return
		/// value is a copy of the original string.
		/// </returns>
		private static string TrimTrailingNBSP ( string pstrStringToTrim )
		{
			if ( pstrStringToTrim == null )
			{
				return null;
			}   // TRUE (The input is a null reference.) block, if ( pstrStringToTrim == null )
			else
			{
				if ( string.IsNullOrEmpty ( pstrStringToTrim ) )
				{
					return SpecialStrings.EMPTY_STRING;
				}   // TRUE (The string is empty, since the previous evaluation eliminated null references.) block, if ( string.IsNullOrEmpty ( pstrStringToTrim ) )
				else
				{
					if ( pstrStringToTrim.EndsWith ( SpecialStrings.NONBREAKING_SPACE_CHAR ) )
					{
						return pstrStringToTrim.Substring (
							ListInfo.BEGINNING_OF_BUFFER ,
							pstrStringToTrim.Length - MagicNumbers.PLUS_ONE );
					}   // TRUE (The last chareacter in the input string is a nonbreaking space.) block, if ( pstrStringToTrim.EndsWith ( SpecialStrings.NONBREAKING_SPACE_CHAR ) )
					else
					{
						return pstrStringToTrim;
					}   // FALSE (The last character in the input string is something besides a nonbreaking space.) block, if ( pstrStringToTrim.EndsWith ( SpecialStrings.NONBREAKING_SPACE_CHAR ) )
				}   // FALSE (The string is neither a null reference, nor the empty string.) block, if ( string.IsNullOrEmpty ( pstrStringToTrim ) )}
			}   // FALSE (The input points to a string of some kind.) block, if ( pstrStringToTrim == null )
		}   // private static string TrimTrailingNBSP
	}   // class Program
}   // partial namespace ConsoleApp1