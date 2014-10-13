using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hjson;

namespace Test
{
  class Program
  {
    static int Main(string[] args)
    {
      var asset=Path.GetFullPath(@"..\..\asset");
      foreach (var file in Directory.GetFiles(asset, "*_test.hjson"))
      {
        string name=Path.GetFileNameWithoutExtension(file);
        name=name.Substring(0, name.Length-5);

        var text=File.ReadAllText(file).Replace("\r\n", "\n"); ;
        var result=JsonValue.Load(Path.Combine(asset, name+"_result.json")).Qo();

        try
        {
          var data=HjsonValue.Parse(text);
          var data1=data.SaveAsString(true);
          var data2=result["data"].SaveAsString(true);
          var hjson1=HjsonValue.SaveAsString(data).Replace("\r\n", "\n");
          var hjson2=File.ReadAllText(Path.Combine(asset, name+"_result.txt")).Replace("\r\n", "\n"); ;

          if (data1!=data2) return showErr(name+" parse", data1, data2);
          else if (hjson1!=hjson2) return showErr(name+" stringify", hjson1, hjson2);
          else Console.WriteLine(name+" SUCCESS");
        }
        catch (Exception e)
        {
          if (result.ContainsKey("err")) Console.WriteLine(name+" SUCCESS");
          else return showErr(name+" exception", e.ToString(), "");
        }
      }
      return 0;
    }

    static int showErr(string cap, string s1, string s2)
    {
      Console.WriteLine(cap+" FAILED!");
      Console.WriteLine("--- actual:");
      Console.WriteLine(s1);
      Console.WriteLine("--- expected:");
      Console.WriteLine(s2);
      return 1;
    }
  }
}