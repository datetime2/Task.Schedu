using Task.Schedu.Utility;
using Nancy;
using Nancy.ModelBinding;
using System;
using Task.Schedu.Quarzt;
using Task.Schedu.Model;

namespace Task.Schedu.Web.Modules
{
    public class TaskModule : BaseModule
    {
        public TaskModule() : base("Task")
        {
            //任务列表
            Get["/"] = r =>
            {
                return View["Grid"];
            };
            //任务编辑界面
            Get["/Edit"] = r =>
            {
                return View["Edit"];
            };

            #region "取数接口API"

            //立即运行一次任务
            Get["/Run/{Id}"] = r =>
            {
                //取出单条记录数据
                string TaskId = r.Id;
                JsonBaseModel<string> result = new JsonBaseModel<string>();
                try
                {
                    TaskHelper.RunById(TaskId);
                }
                catch (Exception ex)
                {
                    result.HasError = true;
                    result.Message = ex.Message;
                }
                return Response.AsJson(result);
            };

            Get["/GetById/{Id}"] = r =>
            {
                JsonBaseModel<TaskUtil> result = new JsonBaseModel<TaskUtil>();
                try
                {
                    //取出单条记录数据
                    string TaskId = r.Id;
                    result.Result = TaskHelper.GetById(TaskId);
                }
                catch (Exception ex)
                {
                    result.HasError = true;
                    result.Message = ex.Message;
                }
                return Response.AsJson(result);
            };

            //列表查询接口
            Post["/PostQuery"] = r =>
            {
                QueryCondition condition = this.Bind<QueryCondition>();
                return Response.AsJson(TaskHelper.Query(condition));
            };

            //保存数据
            Post["/"] = r =>
            {
                TaskUtil TaskUtil = this.Bind<TaskUtil>();
                return Response.AsJson(TaskHelper.SaveTask(TaskUtil));
            };
            //更新数据
            Put["/"] = r =>
            {
                TaskUtil TaskUtil = this.Bind<TaskUtil>();
                return Response.AsJson(TaskHelper.SaveTask(TaskUtil));
            };
            //删除任务接口
            Delete["/{Id}"] = r =>
            {
                JsonBaseModel<string> result = new JsonBaseModel<string>();
                try
                {
                    string TaskId = r.Id;
                    TaskHelper.DeleteById(TaskId);
                }
                catch (Exception ex)
                {
                    result.HasError = true;
                    result.Message = ex.Message;
                }
                return Response.AsJson(result);
            };

            //更新任务运行状态
            Put["/{Id}/{Status:int}"] = r =>
            {
                JsonBaseModel<string> result = new JsonBaseModel<string>();
                try
                {
                    TaskStatus Status = Enum.ToObject(typeof(TaskStatus), r.Status);
                    string TaskId = r.Id;
                    TaskHelper.UpdateTaskStatus(TaskId, Status);
                }
                catch (Exception ex)
                {
                    result.HasError = true;
                    result.Message = ex.Message;
                }
                return Response.AsJson(result);
            };
            #endregion
        }
    }
}