using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Common
{
    public class ApiResultSuccess<T> : ApiResult<T>
    {
        public ApiResultSuccess(T resultObj)
        {
            IsSuccessed = true;
            ResultObj = resultObj;
        }

        public ApiResultSuccess()
        {
            IsSuccessed = true;        
        }
    }
}
