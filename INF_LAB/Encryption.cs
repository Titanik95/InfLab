using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF_LAB
{
    class Encryption
    {
        public static string EncriptSubstitution(string str)
        {

            Dictionary<string, string> table = GenerateTable("012", 2);
            return "test";
        }

        static Dictionary<string, string> GenerateTable(string alpabet, int m)
        {
            // Генерация всех возможных перестановок с повторениями
            List<string> permutations = new List<string>();
            int[] permIndex = new int[m];
            int i;
            StringBuilder sb = new StringBuilder();
            sb.Clear();
            for (int j = 0; j < m; j++)
            {
                sb.Append(alpabet[permIndex[j]]);
            }
            permutations.Add(sb.ToString());
            do
            {
                i = -1;
                do
                {
                    i++;
                    permIndex[i] = (permIndex[i] + 1) % m;
                }
                while (permIndex[i] == 0 && i != m);
                sb.Clear();
                for (int j = m - 1; j >= 0 ; j--)
                {
                    sb.Append(alpabet[permIndex[j]]);
                }
                permutations.Add(sb.ToString());
            } while (i != m);

            using (StreamWriter sw = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\test.txt"))
            {
                foreach (var el in permutations)
                    sw.WriteLine(el);
            }
                var table = new Dictionary<string, string>();
            return table;
        }
    }
}
