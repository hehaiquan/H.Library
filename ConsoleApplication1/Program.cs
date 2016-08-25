using LRXTOOL;
using Maticsoft.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {

        static void Main(string[] args)
        {

            #region 时间差

            //var dateFrom = DateTime.Parse("2016-06-15");
            //var dateTo = DateTime.Parse("2017-06-15");
            //TimeSpan span = dateTo - dateFrom;

            //var spanMonth = dateTo.Year * 12 + dateTo.Month - (dateFrom.Year * 12 + dateFrom.Month) + 1;
            //var checkDate = dateFrom.AddMonths(spanMonth / 2);

            //Console.WriteLine(string.Format("相差{0}个月", spanMonth));
            //Console.WriteLine(string.Format("核查日期：{0}", checkDate)); 
            #endregion

            //Console.WriteLine(DateTime.Parse("2016.10月").ToString());

            #region 去掉中文及字母
            //string text = "约50克sdfasdfdsf无可奈何花落去在顶替厅在";
            //Regex oRegex = new Regex(@"[a-zA-Z\u4E00-\u9FA5\-]*");
            //Console.WriteLine(oRegex.Replace(text, ""));
            #endregion

            #region 获取括号中的内容

            //var text = "现存(瓶";
            //Regex regex = new Regex(@"\(.*?\)");

            //var match = regex.Match(text);
            //Console.WriteLine(match.Value.Trim('(', ')'));

            //MatchCollection matches = regex.Matches(text);
            //StringBuilder sb = new StringBuilder();//存放匹配结果
            //foreach (Match match in matches)
            //{
            //    string value = match.Value.Trim('(', ')');
            //    sb.AppendLine(value);
            //}
            //Console.WriteLine(sb.ToString());

            #endregion

            //GetFieldValueByStr();


            var a = new List<int>() { 1,2,3,5,9 };
            var b = new List<int>() { 4,3,9 };
            var expectedList = a.Except(b);
            Console.WriteLine(string.Join(",", expectedList));


            // var ds = ExcelToDs(@"C:\无机类--标液.xlsx", "标液");




            //var humanList = new List<Human>()
            //{
            //    new Human() {ID=1,Name="aa",Age=21 ,BirthDay=DateTime.Now},
            //    new Human() { ID=2,Name="aa",Age=25,BirthDay = DateTime.Parse("2016-05-24 09:02:11")},
            //    new Human() { ID=3,Name ="cc", Age=30,BirthDay=DateTime.Now},
            //    new Human() { ID=4,Name ="cc", Age=32,BirthDay=DateTime.Now},
            //    new Human() { ID=4,Name ="cc", Age=32,BirthDay = DateTime.Parse("2016-05-24 09:02:50")}
            //};

            //var group = humanList.GroupBy(i => i.Name);
            //foreach (var item in group)
            //{
            //    var lll = item.ToList();

            //    var key = item.Key;

            //    var subGroup = lll.GroupBy(i => i.Age);
            //    foreach (var subItem in subGroup)
            //    {
            //        var subList = subItem.ToList();
            //        var k = subItem.Key;
            //    }

            //}


            Console.Read();

        }

        public static DataSet ExcelToDs(string path,string sheetName)
        {
            //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";" + "Extended Properties='Excel 8.0;'";
            string strConn = "Provider = Microsoft.Jet.OLEDB.4.0; Data Source = " + path + ";" + ";HDR=Yes;IMEX=1;Extended Properties=Excel 12.0;";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            DataSet ds = null;
            strExcel = "select * from [" + sheetName + "$]";
            myCommand = new OleDbDataAdapter(strExcel, strConn);
            ds = new DataSet();
            myCommand.Fill(ds, "table1");

            return ds;
        }


        public static void GetFieldValueByStr()
        {
            //var match = Regex.Match("{Name}={Name}平行样", @"\{(.*)\}");
            //var result = match.Captures[0].Value;
            //var v = match.Groups[1].Value;




            //(?<={)[^{}]*
            //var match2 = Regex.Matches("{Name}={MonitorSitePlace}平行样", @"(?<={)[^{}]*");
            //foreach (Match item in match2)
            //{
            //    Console.WriteLine(item.Value);

            //}


            var humanList = new List<Human>()
            {
                new Human() {ID=1,Name="Mark",Age=21 ,BirthDay=DateTime.Now},
                new Human() { ID=2,Name="Jack",Age=25,BirthDay = DateTime.Parse("2016-05-24 09:02:11")},
                new Human() { ID=3,Name ="Alpha", Age=30,BirthDay=DateTime.Now},
                new Human() { ID=4,Name ="Mark平行样", Age=32,BirthDay=DateTime.Now},
                new Human() { ID=4,Name ="密Jack", Age=25,BirthDay = DateTime.Parse("2016-05-24 09:02:50")}
            };

            #region MyRegion
            var humanList2 = new List<Human>()
            {
                new Human() {Name="Mark",Age=21 },
                new Human() { Name="Jack",Age=25},
                new Human() { Name ="Alpha平行", Age=30},
                new Human() { Name ="Bruce", Age=32}
            };

            var humanList3 = new List<Human>()
            {
                new Human() {Name="Mark",Age=21 },
                new Human() { Name="密码",Age=25},
                new Human() { Name ="Alpha", Age=30},
                new Human() { Name ="Bruce", Age=32}
            };

            var humanList4 = new List<Human>()
            {
                new Human() {Name="Mark",Age=21 },
                new Human() { Name="Jack",Age=25},
                new Human() { Name ="密Alpha", Age=30},
                new Human() { Name ="Bruce", Age=32}
            };
            #endregion


            var ParallelSampleRule = "{Name}=密";
            var normalSampleRule = "{BirthDay}={BirthDay};密{Name}={Name}";






            //获取平行样
            var parallelSampleList = new List<Human>();
            foreach (var item in humanList)
            {
                var pr = ParallelSampleRule.Split('=').ToList();

                //左边条件中的字段
                var leftFieldList = GetFieldList(pr[0], @"(?<={)[^{}]*");

                //右边结果中的字段
                var rightFieldList = GetFieldList(pr[1], @"(?<={)[^{}]*");

                var conditionValue = GetValueByRule(pr[0], item, leftFieldList);
                var resultValue = GetValueByRule(pr[1], item, rightFieldList);

                if (conditionValue.Contains(resultValue))
                {
                    parallelSampleList.Add(item);
                }

            }

            //<平行样,普通样>
            var dictNormalParallelList = new Dictionary<Human, Human>();
            foreach (var parallel in parallelSampleList)
            {
                //循环，查询是否存在平行样
                foreach (var item in humanList)
                {
                    var matchRecordList = new List<bool>();
                    var ruleArr = normalSampleRule.Split(';').ToList();
                    foreach (var subRule in ruleArr)
                    {

                        var conList = subRule.Split('=').ToList();

                        //左边条件中的字段
                        var leftFieldList = GetFieldList(conList[0], @"(?<={)[^{}]*");
                        //右边条件中的字段
                        var rightFieldList = GetFieldList(conList[1], @"(?<={)[^{}]*");

                        var normalSampleFieldValue = GetValueByRule(conList[0], item, leftFieldList);
                        var parallelFieldValue = GetValueByRule(conList[1], parallel, rightFieldList);

                        matchRecordList.Add(normalSampleFieldValue == parallelFieldValue ? true : false);
                    }

                    if (!matchRecordList.Where(i => i == false).Any())
                    {
                        dictNormalParallelList.Add(parallel, item);
                        break;
                    }
                }
            }


            #region other
            //var ruleList = new List<string>()
            //{
            //    //"{Name_P}={Name}_平行样;{P_Name}=_平行样;",
            //    //"{Name}=密{Name}",
            //    //"{Name}=密码",
            //    "{Name}=密码;{Age}=21;{ID}=4;{PickTime}={PickTime}"
            //};
            ////循环，查询是否存在平行样
            //foreach (var item in humanList)
            //{
            //    var ruleObjList = new List<Human>();

            //    foreach (var rule in ruleList)
            //    {
            //        var ruleArr = rule.Split(';').ToList();


            //        #region foreach rule list
            //        foreach (var subRule in ruleArr)
            //        {

            //            var conList = subRule.Split('=').ToList();

            //            //左边条件中的字段
            //            var leftFieldList = GetFieldList(conList[0], @"(?<={)[^{}]*");
            //            //右边条件中的字段
            //            var rightFieldList = GetFieldList(conList[1], @"(?<={)[^{}]*");


            //            var right = conList[1];
            //            for (int i = 0; i < rightFieldList.Count; i++)
            //            {
            //                var fieldName = rightFieldList[i];
            //                var fieldValue = item.GetType().GetProperty(fieldName).GetValue(item, null);
            //                right = string.Format(right.Replace(fieldName, i.ToString()), fieldValue);
            //            }


            //            #region //判断平行样
            //            foreach (var pItem in humanList)
            //            {
            //                if (pItem.ID == item.ID)
            //                {
            //                    continue;
            //                }
            //                //字段值
            //                var value = conList[0];
            //                for (int i = 0; i < leftFieldList.Count; i++)
            //                {
            //                    var fieldName = leftFieldList[i];
            //                    var fieldValue = pItem.GetType().GetProperty(fieldName).GetValue(pItem, null);
            //                    value = string.Format(value.Replace(fieldName, i.ToString()), fieldValue);
            //                }

            //                //如果pItem的中leftField对应属性的值等于item中leftField对应的属性值加上平行样格式后值
            //                if (value.ToString() == right)
            //                {
            //                    ruleObjList.Add(pItem);
            //                }

            //            }
            //            #endregion

            //        }
            //        #endregion

            //    }

            //    var isSame = ruleObjList.Select(i => i.ID).Distinct().ToList().Count == 1;
            //    if (isSame)
            //    {
            //        parallelList.Add(item, ruleObjList.FirstOrDefault());
            //    }

            //} 
            #endregion

            var ggg = 0;

        }

        /// <summary>
        /// 从regText中获取匹配pattern的列表
        /// </summary>
        /// <param name="regText"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static List<string> GetFieldList(string regText, string pattern)
        {
            var rightMatchs = Regex.Matches(regText, pattern);
            //右边条件中的字段
            var rightFieldList = new List<string>();
            foreach (Match match in rightMatchs)
            {
                rightFieldList.Add(match.Value);
            }
            rightFieldList = rightFieldList.Distinct().ToList();
            return rightFieldList;
        }


        /// <summary>
        /// 解析规则
        /// </summary>
        /// <param name="rule">规则</param>
        /// <param name="obj">数据实体</param>
        /// <param name="objFieldList">规则中包含的实体属性</param>
        /// <returns>返回解析后的结果</returns>
        public static string GetValueByRule(string rule, object obj, List<string> objFieldList)
        {
            var value = rule;
            for (int i = 0; i < objFieldList.Count; i++)
            {
                var fieldName = objFieldList[i];
                var fieldType = obj.GetType().GetProperty(fieldName).PropertyType;
                var fieldValue = obj.GetType().GetProperty(fieldName).GetValue(obj, null).ToString();
                if (fieldType.Name == "DateTime")
                {
                    //fieldValue = DateTime.Parse(fieldValue.ToString()).ToString("yyyy-MM-dd hh:mm");
                }
                value = string.Format(value.Replace(fieldName, i.ToString()), fieldValue);
            }
            return value;

        }

        ///// <summary>
        ///// 导入EXCEL到DataSet
        ///// </summary>
        ///// <param name="fileName">Excel全路径文件名</param>
        ///// <returns>导入成功的DataSet</returns>
        //public DataTable ImportExcel(string fileName)
        //{
        //    //判断是否安装EXCEL
        //    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
        //    if (xlApp == null)
        //    {
        //        _ReturnStatus = -1;
        //        _ReturnMessage = "无法创建Excel对象，可能您的计算机未安装Excel";
        //        return null;
        //    }

        //    //判断文件是否被其他进程使用            
        //    Microsoft.Office.Interop.Excel.Workbook workbook;
        //    try
        //    {
        //        workbook = xlApp.Workbooks.Open(fileName, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, 1, 0);
        //    }
        //    catch
        //    {
        //        _ReturnStatus = -1;
        //        _ReturnMessage = "Excel文件处于打开状态，请保存关闭";
        //        return null;
        //    }

        //    //获得所有Sheet名称
        //    int n = workbook.Worksheets.Count;
        //    string[] SheetSet = new string[n];
        //    System.Collections.ArrayList al = new System.Collections.ArrayList();
        //    for (int i = 1; i <= n; i++)
        //    {
        //        SheetSet[i - 1] = ((Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[i]).Name;
        //    }

        //    //释放Excel相关对象
        //    workbook.Close(null, null, null);
        //    xlApp.Quit();
        //    if (workbook != null)
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
        //        workbook = null;
        //    }
        //    if (xlApp != null)
        //    {
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
        //        xlApp = null;
        //    }
        //    GC.Collect();

        //    //把EXCEL导入到DataSet
        //    DataSet ds = new DataSet();
        //    DataTable table = new DataTable();
        //    string connStr = " Provider = Microsoft.Jet.OLEDB.4.0 ; Data Source = " + fileNamee + ";Extended Properties=Excel 8.0";
        //    using (OleDbConnection conn = new OleDbConnection(connStr))
        //    {
        //        conn.Open();
        //        OleDbDataAdapter da;
        //        string sql = "select * from [" + SheetSet[0] + "$] ";
        //        da = new OleDbDataAdapter(sql, conn);
        //        da.Fill(ds, SheetSet[0]);
        //        da.Dispose();
        //        table = ds.Tables[0];
        //        conn.Close();
        //        conn.Dispose();
        //    }
        //    return table;
        //}


    }
}
