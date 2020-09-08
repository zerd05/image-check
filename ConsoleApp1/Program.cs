using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace ConsoleApp1
{
    class Program
    {
        private class ProductUrl
        {
            public string product;
            public List<string> urls;

            public ProductUrl()
            {

            }

            public ProductUrl(string[] s)
            {
                product = s[0];
                urls = new List<string>();
                for (int i = 1; i < s.Length; i++)
                {
                    if (i != 3)
                    {
                        if (s[i] != "")
                            urls.Add(s[i]);
                    }
                    else
                    {

                        List<string> foo = new List<string>(s[i].Split('|'));
                        foreach (string s1 in foo)
                        {
                            if (s1 != "")
                            {
                                urls.Add(s1);
                            }
                        }

                    }
                }
            }
        }
        static void Main(string[] args)
        {
            checkUrls();
        }
        public static void checkUrls()
        {
            List<string> imageLinks = null;
            

            imageLinks = new List<string>(File.ReadAllLines(@"выгрузка.csv"));
            List<ProductUrl> productUrls = new List<ProductUrl>();
            foreach (string s in imageLinks)
            {
                ProductUrl newProductUrl = new ProductUrl(s.Split(';'));
                productUrls.Add(newProductUrl);
            }

            int a = 0;
            int g = 0;
            List<string> BADURL = new List<string>();
            List<string> exMess = new List<string>();
            List<string> tofile404 = new List<string>();
            foreach (ProductUrl current in productUrls)
            {
                g++;
                List<string> badUrls = new List<string>();
                foreach (string s in current.urls)
                {
                    a++;
                    if (!s.Contains("https"))
                    {
                        // logging(current.product+" BAD");
                        BADURL.Add(current.product+";" + s);
                        continue;
                    }
                    WebRequest webRequest = HttpWebRequest.Create(s);
                    webRequest.Method = "HEAD";
                    try
                    {
                        using (WebResponse webResponse = webRequest.GetResponse())
                        {
                            //   logging("OK");
                        }
                    }
                    catch (WebException ex)
                    {
                        //logging(current.product+ " " +ex.Message);
                        exMess.Add(current.product + ";"+s+";" + ex.Message);
                        badUrls.Add(s);
                    }
                    finally
                    {

                    }

                    if (a % 100 == 0)
                    {
                        File.AppendAllLines("Bad.txt", BADURL);
                        File.AppendAllLines("ex.txt", exMess);
                        
                        BADURL.Clear();
                        exMess.Clear();
                    }


          
                }

                if (badUrls.Count != 0)
                {
                    string tofile = current.product + ";";
                    
                    foreach (string url in badUrls)
                    {
                        tofile += url + ";";
                    }
                    tofile404.Add(tofile);
                }

              //  if (g % 100 == 0)
              //{
              Console.Clear();
                    decimal procente = Convert.ToDecimal(g)/Convert.ToDecimal(productUrls.Count);
                    procente = 100 * procente;
                    Console.WriteLine(g.ToString() + "/" + productUrls.Count + "  " + Math.Round(procente,4).ToString() + " %");
                  //  File.WriteAllText("D:/progress.txt", g.ToString() + "/" + productUrls.Count + "  " + procente.ToString() + " %" + Environment.NewLine);
               // }


            }
            File.WriteAllLines("404.txt",tofile404);
        }


    }
}
