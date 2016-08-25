using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
public class DBHelper
{
    //定义连接对象
    private static SqlConnection _sCon = null;
    //从web.confi配置文件中获取数据库连接串
    public static string _sConnectionStr = System.Configuration.ConfigurationManager.ConnectionStrings["Constr"].ToString();

    public static string SConnectionStr
    {
        get { return DBHelper._sConnectionStr; }
        set { DBHelper._sConnectionStr = value; }
    }

    /// <summary>
    /// 获取一个数据库连接对象
    /// </summary>
    /// <returns></returns>
    public static SqlConnection GetSqlConnection()
    {
        try
        {
            return new SqlConnection(SConnectionStr);
        }
        catch (Exception ex)
        {
            //:@在这里处理异常
            throw new Exception(ex.Message);
            //:$
        }
    }

    /// <summary>
    /// 执行指定sql语句，并返回DataTable对象
    /// </summary>
    /// <param name="CommandText">sql语句</param>
    /// <param name="Params">参数列表</param>
    /// <returns></returns>
    public static DataTable ExecuteAsDataTable(string CommandText, params Parameter[] Params)
    {
        DataTable dataTable = null;
        try
        {
            if (_sCon == null) //如果连接对象为空,则新建个连接对象
            {
                _sCon = GetSqlConnection();
            }
            if (_sCon.State != ConnectionState.Open)
            {
                _sCon.Open();
            }
            using (SqlDataAdapter adapter = new SqlDataAdapter("", _sCon))
            {
                SqlCommand cmd = new SqlCommand(CommandText, _sCon);
                AddParameter(ref cmd, Params); //为sql语句填充参数
                adapter.SelectCommand = cmd;
                dataTable = new DataTable();
                adapter.Fill(dataTable);
            }
        }
        catch (Exception ex1)
        {
            //:@在这里处理异常
            throw new Exception(ex1.Message);
            //:$
        }
        finally
        {
            try
            {
                _sCon.Close();
            }
            catch (Exception ex2)
            {
                //:@在这里处理异常
                throw new Exception(ex2.Message);
                //:$
            }
        }
        return dataTable;
    }

    /// <summary>
    /// 执行指定sql语句
    /// </summary> 
    /// <param name="CommandText">sql语句</param>
    /// <param name="Params">参数列表</param>
    /// <returns>返回受影响行数 小于零执行出错</returns>
    public static int ExecuteNonQuery(string CommandText, params Parameter[] Params)
    {
        int result = 0;
        SqlTransaction transaction = null;
        try
        {
            if (_sCon == null)
            {
                _sCon = GetSqlConnection();
            }
            if (_sCon.State != ConnectionState.Open)
            {
                _sCon.Open();
            }
            SqlCommand cmd = new SqlCommand(CommandText, _sCon);
            AddParameter(ref cmd, Params);
            transaction = _sCon.BeginTransaction(); //在事务中执行sql语句
            cmd.Transaction = transaction;
            result = cmd.ExecuteNonQuery();
            transaction.Commit();
        }
        catch (Exception ex1)
        {
            transaction.Rollback();
            result = -1;
            //:@在这里处理异常
            throw new Exception(ex1.Message);
            //:$
        }
        finally
        {
            try
            {
                _sCon.Close();
            }
            catch (Exception ex2)
            {
                //:@在这里处理异常
                throw new Exception(ex2.Message);
                //:$
            }
        }
        return result;
    }


    /// <summary>
    /// 执行指定sql语句，并返回第一行第一列值
    /// </summary>    
    /// <param name="CommandText">sql语句</param>
    /// <param name="Params">参数列表</param>
    /// <returns>返回一行一列值</returns>
    public static object ExecuteScalar(string CommandText, params Parameter[] Params)
    {
        object result = null;
        try
        {
            if (_sCon == null)
            {
                _sCon = GetSqlConnection();
            }
            if (_sCon.State != ConnectionState.Open)
            {
                _sCon.Open();
            }
            SqlCommand cmd = new SqlCommand(CommandText, _sCon);
            AddParameter(ref cmd, Params);
            result = cmd.ExecuteScalar();
        }
        catch (Exception ex)
        {
            //:@在这里处理异常
            throw new Exception(ex.Message);
            //:$
        }
        finally
        {
            try
            {
                _sCon.Close();
            }
            catch (Exception ex2)
            {
                //:@在这里处理异常
                throw new Exception(ex2.Message);
                //:$
            }
        }
        return result;
    }


