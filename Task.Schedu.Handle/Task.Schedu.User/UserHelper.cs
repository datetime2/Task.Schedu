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
        private const string InsertUser = @"INSERT INTO t_Users (UserId,UserName,PassWord,PassSalt,RealName,Tel,Email,Status,CreateOn,Remark,ModifyOn,LastLogTime,LastLogIP) VALUES(@UserId,@UserName,@PassWord,@PassSalt,@RealName,@Tel,@Email,@Status,@CreateOn,@Remark,@ModifyOn,@LastLogTime,@LastLogIP)";
        private const string UpdateUser = @"UPDATE t_Users SET RealName=@RealName,Tel=@Tel,Email=@Email,Status=@Status,Remark=@Remark,ModifyOn=@ModifyOn WHERE UserId=@UserId";
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
        /// <summary>
        /// 修改用户状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="Status"></param>
        public static void UpdUserStatus(string userId, int Status)
        {
            SQLHelper.ExecuteNonQuery("UPDATE t_Users SET Status=@Status WHERE UserId=@UserId", new { UserId = userId, Status = Status });
        }
        public static void DeleteById(string userId)
        {
            SQLHelper.ExecuteNonQuery("DELETE FROM t_Users WHERE UserId=@UserId", new { UserId = userId });
        }
        public static JsonBaseModel<string> SaveUser(Users value)
        {
            JsonBaseModel<string> result = new JsonBaseModel<string>();
            result.HasError = true;
            if (value == null)
            {
                result.Message = "参数空异常";
                return result;
            }
            try
            {
                if (!string.IsNullOrEmpty(value.UserId))
                {
                    value.ModifyOn = DateTime.Now;
                    SQLHelper.ExecuteNonQuery(UpdateUser, value);
                }
                else
                {
                    var slat = Guid.NewGuid().ToString();
                    value.UserId = Guid.NewGuid().ToString();
                    value.CreateOn = DateTime.Now;
                    value.PassSalt = slat;
                    value.PassWord = DESEncrypt.Md5Hash(DESEncrypt.Md5Hash(value.PassWord) + slat);
                    SQLHelper.ExecuteNonQuery(InsertUser, value);
                }
                result.HasError = false;
                result.Result = value.UserId;
            }
            catch (Exception ex)
            {
                result.HasError = true;
                result.Message = ex.Message;
            }
            return result;
        }
        public static Users GetById(string userId)
        {
            return SQLHelper.Single<Users>("SELECT * FROM t_Users WHERE UserId=@UserId", new { UserId = userId });
        }

        public static JsonBaseModel<Users> Login(string userName, string passWord)
        {
            JsonBaseModel<Users> result = new JsonBaseModel<Users>();
            result.HasError = true;
            var user = SQLHelper.Single<Users>("SELECT * FROM t_Users WHERE UserName=@UserName", new { UserName = userName });
            if (user != null)
            {
                if (user.Status == 0)
                {
                    var logPass = DESEncrypt.Md5Hash(DESEncrypt.Md5Hash(passWord) + user.PassSalt);//实际应用passWord 应传输MD5串
                    if (user.PassWord.Equals(logPass))
                    {
                        result.HasError = false;
                        result.Result = user;
                    }
                    else
                        result.Message = "账号或密码错误,请重试!";
                }
                else
                    result.Message = "账号已被禁用,请联系管理员!";
            }
            else
                result.Message = "账号或密码错误,请重试!";
            return result;
        }
    }
}
