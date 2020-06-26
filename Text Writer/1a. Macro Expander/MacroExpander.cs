using System.Collections.Generic;
using System.Text;

namespace Clouds.Strings {
	/// <summary>
	/// Methods to expand macros found in strings.
	/// </summary>
	public static class MacroExpander {
		/// <summary>
		/// Attempts to replace the invocation key of a macro with the string that macro represents.
		/// </summary>
		/// <param name="macroKey"></param>
		/// <param name="macroEvalTable"></param>
		/// <returns>The expanded macro, if the key is in the dictionary; the unaltered key otherwise.</returns>
		static string Expand (string macroKey, in Dictionary<string, string> macroEvalTable) {
			string returner;

			//Attempt to dig the key out of the dictionary.
			if (!macroEvalTable.TryGetValue(macroKey, out returner) ) {
				//If the key is not found, return the plain, unexpanded key instead.
				returner = macroKey;
			}

			return returner;
		}

		/// <summary>
		/// Evaluates a string at a position and recursively expands any macro keys found.
		/// </summary>
		/// <param name="containingString">The string which might contain a macro.</param>
		/// <param name="posInString">The position at which you'd like to check the string.</param>
		/// <param name="macroTable">A dictionary of macro keys/expansions to be expanded.</param>
		/// <param name="macroInvoker">The character which will invoke the expansion of the following text.</param>
		/// <returns>If the string is a macro key, the result of expanding the macro and all macros in it; an empty string otherwise.</returns>
		public static string EvaluateMacros (
			string containingString,
			//uint posInString,
			in Dictionary<string, string> macroTable,
			char macroInvoker
		) {
			//VALIDATE: Is the macro table existent?
			if (macroTable == null) {
				//If not, abort and return unmodified.
				return containingString;
			}

			//VALIDATE: Is the string long enough for a macro invoker and >= 1 character?
			if (containingString.Length < 2) {
				//If not, abort and return unmodified.
				return containingString;
			}

			//This list stores evaluated keys to prevent infinitely-recursing macro expansions.
			List<string> alreadyEvaluated = new List<string>();

			//Perform the actual evaluation.
			return EvaluateMacrosInternal(containingString, macroTable, macroInvoker, ref alreadyEvaluated);
		}

		private static string EvaluateMacrosInternal (
			string containingString,
			//uint posInString,
			in Dictionary<string, string> macroTable,
			char macroInvoker,
			ref List<string> alreadyEvaluated
		) {
			//If the string contains no macro-invocation characters, return without logicking.
			if (!containingString.Contains(macroInvoker.ToString())) {
				return containingString;
			}

			//For each key in the macro table:
			foreach (string key in macroTable.Keys) {
				//Check if this key's been evaluated in a previous iteration.
				if (alreadyEvaluated.Contains(key)) {
					//If it has, throw an error message.
					UnityEngine.Debug.LogError(
						"Macro \"" + key + "\" expands to contain itself."
						//@TODO: Exact location of offending invocation would be great, but I can't see that happening....
					);

					//Continue to next macro that needs evaluating, leaving this one non-expanded.
					continue;
				}

				//The key has not been evaluated on this level of recursion. Prevent lower levels from doing it.
				alreadyEvaluated.Add(key);
				
				//replace instances of invoker + key with the recursively unfolded macro.
				containingString = containingString.Replace(
					macroInvoker + key,
					EvaluateMacrosInternal(macroTable[key], in macroTable, macroInvoker, ref alreadyEvaluated)
				);

				//Now we can forget this key, as it's safe to execute within a different macro.
				alreadyEvaluated.Remove(key); //Efficiency? It's at the end, so no, it's not a problem.
			}

			//Return the fully-evaluated result.
			return containingString;
		}
		
	}
}