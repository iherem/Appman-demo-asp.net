using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    public class Data
    {
        public static String ReadJson(string path) {
            string s1 = File.ReadAllText(path);
            string app = s1.Split('[', ']')[1];
            Boolean hascon = true;
            var map = new Dictionary<string, string>();
            int count = 1;
            string result = "";
            while (hascon)
            {
                try
                {
                    string eachtag = s1.Split('{', '}')[count];
                    try
                    {
                        string[] eachValues = eachtag.Split(',');
                        string key = eachValues[0];
                        string value = eachValues[1];
                        string[] keySplit = key.Split(':');
                        string[] valueSplit = value.Split(':');
                        string keyFinal = keySplit[1].Replace("\"", "");
                        string valueFinal = valueSplit[1].Replace("\"", "");

                        map.Add(keyFinal.Replace(" ", string.Empty), valueFinal);

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

            XDocument doc = new XDocument();
            XElement root = new XElement("Application");
            foreach (var pair in map)
            {
                XElement current = root;
                string key = pair.Key;
                string value = pair.Value;
                if (key.Contains('_'))
                {
                    string[] keys = key.Split('_');
                    int keyNestedLength = keys.Length; //3
                    for (int i = 0; i <= keyNestedLength; i++) //2<=3
                    {
                        if (i < keyNestedLength && keys[i].Contains('[')) //is array
                        {
                            int arr = int.Parse(keys[i].Substring(keys[i].Length - 2, 1));

                            string arrKey = "";
                            if (keys[i].Substring(keys[i].Length - 6, 3).ToLower().Equals("ies"))
                            {
                                arrKey = keys[i].Substring(0, keys[i].Length - 3);
                            }
                            else if (keys[i].Substring(keys[i].Length - 4, 1).ToLower().Equals("s"))
                            {
                                arrKey = keys[i].Substring(0, keys[i].Length - 3);
                            }
                            else
                            {
                                arrKey = keys[i].Substring(0, keys[i].Length - 3) + "s";
                            }
                            if (current.Element(arrKey) == null)
                            {
                                XElement parent = new XElement(arrKey);
                                current.Add(parent);
                            }
                            current = current.Element(arrKey);
                            string child = "";
                            if (keys[i].Substring(keys[i].Length - 6, 3).ToLower().Equals("ies"))
                            {
                                child = keys[i].Substring(0, keys[i].Length - 6) + "y";
                            }
                            else if (keys[i].Substring(keys[i].Length - 4, 1).ToLower().Equals("s"))
                            {
                                child = keys[i].Substring(0, keys[i].Length - 4);
                            }
                            else
                            {
                                child = keys[i].Substring(0, keys[i].Length - 3);
                            }
                            try
                            {
                                current = current.Descendants(child).ElementAt(arr);
                            }
                            catch (ArgumentOutOfRangeException e)
                            {
                                XElement parent = new XElement(child);
                                current.Add(parent);
                                current = current.Descendants(child).ElementAt(arr);
                            }
                        }
                        else //is not array
                        {
                            if (i < keyNestedLength)//0 < (3-1) 2
                            {
                                if (current.Element(keys[i]) == null)
                                {
                                    XElement parent = new XElement(keys[i]);
                                    current.Add(parent);
                                }
                                current = current.Element(keys[i]);
                            }
                            else
                            {
                                current.SetValue(value);

                            }
                        }
                    }
                }
                else
                {
                    current.Add(new XElement(key, value));
                }
                result += pair.Key + "/" + pair.Value;
            }

            doc.Add(root);
            doc.Save("mm.xml");
            return result;
        }
    }
}
