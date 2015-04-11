//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Data;
//using System.Linq;
//using System.Reflection;
//using Oracle.DataAccess.Client;

//namespace Hsr.Data.Oracle
//{
//    public class DataAccess
//    {
//        public bool ExecuteNonQueryBatch(string dataSourceName, string cmdText, OracleParameter[] cmdParms, int arrayCount,
//       CommandType cmdType = CommandType.Text)
//        {
//            OracleConnection connection = null;
//            try
//            {
//                using (connection=new OracleConnection())
//                {
//                    OracleCommand cmd = new OracleCommand();
//                    cmd.Connection = connection;   
                   
//                    cmd.CommandText = cmdText;
//                    cmd.CommandType = cmdType;
//                    cmd.BindByName = true;
//                    cmd.Parameters.AddRange(cmdParms);
//                    cmd.ArrayBindCount = arrayCount;
//                    int result= cmd.ExecuteNonQuery();
//                    if (result>0)
//                    {
//                        return true;
//                    }
//                    return false;
//                }
//            }
//            catch (Exception ex)
//            {
               
                
//            }
//            finally
//            {
//                if (connection!=null)
//                {
//                    connection.Close();
//                }
              
//            }
//            return false;
//        }

//        private object[] GetValues<T>(IEnumerable<T> list, PropertyInfo property)
//        {
//            List<object> objects = new List<object>();
//            foreach (var entity in list)
//            {
//                var value = property.GetValue(entity, null);
//                if (value == null)
//                {
//                    objects.Add(DBNull.Value);
//                }
//                else
//                {
//                    objects.Add(value);
//                }

//            }
//            return objects.ToArray();
//        }

//        private OracleParameter GetOracleParameterEach<T>(IEnumerable<T> list, PropertyInfo property, ColumnAttribute column, bool setValueNull = false)
//        {
//            var oracleParameter = new OracleParameter();

//            if (property.PropertyType == typeof(Guid) ||
//                property.PropertyType == typeof(string))
//            {
//                oracleParameter.OracleDbType = OracleDbType.Varchar2;
//            }
//            else if (property.PropertyType == typeof(decimal))
//            {
//                oracleParameter.OracleDbType = OracleDbType.Decimal;
//            }
//            else if (property.PropertyType == typeof(int) ||
//                     property.PropertyType == typeof(Int32))
//            {
//                oracleParameter.OracleDbType = OracleDbType.Int32;
//            }
//            else if (property.PropertyType == typeof(Int16))
//            {
//                oracleParameter.OracleDbType = OracleDbType.Int16;
//            }
//            else if (property.PropertyType == typeof(Int64))
//            {
//                oracleParameter.OracleDbType = OracleDbType.Int64;
//            }
//            else if (property.PropertyType == typeof(double) ||
//                     property.PropertyType == typeof(float))
//            {
//                oracleParameter.OracleDbType = OracleDbType.Double;
//            }
//            else if (property.PropertyType == typeof(char))
//            {
//                oracleParameter.OracleDbType = OracleDbType.Char;
//            }
//            else if (property.PropertyType == typeof(DateTime))
//            {
//                oracleParameter.OracleDbType = OracleDbType.Date;
//            }
//            else if (property.PropertyType == typeof(byte[]))
//            {
//                oracleParameter.OracleDbType = OracleDbType.Blob;
//            }

//            oracleParameter.ParameterName = ":" + column.Name;
//            var enumerable = list as IList<T> ?? list.ToList();
//            if (setValueNull)
//            {
//                oracleParameter.Value = null;
//            }
//            else
//            {
//                oracleParameter.Value = GetValues<T>(enumerable, property);
//            }
//            return oracleParameter;
//        }
//    }


   
//}
