using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Program
    {
        public static Dictionary<string, dynamic> ReadJSON(string path)
        {
            string s1 = File.ReadAllText(path);
            string app = s1.Split('[', ']')[1];
            Boolean hascon = true;
            var map = new Dictionary<string, dynamic>();
            int count = 1;
            string result = "";
            while (hascon)
            {
                try
                {
                    string eachtag = s1.Split('{', '}')[count];
                    string keyFinal = "";
                    string valueFinal = "";
                    try
                    {
                        string[] eachValues = eachtag.Split(',');
                        string key = eachValues[0];
                        string value = eachValues[1];
                        string[] keySplit = key.Split(':');
                        string[] valueSplit = value.Split(':');
                        keyFinal = keySplit[1].Replace("\"", "");
                        valueFinal = valueSplit[1].Replace("\"", "");

                        map.Add(keyFinal.Replace(" ", string.Empty), valueFinal);

                        count++;
                    }
                    catch (ArgumentException e)
                    {
                        map[keyFinal.Replace(" ", string.Empty)] = valueFinal;
                        count++;
                    }
                    catch (Exception e)
                    {
                        count++;
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    hascon = false;
                }
            }
            return map;
        }
        public static String RunTest()
        {
            var jsonDatas = Program.ReadJSON(@"C:\Users\mm\Documents\Visual Studio 2017\Projects\WebApplication1\WebApplication1\aaa.json");
            Data data = new Data();
            foreach (var json in jsonDatas) {
                data.addActionList(json.Key, json.Value);
            }
            var result = data.PerformXML();
            return result;
        }
    }
}