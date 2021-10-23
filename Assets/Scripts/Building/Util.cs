using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


static class Util
{
    static StringBuilder builder = new StringBuilder();
    public static string CombineString(params string[] value)
    {
        builder.Clear();

        for (int i = 0; i < value.Length; i++)
            builder.Append(value[i]);

        return builder.ToString();
    }
}

