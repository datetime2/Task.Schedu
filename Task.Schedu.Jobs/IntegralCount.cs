using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task.Schedu.Data;
using Task.Schedu.Utility;
using Dapper;
namespace Task.Schedu.Jobs
{
    /// <summary>
    /// 积分统计
    /// </summary>
    [DisallowConcurrentExecution]
    class IntegralCount : DbAccess<dynamic>, IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var MemberIntegral = FindBy((client) =>
            {
                return client.Query(@"select * from MemberIntegral a  where a.MemberId in  (select MemberId from MemberIntegral group by MemberId having count(*) > 1) 
and Id in (select min(Id) from MemberIntegral group by  MemberId  having count(*)>1)");
            }, SysConfig.MainConnect);
            if (MemberIntegral.Any())
            {
                foreach (var MemberIntegral_ in MemberIntegral)
                {
                    try
                    {
                        using (TransactionScope scope = new TransactionScope())
                        {
                            // 合并积分
                            Himall_MemberIntegral updateModel = entity.Himall_MemberIntegral.Where(w => w.Id == MemberIntegral_.Id).FirstOrDefault();
                            if (updateModel != null)
                            {
                                updateModel.HistoryIntegrals = entity.Himall_MemberIntegral.Where(w => w.MemberId == MemberIntegral_.MemberId).Sum(s => s.HistoryIntegrals);
                                updateModel.AvailableIntegrals = entity.Himall_MemberIntegral.Where(w => w.MemberId == MemberIntegral_.MemberId).Sum(s => s.AvailableIntegrals);
                            }
                            // 删除其他重复记录
                            List<Himall_MemberIntegral> delMemberIntegral = entity.Himall_MemberIntegral.Where(w => w.Id != MemberIntegral_.Id && w.MemberId == MemberIntegral_.MemberId).ToList();
                            entity.Himall_MemberIntegral.RemoveRange(delMemberIntegral);
                            entity.SaveChanges();
                            scope.Complete();
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("合并重复的用户积分信息失败：" + ex.Message + "\r\n" + ex.StackTrace);
                    }
                }
            }

            throw new NotImplementedException();
        }
    }
}