    /// <summary>
    /// 执行指定存储过程(事务中),返回结果集
    /// </summary>
    /// <param name="StoredProcedureName">存储过程名称</param>
    /// <param name="Outputs">输出参数列表</param>
    /// <param name="Params">输入参数列表</param>
    /// <returns>返回查询结果集</returns>
    public static DataSet ExecuteStoredProcedureWithQuery(string StoredProcedureName, ref OutputParameter Outputs, params Parameter[] Params)
    {
        DataSet dataSet = null;
        SqlTransaction transaction = null;
        try
        {
            if (_sCon == null)
            {
                _sCon = GetSqlConnection();
            }
            if (_sCon.State != ConnectionState.Open)
            {
                _sCon.Open();
            }
            using (SqlDataAdapter adapter = new SqlDataAdapter("", _sCon))
            {
                SqlCommand cmd = new SqlCommand(StoredProcedureName, _sCon);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();//清除参数
                AddParameter(ref cmd, Params);//为sql语句添加参数  
                transaction = _sCon.BeginTransaction(); //开始事务
                cmd.Transaction = transaction;
                adapter.SelectCommand = cmd;
                dataSet = new DataSet();
                adapter.Fill(dataSet);
                transaction.Commit();
                AddReturnParameter(cmd, ref Outputs); //将输出参数添加到 Outputs中 以便上层获取输出参数值
            }
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            //:@在这里处理异常
            throw new Exception(ex.Message);
            //:$
        }
        finally
        {
            try
            {
                _sCon.Close();
            }
            catch (Exception ex2)
            {
                //:@在这里处理异常
                throw new Exception(ex2.Message);
                //:$
            }
        }
        return dataSet;
    }

    /// <summary>
    /// 执行指定存储过程(事务中),不返回结果集
    /// </summary>
    /// <param name="StoredProcedureName">存储过程名称</param>
    /// <param name="Outputs">输出参数列表</param>
    /// <param name="Params">输入参数列表</param>
    /// <returns>返回受影响行数</returns>
    public static int ExecuteStoredProcedureNonQuery(string StoredProcedureName, ref OutputParameter Outputs, params Parameter[] Params)
    {
        int result = 0;
        SqlTransaction transaction = null;
        try
        {
            if (_sCon == null)
            {
                _sCon = GetSqlConnection();
            }
            if (_sCon.State != ConnectionState.Open)
            {
                _sCon.Open();
            }
            SqlCommand cmd = new SqlCommand(StoredProcedureName, _sCon);
            cmd.CommandType = CommandType.StoredProcedure;
            AddParameter(ref cmd, Params);
            transaction = _sCon.BeginTransaction();
            cmd.Transaction = transaction;
            result = cmd.ExecuteNonQuery();
            transaction.Commit();
            AddReturnParameter(cmd, ref Outputs);
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            //:@在这里处理异常
            throw new Exception(ex.Message);
            //:$
        }
        finally
        {
            try
            {
                _sCon.Close();
            }
            catch (Exception exception2)
            {
                //:@在这里处理异常
                throw new Exception(exception2.Message);
                //:$
            }
        }
        return result;
    }

    /// <summary>
    /// 为SqlCommand 对象添加参数
    /// </summary>
    /// <param name="Cmd">SqlCommand对象</param>
    /// <param name="Params">参数列表</param>
    private static void AddParameter(ref SqlCommand Cmd, params Parameter[] Params)
    {
        //如果SqlCommand对象和参数列表不为空,则进行添加参数操作
        if ((Params != null) && (Cmd != null))
        {
            for (int i = 0; i < Params.Length; i++)
            {
                if (Params[i] != null) //判断参数是否为空
                {
                    SqlParameter parameter = new SqlParameter();
                    parameter.ParameterName = Params[i].Name.StartsWith("@") ? Params[i].Name : ("@" + Params[i].Name); //哪果参数没有以"@"开头，则手动添加上"@"符号
                    parameter.SqlDbType = ConvertDbTypeToSqlDbType(Params[i].Type); //设置参数类型
                    if (Params[i].Size > 0)
                    {
                        parameter.Size = Params[i].Size; //设置参数大小
                    }
                    parameter.Direction = Params[i].Direction; //设置参数方向
                    //如果参数方向为"输入" 或者 "输入输出"并且参数不为空，则为参数的 Value属性赋值
                    if (((Params[i].Direction == ParameterDirection.InputOutput) || (Params[i].Direction == ParameterDirection.Input)) && (Params[i].Value != null))
                    {
                        parameter.Value = Params[i].Value;
                    }
                    Cmd.Parameters.Add(parameter); //将参数添加到SqlCommand命令中
                }
            }
        }
    }

