using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hsr.Core.Cache;
using Hsr.Core.Infrastructure;
using Hsr.Model.CustomValidators;
using Hsr.Model.Models.ViewModel;
using Hsr.Models;
using Hsr.Service.Iservice;

namespace Hsr.Service.Service
{

    public class ColumnValidatorContext
    {
        public DatamappingColumn Column { get; set; }

        public DatamappingParam ValidatorParam { get; set; }

        public List<CellValidatorContext> CellValidatorContexts { get; set; }

        private bool _initialized = false;
        public void Initial(List<ValidatorEx> mappingValidators)
        { 
            CellValidatorContext cellValidatorContext = new CellValidatorContext();
            cellValidatorContext.BelongColumn = Column;
            cellValidatorContext.Validators = new List<IValidator>();
            foreach (var datamappingValidator in mappingValidators)
            {
                var validator =
                    EngineContext.Current.ContainerManager.Resolve<IValidator>(datamappingValidator.Registername);
                validator.Param = datamappingValidator.Params.ToArray();
                cellValidatorContext.Validators.Add(validator);
                
            }
            CellValidatorContexts=new List<CellValidatorContext>(){cellValidatorContext};
        }

        public void Add(string value)
        {
            if (!_initialized||!CellValidatorContexts.Any())
            {
                throw new ConstraintException("列验证器上下文没有初始化！");
            }
            var validator = CellValidatorContexts.First().Clone();
            validator.Value = value;
            CellValidatorContexts.Add(validator);
        }

        public List<string> IsValid(bool isContinue)
        {
            var messages = new List<string>();
            foreach (var cellValidator in  CellValidatorContexts)
            {
                foreach (var validator in cellValidator.Validators)
                {
                    if (!validator.IsValid(cellValidator))
                    {
                        if (!isContinue)
                        {
                            return new List<string>(){validator.ErrorMessage};
                        }
                        messages.Add(validator.ErrorMessage);
                    }
                }

            }
            return messages;
        }
    }

    public class TableValidatorContext
    {

        private bool _initialized = false;

        private bool _onErrorStop;

        /// <summary>
        /// 只有有验证失败就停止继续验证
        /// </summary>
        /// <value>
        ///   <c>true</c> if [on error stop]; otherwise, <c>false</c>.
        /// </value>
        public bool OnErrorStop
        {
            get { return _onErrorStop; }
            set { _onErrorStop = value; }
        }

        public List<ColumnValidatorContext> ColumnValidatorContexts { get; set; }

        public void Initial(List<DataColumnEx> columns)
        {
            ColumnValidatorContext columnValidatorContext = new ColumnValidatorContext();
            foreach (var datamappingColumn in columns)
            {
                columnValidatorContext.Column = datamappingColumn;
                 
                columnValidatorContext.Initial(datamappingColumn.Validators);
            }

            _initialized = true;
        }

        public List<string> IsValid()
        {
            List<string> messages = new List<string>();
            foreach (var columnValidatorContext in ColumnValidatorContexts)
            {
                var error = columnValidatorContext.IsValid(!OnErrorStop);
                if (error.Any())
                {
                    if (OnErrorStop)
                   {
                    return error;
                    }
                }
                messages.AddRange(error);
            }
            return messages;
        }

        public List<string> IsValidParall()
        {
            var message = new List<string>();
            CancellationTokenSource cts = new CancellationTokenSource();
            ParallelOptions po = new ParallelOptions();
            po.CancellationToken = cts.Token;
            po.MaxDegreeOfParallelism = System.Environment.ProcessorCount;
            Parallel.ForEach(ColumnValidatorContexts, po, columnContext =>
            {
                if (cts.IsCancellationRequested)
                {
                    po.CancellationToken.ThrowIfCancellationRequested();
                }
                var error = columnContext.IsValid(!OnErrorStop);
                message.AddRange(error);
                if (OnErrorStop)
                {
                    if (OnErrorStop)
                    {
                        if (error.Any())
                        {
                            cts.Cancel();
                        }
                       
                    }
                }
                
            });
            return message;
        }
    }


    public class ImportSevice : IImportService
    {
        private readonly ICacheManager _cacheManager;
        public ImportSevice(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }
        public void Import(string tableName)
        {
            
        }
    }
}
