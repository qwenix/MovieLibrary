using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebLibrary.Models {
    public static class StringRevision {
        public const string INPUT_REG_EXP = @"[\s\S]*[a-zA-Zа-яА-яії]+[\s\S]*";

        public static string RemoveUnnecessarySpaces(string s) {
            if (s != null) {
                s = s.Trim();
                bool flag = true;
                StringBuilder sb = new StringBuilder();

                foreach (char c in s) {
                    if (flag) {
                        sb.Append(c);
                        flag = c == ' ' ? false : true;
                    }
                    else if (c != ' ') {
                        sb.Append(c);
                        flag = true;
                    }
                }

                return sb.ToString();
            }
            return s;
        }
    }
}
