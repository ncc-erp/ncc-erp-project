using System;
using System.Collections.Generic;
using System.Text;

namespace NccCore.DataExport.Dto
{
    public class ExcelGroupData<Data, GroupRow>
        where Data : class
        where GroupRow : class
    {
        public IList<Data> DataRow { get; set; }
        public GroupRow DataGroupRow { get; set; }
        public Data SumRow { get; set; }
    }
}