    /// <summary>
    /// 从SqlCommand 对象获取输出参数值
    /// </summary>
    /// <param name="Cmd">SqlCommand对象</param>
    /// <param name="Outputs">输出参数对象(这里是传的引用)</param>
    private static void AddReturnParameter(SqlCommand Cmd, ref OutputParameter Outputs)
    {
        //如果SqlCommand对象和参数列表不为空,则进行获取参数操作
        if ((Cmd != null) && (Cmd.Parameters.Count != 0))
        {
            for (int i = 0; i < Cmd.Parameters.Count; i++)
            {
                SqlParameter parameter = Cmd.Parameters[i];
                //如果参数方向为"输出" 或者 "输入输出"并且参数不为空，则提取出参数的"ParameterName"与 "Value"属性的值，并添加到Outputs中
                if (((parameter.Direction == ParameterDirection.InputOutput) || (parameter.Direction == ParameterDirection.Output)) || (parameter.Direction == ParameterDirection.ReturnValue))
                {
                    Outputs.Add(parameter.ParameterName, parameter.Value);
                }
            }
        }
    }

    /// <summary>
    /// 输出参数,用来获取执行Sql语句后的输出参数
    /// </summary>
    public class OutputParameter
    {
        private List<string> _parametersName = new List<string>(); //保存输出参数名称
        private List<object> _parametersValue = new List<object>();//保存输出参数值

        public void Add(string Name, object Value)
        {
            this._parametersName.Add(Name.StartsWith("@") ? Name.Substring(1, Name.Length - 1) : Name);//去掉参数前的"@"符号
            this._parametersValue.Add(Value);
        }

        //清除所有参数
        public void Clear()
        {
            this._parametersName.Clear();
            this._parametersValue.Clear();
        }
        //获取参数个数
        public int Count
        {
            get
            {
                if (this._parametersName == null)
                {
                    return 0;
                }
                return this._parametersName.Count;
            }
        }

        /// <summary>
        /// 获取参数值(按参数名称)
        /// </summary>
        /// <param name="Name">参数名称</param>
        /// <returns></returns>
        public object this[string Name]
        {
            get
            {
                if (this._parametersValue != null)
                {
                    for (int i = 0; i < this._parametersName.Count; i++)
                    {
                        if (this._parametersName[i].ToString() == Name)
                        {
                            return this._parametersValue[i];
                        }
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 获取参数值(按下标)
        /// </summary>
        /// <param name="Index">索引</param>
        /// <returns></returns>
        public object this[int index]
        {
            get
            {
                if (this._parametersValue == null || this._parametersValue.Count == 0)
                {
                    return null;
                }
                return this._parametersValue[index];
            }
        }
    }

    /// <summary>
    /// 执行Sql语句的参数
    /// </summary>
    public class Parameter
    {
        public ParameterDirection Direction; //方向
        public string Name; //名称
        public int Size; //大小
        public DbType Type; //类型
        public object Value; //值

        /// <summary>
        /// 创建数据库参数，默认将参数处理为string类型
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数值</param>
        public Parameter(string name, object value)
        {
            this.Name = name;
            this.Type = DbType.String;
            this.Size = Int32.MaxValue;
            this.Direction = ParameterDirection.Input;
            this.Value = value;
        }
        public Parameter(string name, DbType type, ParameterDirection direction, object value)
        {
            this.Name = name;
            this.Type = type;
            this.Size = Int32.MaxValue;
            this.Direction = direction;
            this.Value = value;
        }

        public Parameter(string name, DbType type, int size, ParameterDirection direction, object value)
        {
            this.Name = name;
            this.Type = type;
            this.Size = size;
            this.Direction = direction;
            this.Value = value;
        }
    }

    private static SqlDbType ConvertDbTypeToSqlDbType(DbType dbType)
    {
        switch (dbType)
        {
            case DbType.Boolean:
                return SqlDbType.Bit;
            case DbType.Byte:
                return SqlDbType.TinyInt;
            case DbType.Int16:
                return SqlDbType.SmallInt;
            case DbType.Int32:
                return SqlDbType.Int;
            case DbType.Int64:
                return SqlDbType.BigInt;
            case DbType.Single:
                return SqlDbType.Real;
            case DbType.Double:
                return SqlDbType.Float;
            case DbType.Decimal:
                return SqlDbType.Decimal;
            case DbType.DateTime:
                return SqlDbType.DateTime;
            case DbType.Binary:
                return SqlDbType.Image;
            case DbType.String:
                return SqlDbType.NVarChar;
            case DbType.Guid:
                return SqlDbType.UniqueIdentifier;
            default:
                return SqlDbType.NText;
        }

    }
}