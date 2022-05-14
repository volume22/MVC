using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Taskfor3
{
    internal class Program
    {
        static List<TableExRate> tableList = new List<TableExRate>();
        static void Main(string[] args)
        {

            int num = 0;
            TimerCallback tm = new TimerCallback(ReadXml);
            Timer timer = new Timer(tm, num, 0, 2000);
            DbAdd();
            Console.ReadLine();
        }
        private static void ReadXml(object obj)
        {
            string checkTable = "";
            XmlDocument doc = new XmlDocument();
            doc.Load("https://www.nationalbank.kz/rss/rates_all.xml");
            XmlNodeList cl = doc.GetElementsByTagName("item");
            tableList.Clear();
            try
            {
                foreach (XmlNode item in cl)
                {
                    TableExRate table = new TableExRate();

                    table.title = item["title"].InnerText;
                    table.pubDate = DateTime.Parse(item["pubDate"].InnerText);
                    table.quant = int.Parse(item["quant"].InnerText);
                    table.description = double.Parse(item["description"].InnerText.Replace('.', ','));

                    tableList.Add(table);
                    checkTable = table.title;
                }
            }
            catch (Exception ex)
            {
                Mail.SendEmailAsync($"{ex.Message}" + $"{checkTable}").GetAwaiter();
            }
            DbUpdate();
        }
        private static void DbUpdate()
        {
            var Lis = tableList;
            /*try
            {*/
            foreach (TableExRate elem in tableList)
            {
                using (TableExRateEntities db = new TableExRateEntities())
                {
                    try
                    {
                        TableExRate rate = db.TableExRate.FirstOrDefault(w => w.title == elem.title);
                        if (rate.description != elem.description)
                        {
                            Mail.SendEmailAsync($"Произошло обновление{elem.title}  ").GetAwaiter();
                            rate.pubDate = elem.pubDate;
                            rate.description = elem.description;
                            db.SaveChanges();
                        }
                    }
                    catch
                    {
                        Console.WriteLine($"Обновлений {elem.title} нет");
                    }
                }
            }
        }
        /*catch (Exception ex)
        {
            Mail.SendEmailAsync($"{ex.Message}").GetAwaiter();
        }*/ private static void DbAdd()
    {
        foreach (TableExRate elem in tableList)
        {
            using (TableExRateEntities db = new TableExRateEntities())
            {
                db.TableExRate.Add(elem);
                db.SaveChanges();
            }
        }
    }
    }
    }
   

   

