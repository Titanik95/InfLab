using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace INF_LAB
{
    class Encryption
    {
        public static string EncryptSubstitution(string str, string alphabet, int m)
        {
            if (!CheckString(str, alphabet))
                return "Недопустимые символы во входной строке";
            Dictionary<string, string> table = GenerateTable(alphabet, m);
            while (str.Length % m != 0)
                str += alphabet[0];
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i += m)
                sb.Append(table[str.Substring(i, m)]);

            SaveTable(table);
            return sb.ToString();
        }

		public static string EncryptTransposition(string str, string alphabet, int m)
		{
			if (!CheckString(str, alphabet))
				return "Недопустимые символы во входной строке";
			Dictionary<int, int> table = GenerateTableTransposition(m);
			while (str.Length % m != 0)
				str += alphabet[0];
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < str.Length; i += m)
			{
				for (int j = 0; j < m; j++)
					sb.Append(str[i + table[j]]);
			}

			SaveTableTransposition(table);
			return sb.ToString();
		}

		public static List<string> EncryptLinear(string str, GammaParameters gp)
		{
			var byteArr = Encoding.ASCII.GetBytes(str.ToArray());
			foreach (var b in byteArr)
				if (b >= gp.B)
					return new List<string>() { "Недопустимые символы во входной строке", ""};
			List<byte> gammaElements = GenerateGammaElements(gp, str.Length);

			List<byte> resArr = new List<byte>();
			for (int i = 0; i < str.Length; i++)
			{
				int num = (byteArr[i] + gammaElements[i]) % gp.B;
				resArr.Add((byte)num);
			}

			List<string> res = new List<string>();
			StringBuilder sb = new StringBuilder();
			foreach (var num in resArr)
			{
				sb.Append(num);
				sb.Append(" ");
			}
			res.Add(sb.ToString());

			sb.Clear();
			foreach (var num in gammaElements)
			{
				sb.Append(num);
				sb.Append(" ");
			}
			res.Add(sb.ToString());

			SaveGammaParameters(gp);
			return res;
		}


		public static string DecryptSubstitution(string str, string keyFilePath, string alphabet, int m)
        {
            var table = GetTable(keyFilePath);
            if (table == null)
                return "Неправильный ключ";
            if (!CheckString(str, alphabet))
                return "Недопустимые символы во входной строке";

            Dictionary<string, string> reverseTable = new Dictionary<string, string>();
            foreach (var kv in table)
                reverseTable.Add(kv.Value, kv.Key);

            while (str.Length % m != 0)
                str += alphabet[0];
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < str.Length; i += m)
                sb.Append(reverseTable[str.Substring(i, m)]);

            return sb.ToString();
        }

		public static string DecryptTransposition(string str, string keyFilePath, string alphabet, int m)
		{
			var table = GetTableTransposition(keyFilePath);
			if (table == null)
				return "Неправильный ключ";
			if (!CheckString(str, alphabet))
				return "Недопустимые символы во входной строке";

			var reverseTable = new Dictionary<int, int>();
			foreach (var kv in table)
				reverseTable.Add(kv.Value, kv.Key);

			while (str.Length % m != 0)
				str += alphabet[0];
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < str.Length; i += m)
				for (int j = 0; j < m; j++)
					sb.Append(str[i + reverseTable[j]]);

			return sb.ToString();
		}

		public static string DecryptLinear(string str, string keyFilePath)
		{
			List<byte> byteArr = new List<byte>();
			foreach (var num in str.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
			{
				byte res;
				if (byte.TryParse(num, out res))
					byteArr.Add(res);
				else
					return "Недопустимые символы во входной строке";
			}
			var gp = GetGammaParameters(keyFilePath);
			if (gp == null)
				return "Неверный ключ";
			List<byte> gammaElements = GenerateGammaElements(gp, byteArr.Count);

			List<byte> resArr = new List<byte>();
			for (int i = 0; i < byteArr.Count; i++)
			{
				int num = (byteArr[i] - gammaElements[i] + gp.B) % gp.B;
				resArr.Add((byte)num);
			}

			return new string(Encoding.ASCII.GetChars(resArr.ToArray()));
		}

        static bool CheckString(string str, string alphabet)
        {
            foreach (var sb in str)
            {
                bool exist = false;
                foreach (var let in alphabet)
                {
                    if (sb == let)
                    {
                        exist = true;
                        break;
                    }
                }
                if (!exist)
                    return false;
            }
            return true;
        }

        static Dictionary<string, string> GetTable(string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    BinaryFormatter bf = new BinaryFormatter();

                    return (Dictionary<string, string>)bf.Deserialize(fs);
                }
            }
            catch
            {
                return null;
            }
        }

		static Dictionary<int, int> GetTableTransposition(string filePath)
		{
			try
			{
				using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
				{
					BinaryFormatter bf = new BinaryFormatter();

					return (Dictionary<int, int>)bf.Deserialize(fs);
				}
			}
			catch
			{
				return null;
			}
		}

		static GammaParameters GetGammaParameters(string filePath)
		{
			try
			{
				using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
				{
					BinaryFormatter bf = new BinaryFormatter();

					return (GammaParameters)bf.Deserialize(fs);
				}
			}
			catch
			{
				return null;
			}
		}

		static void SaveTable(Dictionary<string, string> table)
        {
            using (FileStream fs = new FileStream(Environment.CurrentDirectory + "\\key", FileMode.OpenOrCreate))
            {
                BinaryFormatter bf = new BinaryFormatter();

                try
                {
                    bf.Serialize(fs, table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Невозможно сохранить ключ", ex.Message);
                }
            }
        }

		static void SaveTableTransposition(Dictionary<int, int> table)
		{
			using (FileStream fs = new FileStream(Environment.CurrentDirectory + "\\key", FileMode.OpenOrCreate))
			{
				BinaryFormatter bf = new BinaryFormatter();

				try
				{
					bf.Serialize(fs, table);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Невозможно сохранить ключ", ex.Message);
				}
			}
		}

		static void SaveGammaParameters(GammaParameters gp)
		{
			using (FileStream fs = new FileStream(Environment.CurrentDirectory + "\\key", FileMode.OpenOrCreate))
			{
				BinaryFormatter bf = new BinaryFormatter();

				try
				{
					bf.Serialize(fs, gp);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Невозможно сохранить ключ", ex.Message);
				}
			}
		}

		static Dictionary<string, string> GenerateTable(string alpabet, int m)
        {
            // Генерация всех возможных перестановок с повторениями
            List<string> permutations = new List<string>();
            int[] permIndex = new int[m];
            int i;
            StringBuilder sb = new StringBuilder();
            do
            {
                sb.Clear();
                for (int j = m - 1; j >= 0 ; j--)
                {
                    sb.Append(alpabet[permIndex[j]]);
                }
                permutations.Add(sb.ToString());

                i = -1;
                do
                {
                    if (++i == m) break;
                    permIndex[i] = (permIndex[i] + 1) % m;
                }
                while (permIndex[i] == 0);
            } while (i != m);

            
            var table = new Dictionary<string, string>();
            List<int> keys = new List<int>();
            List<int> values = new List<int>();
            for (int n = 0; n < permutations.Count; n++)
            {
                keys.Add(n);
                values.Add(n);
            }

            Random rnd = new Random();
            for (int n = permutations.Count - 1; n >= 0; n--)
            {
                int key = rnd.Next(n);
                int value = rnd.Next(n);
                table.Add(permutations[keys[key]], permutations[values[value]]);
                keys.RemoveAt(key);
                values.RemoveAt(value);
            }

            return table;
        }

		static Dictionary<int, int> GenerateTableTransposition(int m)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < m; i++)
				list.Add(i);
			Random rand = new Random();
			Dictionary<int, int> table = new Dictionary<int, int>();
			for (int i = m - 1; i >= 0; --i)
			{
				int ind = rand.Next(i);
				table.Add(i, list[ind]);
				list.RemoveAt(ind);
			}

			return table;
		}

		static List<byte> GenerateGammaElements(GammaParameters gp, int n)
		{
			var res = new List<byte>();
			res.Add((byte)((gp.A * gp.T0 + gp.C) % gp.B));
			for (int i = 1; i < n; i++)
				res.Add((byte)((gp.A * res[i - 1] + gp.C) % gp.B));

			return res;
		}

	}
}
