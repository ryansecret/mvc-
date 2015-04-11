using Hsr.Data.Interface;
using Hsr.Model.Models;
using Hsr.Model.Models.ViewModel;
using Hsr.Models;
using Hsr.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hsr.Service
{
    public class OrganizeInfoService : IOrganizeInfoService
    {
        private readonly IRepository<OrganizeInfo> _organizeInfoRepository;
        public OrganizeInfoService(IRepository<OrganizeInfo> organizeInfoRepository)
        {
            this._organizeInfoRepository = organizeInfoRepository;
        }
        public List<OrganizeInfo> GetOrganizeInfoByPId(int pid)
        {
            List<OrganizeInfo> list = new List<OrganizeInfo>();
            if (pid == 0)
                return new List<OrganizeInfo>();
            decimal? Pid = Convert.ToDecimal(pid);
            list = _organizeInfoRepository.Table.Where(s => s.OrgPid == Pid).ToList();
            return list;
        }
        public OrganizeInfo GetOrganizeInfoByID(int id)
        {
            return _organizeInfoRepository.GetById(id);
        }
        public bool CreateOrganizeInfo(OrganizeInfo model)
        {
            return _organizeInfoRepository.Insert(model);
        }
        public bool DeleteOrganizeInfo(OrganizeInfo model)
        {
            return _organizeInfoRepository.Delete(model);
        }
        public bool EditOrganizeInfo(OrganizeInfo model)
        {
            return _organizeInfoRepository.Update(model);
        }
        public OrganizeInfoVm GetOrganizeInfoTreeItems()
        {
            OrganizeInfoVm viewmodel = new OrganizeInfoVm();
            List<TreeNode> nodes = new List<TreeNode>();
            List<OrganizeInfo> list = new List<OrganizeInfo>();
            list = _organizeInfoRepository.Table.Where(x => x.Isenabled == 1).ToList();
            List<OrganizeInfo> returnlist = new List<OrganizeInfo>();
            if (list.Count > 0)
            {
               
                Dictionary<int, OrganizeInfo[]> dictForPid = list.GroupBy(p => p.OrgPid)
        .ToDictionary(g => Convert.ToInt32(g.Key), g => g.Select(p => p).ToArray());
                OrganizeInfo rootDefine = list.First(p => p.OrgPid == 0);
                returnlist = list.Where(x => x.OrgPid == rootDefine.OrgId).ToList();
                Queue<TreeNode> queue = new Queue<TreeNode>();
                TreeNode rootTreeNode = ParseToTreeNode(rootDefine);

                queue.Enqueue(rootTreeNode);
                while (queue.Count > 0)
                {
                    TreeNode curNode = queue.Dequeue();
                    if (dictForPid.ContainsKey(curNode.NodeID))
                    {
                        OrganizeInfo[] childNotes = dictForPid[curNode.NodeID];
                        foreach (OrganizeInfo property in childNotes)
                        {
                            TreeNode node = ParseToTreeNode(property);
                            curNode.Children.Add(node);

                            queue.Enqueue(node);
                        }
                    }
                }
                //viewmodel.node = rootTreeNode;
                //viewmodel.returnlist = returnlist;
                return viewmodel;
            }
            return new OrganizeInfoVm() {  };
        }
        public TreeNode GetOrganizeInfoTree(int pid)
        {
            List<TreeNode> nodes = new List<TreeNode>();
            List<OrganizeInfo> list = new List<OrganizeInfo>();
            list = _organizeInfoRepository.Table.Where(x => x.Isenabled == 1).ToList();
            if (list.Count > 0)
            {
                Dictionary<int, OrganizeInfo[]> dictForPid = list.GroupBy(p => p.OrgPid)
        .ToDictionary(g => Convert.ToInt32(g.Key), g => g.Select(p => p) .ToArray());
                OrganizeInfo rootDefine = new OrganizeInfo();
                if (pid == 0) {
                     rootDefine = list.First(p => p.OrgPid == 0);
                }
                else {
                    if (list.Where(p => p.OrgPid == pid).ToList().Count > 0) { 
                    rootDefine = list.First(p => p.OrgPid == pid);}
                }
              
                Queue<TreeNode> queue = new Queue<TreeNode>();
                TreeNode rootTreeNode = ParseToTreeNode(rootDefine);
                 if (rootDefine.OrgId != null) { 
                queue.Enqueue(rootTreeNode);
                while (queue.Count > 0)
                {
                    TreeNode curNode = queue.Dequeue();
                    if (dictForPid.ContainsKey(curNode.NodeID))
                    {
                        OrganizeInfo[] childNotes = dictForPid[curNode.NodeID];
                        foreach (OrganizeInfo property in childNotes)
                        {
                            TreeNode node = ParseToTreeNode(property);
                            curNode.Children.Add(node);

                            queue.Enqueue(node);
                        }
                    }
                }
                }
                return rootTreeNode;
            }
            return new TreeNode();
        }
        private TreeNode ParseToTreeNode(OrganizeInfo model)
        {
            TreeNode node = new TreeNode();
            node.NodeID = Convert.ToInt32(model.OrgId);
            node.NodeName = model.OrgName;
            node.ParentID = Convert.ToInt32(model.OrgPid);
            node.Children = new List<TreeNode>();
            return node;
        }
    }
}
