using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.Common
{
    public class ApiResultError<T> : ApiResult<T>
    {
        public string[] ValidationErrors { get; set; }

        public ApiResultError()
        {
            IsSuccessed = false;
        }

        public ApiResultError(string message) //Dung de truyen loi bussiness
        {
            IsSuccessed = false;
            Message = message;
        }

        public ApiResultError(string[] validationErrors) //Dung de truyen ve danh sach cac loi validation
        {
            IsSuccessed = false;
            ValidationErrors = validationErrors;
        }
    }
}