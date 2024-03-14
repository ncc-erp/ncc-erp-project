using Abp.Linq.Expressions;
using NccCore.Anotations;
using NccCore.DynamicFilter;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.Services.ProjectUserBill.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.Services.ProjectUserBill
{
    public static class ProjectUserBillQueryEx
    {
        public static IQueryable<ProjectManagement.Entities.ProjectUserBill> FilterByChargeStatus(this IQueryable<ProjectManagement.Entities.ProjectUserBill> qProjectUserBill, ChargeStatus chargeStatus)
        {

            if (chargeStatus == ChargeStatus.All)
                return qProjectUserBill;
            return qProjectUserBill.Where(x => x.isActive == (chargeStatus == ChargeStatus.IsCharge));
        }
        public static IQueryable<ProjectManagement.Entities.ProjectUserBill> FilterByChargeName(this IQueryable<ProjectManagement.Entities.ProjectUserBill> qProjectUserBill, List<string> chargeNameFilter)
        {

            if (chargeNameFilter == null || !chargeNameFilter.Any())
                return qProjectUserBill;
            return qProjectUserBill.Where(x => chargeNameFilter.Contains(x.AccountName));
        }
        public static IQueryable<ProjectManagement.Entities.ProjectUserBill> FilterByChargeRole(this IQueryable<ProjectManagement.Entities.ProjectUserBill> qProjectUserBill, List<string> chargeRoleFilter)
        {

            if (chargeRoleFilter == null || !chargeRoleFilter.Any())
                return qProjectUserBill;
            return qProjectUserBill.Where(x => chargeRoleFilter.Contains(x.BillRole));
        }
        public static IQueryable<ProjectManagement.Entities.ProjectUserBill> FilterByChargeType(this IQueryable<ProjectManagement.Entities.ProjectUserBill> qProjectUserBill, ChargeType chargeType)
        {

            if (chargeType == ChargeType.All)
                return qProjectUserBill;
            return qProjectUserBill.Where(x => x.ChargeType == chargeType);
        }
    }
}
