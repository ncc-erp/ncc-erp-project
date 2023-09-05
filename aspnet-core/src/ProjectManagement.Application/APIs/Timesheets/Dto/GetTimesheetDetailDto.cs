using Abp.Application.Services.Dto;
using NccCore.Anotations;
using NccCore.Extension;
using NccCore.Paging;
using ProjectManagement.APIs.ProjectUserBills.Dto;
using ProjectManagement.APIs.TimeSheetProjectBills.Dto;
using ProjectManagement.Services.Finance.Dto;
using ProjectManagement.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using static ProjectManagement.Constants.Enum.ProjectEnum;

namespace ProjectManagement.APIs.Timesheets.Dto
{
    public class GetTimesheetDetailDto : EntityDto<long>
    {
        public long ProjectId { get; set; }
        public long TimesheetId { get; set; }
        [ApplySearchAttribute]
        public string ProjectName { get; set; }
        public string ProjectCode { get; set; }
        public long PmId { get; set; }
        [ApplySearchAttribute]
        public string PmEmailAddress { get; set; }
        public string PmUserName { get; set; }
        [ApplySearchAttribute]
        public string PmFullName { get; set; }
        public string PmAvatarPath { get; set; }
        public string PmAvatarFullPath => FileUtils.FullFilePath(PmAvatarPath);
        public UserType PmUserType { get; set; }
        public Branch PmBranch { get; set; }
        public long? ClientId { get; set; }
        [ApplySearchAttribute]
        public string ClientName { get; set; }
        public string FilePath { get; set; }
        public string File => FileUtils.GetFileName(FilePath);
        public string HistoryFile { get; set; }
        public List<TimesheetProjectBillInfoDto> ProjectBillInfomation { get; set; }
        public string Note { get; set; }
        public PMReportProjectStatus IsSendReport { get; set; }
        public bool HasFile { get; set; }
        public bool? IsComplete { get; set; }
        public bool RequireTimesheetFile { get; set; }

        public string ProjectCurrency { get; set; }
        public ChargeType? ProjectChargeType { get; set; }
        //public string? ParentName { get; set; }
        //public long? ParentInvoiceId { get; set; }
        public IEnumerable<SubInvoiceDto> SubInvoices { get; set; }

        public string Currency
        {
            get
            {
                var currency = this.ProjectBillInfomation.Select(x => x.Currency).FirstOrDefault();
                if (currency == default)
                {
                    currency = this.ProjectCurrency;
                }
                return currency;
            }
        }
        public ChargeType? ChargeType
        {
            get
            {
                var chargeType = this.ProjectBillInfomation.Select(x => x.ChargeType).FirstOrDefault();
                if (chargeType == default)
                {
                    chargeType = this.ProjectChargeType;
                }
                return chargeType;
            }
        }
        public long InvoiceNumber { get; set; }
        public float WorkingDay { get; set; }
        public float Discount { get; set; }
        public float TransferFee { get; set; }
        [ApplySearchAttribute]
        public string ClientCode { get; set; }
        public string PmBranchColor { get; set; }
        public string PmBranchDisplayName { get; set; }
        public double TotalAmountProjectBillInfomation => GetIntoMoneyProject();
        public double RoundTotalAmountProjectBillInfomation => CommonUtil.Round(TotalAmountProjectBillInfomation);

        private double GetIntoMoneyProject()
        {
            if (this.ProjectBillInfomation == null || ProjectBillInfomation.IsEmpty())
            {
                return 0;
            }

            double amount = ProjectBillInfomation.Sum(x => x.Amount);

            if (amount > 0)
            {
                return (100 - Discount) / 100 * amount + TransferFee;
            }

            return amount;
        }

        public bool IsMainProjectInvoice => !MainProjectId.HasValue;
        public long? MainProjectId { get; set; }
        public string MainProjectName { get; set; }
        public List<IdNameDto> SubProjects { get; set; }
        public List<string> SubProjectNames => SubProjects != null ? SubProjects.Select(s => s.Name).ToList() : null;
        public List<long> SubProjectIds => SubProjects != null ? SubProjects.Select(s => s.Id).ToList() : null;

        public byte PaymentDueBy { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }

        public ProjectType ProjectType { get; set; }
        public ProjectStatus ProjectStatus { get; set; }
        public string CloseTime { get; set; }
    }

    public class IdNameDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class ResultTimesheetDetail
    {
        public GridResult<GetTimesheetDetailDto> ListTimesheetDetail { get; set; }
        public List<TotalMoneyByCurrencyDto> ListTotalAmountByCurrency { get; set; }
    }
}