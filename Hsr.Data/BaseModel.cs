#region

using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Hsr.Data.CustomAttribute;
 

#endregion

namespace Hsr.Data
{
    public abstract class BaseModel
    {
        public const string Schema = "MNCMS_APP";

        public void GenerateId()
        {
            var memberInfos = GetType().GetProperties().Take(2);
           
            foreach (var memberInfo in memberInfos)
            {
                
                var keyGener = memberInfo.GetCustomAttributes(true).OfType<KeyGenerate>().ToList();

                if (keyGener.Any())
                {
                    var key = keyGener.First();

                    switch (key.KeyKind)
                    {
                        case KeyKind.Guid:
                            memberInfo.SetValue(this, Guid.NewGuid().ToString("N"));
                            break;
                        //case KeyKind.Id:
                        //    if (!String.IsNullOrEmpty(key.SeqenceName))
                        //    {
                        //        string slectedSqu = String.Format("select {0}.nextval from dual", key.SeqenceName);
                        //        using (var connection = new OracleConnection(ConfigurationManager.ConnectionStrings[BaseObjectContext.Connect].ConnectionString))
                        //        {
                        //            connection.Open();
                        //            var command = connection.CreateCommand();
                        //            command.CommandText = slectedSqu;
                        //            var seq = command.ExecuteScalar();
                        //            memberInfo.SetValue(this, seq);
                        //        }
                        //    }
                            break;
                    }
                }
            }
        }

        public PropertyInfo GetKey()
        {
             
            var key = from property in this.GetType().GetProperties().Take(2)
                      from ca in property.GetCustomAttributesData()
                     
                      where ca.AttributeType.Name.Equals(typeof(KeyAttribute).Name)
                      select property;
            var propertyInfos = key as PropertyInfo[] ?? key.ToArray();
            return propertyInfos.Any() ? propertyInfos.First() : null;
        }
    }
}