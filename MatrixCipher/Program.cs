using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace MatrixCipher
{
    class Program
    {

        
        static void Main(string[] args)
        {
            Console.Write("Enter plain text: ");
            string plainText = Console.ReadLine();
            Console.Write("\nEnter one key word: ");
            string key = Console.ReadLine();
            var sentanceAssemblyOrder = GetAssemblyOrderUsingKey(key);
            var reorderedKey = new String(key.ToLower().OrderBy(key => key).ToArray());
            var matrix = SplitByKey(key, plainText.Replace(" ", ""));
            var reorderedMatrix = ReorderMatrix(matrix, reorderedKey, sentanceAssemblyOrder);
            Console.WriteLine(GetStringFromMatrix(reorderedMatrix));
        }

        public static List<int> GetAssemblyOrderUsingKey(string originalKey)
        {
            var orderedKey = originalKey.ToLower().OrderBy(key => key);
            var cipherTextAssemblyOrder = new List<int>();
            var numericalKeyTupleList = new List<ValueTuple<int, char>>();
            for (int z = 0; z < originalKey.Length; z++) {
                var keyCharIndex = (Index: z, Value: orderedKey.ElementAt(z)); // A tuple containing a a char from the alphabetised key and it's associated value
                numericalKeyTupleList.Add(keyCharIndex);
            }
            var alphaKeyTupleList = new List<ValueTuple<int, char>>();
            foreach (char c in originalKey.ToLower()) {
                for (int i = 0; i < numericalKeyTupleList.Count; i++) {
                    if (c == numericalKeyTupleList[i].Item2) {
                        alphaKeyTupleList.Add((numericalKeyTupleList[i].Item1, c));
                        break;
                    }
                }
            }
            foreach (var item in alphaKeyTupleList) {
                cipherTextAssemblyOrder.Add(item.Item1);
            }
            return cipherTextAssemblyOrder;
        }

        public static List<List<char>> SplitByKey(string key, string plainText)
        {
            var fullMatrix = new List<List<char>>();
            int i = 0;
            foreach(var c in key) {
                var matrixColumn = new List<char>();
                int j = 0;
                foreach (var l in plainText) {
                    if(j % key.Length == i) {
                        matrixColumn.Add(plainText[j]);
                    }
                    j++;
                }
                fullMatrix.Add(matrixColumn);
                i++;
            }
            return fullMatrix;            
        }
        public static List<ValueTuple<int, char, List<char>>> ReorderMatrix(List<List<char>> matrix, string orderedKey, List<int> assemblyOrder)
        {
            var indexedMatrix = new List<ValueTuple<int, char, List<char>>>();
            for (int i = 0; i < assemblyOrder.Count; i++) {
                int num = assemblyOrder[i];
                char ch = orderedKey[i];
                List<char> list = matrix[i];
                indexedMatrix.Add((num, ch, list));
            }
            var orderedMatrix = new List<ValueTuple<int, char, List<char>>>(indexedMatrix.OrderBy(a => a.Item1));
            return orderedMatrix;
        }

        public static string GetStringFromMatrix(List<(int, char, List<char>)> matrix)
        {
            int longestColumnLength = 0;
            foreach (var tup in matrix) {
                if (tup.Item3.Count > longestColumnLength) { 
                    longestColumnLength = tup.Item3.Count;
                }
            }
            int i = 0;
            var encryptedText = new StringBuilder();
            while (i < longestColumnLength) {
                foreach (var column in matrix) {
                    if (i < (column.Item3.Count)) {
                        encryptedText.Append(column.Item3[i]);
                    }
                }
                i++;
            }            
            return encryptedText.ToString();
        }
    }
}
