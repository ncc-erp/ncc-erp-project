using Abp.Application.Services;
using NccCore.IoC;
using System.Collections.Generic;
using System.Linq;

namespace ProjectManagement.Services.Common
{
    public class CommonManager : ApplicationService
    {
        private readonly IWorkScope _workScope;

        public CommonManager(IWorkScope workScope)
        {
            _workScope = workScope;
        }

        public List<long> GetAllNodeAndLeafIdById(long Id, List<Entities.ProcessCriteria> list, bool isGetParent = false)
        {
            var listIds = new List<long>();
            var PC = list.Where(x => x.Id == Id).FirstOrDefault();
            if (PC.IsLeaf || isGetParent)
            {
                listIds.AddRange(GetAllParentId(PC.Id, list));
            }
            if (PC.IsLeaf)
            {
                listIds.Add(PC.Id);
            }
            else
            {
                listIds.Add(PC.Id);
                var listPCs = list.Where(x => x.ParentId == Id).ToList();
                foreach (var child in listPCs)
                {
                    listIds.AddRange(GetAllChildId(child.Id, list));
                }
            }
            return listIds;
        }

        public List<long> GetAllParentNode(long Id, List<Entities.ProcessCriteria> list, bool isGetParent = false)
        {
            var listIds = new List<long>();
            var PC = list.Where(x => x.Id == Id).FirstOrDefault();
            if (PC.IsLeaf && isGetParent)
            {
                listIds.AddRange(GetAllParentId(PC.Id, list));
            }
            if (PC.IsLeaf)
            {
                listIds.Add(PC.Id);
            }

            return listIds;
        }

        public List<long> GetAllParentId(long Id, List<Entities.ProcessCriteria> list)
        {
            var result = new List<long>();
            var item = list.Where(x => x.Id == Id).FirstOrDefault();
            if (item!=null && item.ParentId.HasValue)
            {
                result.AddRange(GetAllParentId((long)item.ParentId, list));
            }
            result.Add((long)item.Id);
            return result;
        }

        public List<long> GetAllNodeAndLeafIdByProcessCriteriaId(long processCriteriaId, List<Entities.ProjectProcessCriteriaResult> list, bool isGetParent = false)
        {
            var listIds = new List<long>();
            var PC = list.Where(x => x.ProcessCriteriaId == processCriteriaId).FirstOrDefault();
            if (PC.ProcessCriteria.IsLeaf && isGetParent)
            {
                listIds.AddRange(GetAllParentIdOfProcessCriteria(PC.ProcessCriteriaId, list));
            }
            if (PC.ProcessCriteria.IsLeaf)
            {
                listIds.Add(PC.ProcessCriteriaId);
            }
            else
            {
                listIds.Add(PC.ProcessCriteriaId);
                var listPCs = list.Where(x => x.ProcessCriteria.ParentId == processCriteriaId).ToList();
                foreach (var child in listPCs)
                {
                    listIds.AddRange(GetAllNodeAndLeafIdByProcessCriteriaId(child.ProcessCriteriaId, list));
                }
            }
            return listIds;
        }

        public List<long> GetAllParentIdOfProcessCriteria(long processCriteriaId, List<Entities.ProjectProcessCriteriaResult> list)
        {
            var result = new List<long>();
            var item = list.Where(x => x.ProcessCriteriaId == processCriteriaId).FirstOrDefault();
            if (item.ProcessCriteria.ParentId.HasValue)
            {
                result.AddRange(GetAllParentIdOfProcessCriteria((long)item.ProcessCriteria.ParentId, list));
            }
            result.Add((long)item.Id);
            return result;
        }

        public List<long> GetAllChildId(long Id, List<Entities.ProcessCriteria> list)
        {
            var result = new List<long>();
            result.Add(Id);
            var items = list.Where(x => x.ParentId == Id).Select(x => x.Id).ToList();
            if (items.Count > 0) { result.AddRange(items); }
            items.ForEach(x =>
            {
                result.AddRange(GetAllChildId(x, list));
            });
            return result;
        }
    }
}