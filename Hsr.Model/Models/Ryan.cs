#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation.Attributes;
using Hsr.Data;
using Hsr.Data.CustomAttribute;
using Hsr.Model.Validators;
using MVC.Controls.Paging;

#endregion

namespace Hsr.Models
{
    [Validator(typeof (RyanValidator))]
    [Table("RYAN", Schema = "CTUNI_PUB")]
    public class Ryan : BaseModel
    {
        [Column("NAME")]
        public string Name { get; set; }

        [Key]
        [KeyGenerate(KeyKind = KeyKind.Guid)]
        [Column("ID")]
        public string Id { get; set; }
    }

    public class MessageFilterModel : BasePageableModel
    {
        public List<ControllerFilterData> Data { get; set; }
    }

    [Table("CONTROLLER_MSG", Schema = "CTUNI_PUB")]
    public class ControllerFilterData : BaseModel
    {
        [Column("MSG_ID")]
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? MsgId { get; set; }

        [
            Column("MODULE_NAME")]
        public string ModuleName { get; set; }

        [
            Column("METHODE_NAME")]
        public string MethodeName { get; set; }

        [
            Column("OPERATION_TIME")]
        public DateTime? OperationTime { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }

        [Column("USER_ID")]
        public string UserID { get; set; }

        [
            Column("OPERATE_TYPE")]
        public short? OperateType { get; set; }


        [
            Column("PARAMSJSONSTR")]
        public string ParamsJsonStr { get; set; }


        [Column("TEST_ID")]
        [ForeignKey("Test")]
        public decimal? TestId { get; set; }

        public virtual Test Test { get; set; }
    }


    [Table("TEST", Schema = "CTUNI_PUB")]
    public class Test : BaseModel
    {
        [Column("NAME")]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [KeyGenerate(KeyKind = KeyKind.Id, SeqenceName = "TESTCHEN")]
        [Column("T1")]
        public decimal ControlleId { get; set; }

        [Column("ID")]
        public string Ids { get; set; }

        public virtual ICollection<ControllerFilterData> Person { get; set; }
    }
}