using System.Data;
using System.Data.Common;
namespace MeeyPage.Data
{
    public class DataSetAndOutputParam
    {
        public DataSet DataSetOutput { get; set; }
        public DbParameterCollection ParamOutput { get; set; }
    }
}
