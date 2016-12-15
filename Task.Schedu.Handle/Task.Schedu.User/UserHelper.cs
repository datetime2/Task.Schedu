using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Task.Schedu.Data;
using Task.Schedu.Model;
using Task.Schedu.Utility;

namespace Task.Schedu.User
{
    public class UserHelper
    {
        public static JsonBaseModel<List<Users>> Query(QueryCondition condition)
        {
            JsonBaseModel<List<Users>> result = new JsonBaseModel<List<Users>>();
            if (string.IsNullOrEmpty(condition.SortField))
            {
                condition.SortField = "CreateOn";
                condition.SortOrder = "DESC";
            }
            Hashtable ht = Pagination.QueryBase<Users>("SELECT * FROM t_Users", condition);
            result.Result = ht["data"] as List<Users>;
            result.TotalCount = Convert.ToInt32(ht["total"]);
            result.TotalPage = result.CalculateTotalPage(condition.PageSize, result.TotalCount.Value, condition.IsPagination);
            return result;
        }
    }
}
