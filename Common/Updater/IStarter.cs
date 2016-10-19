using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mn.NewsCms.Common
{

    //har bakhsh ejrayi consol bayad in interface ra implament konad
    public interface IStarter<T>
    {
        void Start(T inputParams);
    }
    public interface IStarter
    {
        void Start(params object[] inputParams);
    }
}
