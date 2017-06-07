using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WebApplication1.Models;
using System.Web.Mvc;
namespace WebApplication1.Models
{
    public class Data
    {
        List<Action> listAction = new List<Action>();
        public String PerformXML() {
            
            XDocument doc = new XDocument();
            XElement root = new XElement("Application");
            int tmpcount = 0;
            foreach (var pair in this.listAction)
            {
                XElement current = root;
                string key = pair.Key;
                string value = pair.Value;
                HashSet<int> tmp = new HashSet<int>();
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
                                tmp.Add(arr);
                                tmpcount = 0;
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
                                for (int j = 0; j <= arr; j++) {
                                    try
                                    {
                                        current.Descendants(child).ElementAt(j);
                                    }
                                    catch (ArgumentOutOfRangeException a) {
                                        XElement parent = new XElement(child);
                                        current.Add(parent);
                                    }
                                }
                               
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
                List<int> currentList = new List<int>();
                foreach (var t in tmp)
                {
                    currentList.Add(t);
                }
                //result += pair.Key + "/" + pair.Value;
            }
            doc.Add(root);
            doc.Save("mm.xml");
            //var result = "";
            return "OK";
        }

        public void addActionList(String key, dynamic value) {
            Action action = new Action();
            action.Key = key;
            action.Value = value;
            this.listAction.Add(action);
        }
        
    }
    public class Action {
        public string Key { get; set; }
        public string Value { get; set; }

    }

    
}
