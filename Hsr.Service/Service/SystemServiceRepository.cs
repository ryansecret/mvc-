using Hsr.Data.Interface;
using Hsr.Model.Models;
using Hsr.Model.Models.ViewModel;
using Hsr.Service;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Hsr.Service
{
    public class SystemServiceRepository : ISystemServiceRepository
    {
        private readonly IRepository<Sysrole> _sysroleRepository;
        public SystemServiceRepository(IRepository<Sysrole> sysroleRepository)
        {
            this._sysroleRepository = sysroleRepository;
        }
        public List<Sysrole> GetRoleByPId(int pid)
        {
            List<Sysrole> list = new List<Sysrole>();
            if (pid == 0)
                return new List<Sysrole>();
            decimal? Pid = Convert.ToDecimal(pid);
            list = _sysroleRepository.Table.Where(s => s.RolePid == Pid).ToList();
            return list;
        }
        public Sysrole GetRoleByID(int id)
        {
            return _sysroleRepository.GetById(id);
        }
        public bool CreateRole(Sysrole model)
        {
            return _sysroleRepository.Insert(model);
        }
        public bool DeleteRole(Sysrole model)
        {
            return _sysroleRepository.Delete(model);
        }
        public bool EditRole(Sysrole model)
        {
            return _sysroleRepository.Update(model);
        }
        public RoleVm GetRoleTreeItems()
        {
            RoleVm viewmodel = new RoleVm();
            List<TreeNode> nodes = new List<TreeNode>();
            List<Sysrole> list = new List<Sysrole>();
            list=_sysroleRepository.Table.Where(x=>x.Isenabled==1).ToList();
            List<Sysrole> returnlist = new List<Sysrole>();
            if (list.Count > 0)
            {
               
                Dictionary<int, Sysrole[]> dictForPid = list.GroupBy(p => p.RolePid)
        .ToDictionary(g => Convert.ToInt32(g.Key), g => g.Select(p => p).OrderBy(p => p.OrderNum).ToArray());
                Sysrole rootDefine = list.First(p => p.RolePid == 0);
                returnlist = list.Where(x => x.RolePid == rootDefine.RoleId).ToList();
                Queue<TreeNode> queue = new Queue<TreeNode>();
                TreeNode rootTreeNode = ParseToTreeNode(rootDefine);

                queue.Enqueue(rootTreeNode);
                while (queue.Count > 0)
                {
                    TreeNode curNode = queue.Dequeue();
                    if (dictForPid.ContainsKey(curNode.NodeID))
                    {
                        Sysrole[] childNotes = dictForPid[curNode.NodeID];
                        foreach (Sysrole property in childNotes)
                        {
                            TreeNode node = ParseToTreeNode(property);
                            curNode.Children.Add(node);

                            queue.Enqueue(node);
                        }
                    }
                }
                viewmodel.node = rootTreeNode;
                viewmodel.returnlist = returnlist;
                return viewmodel;
            }
            return new RoleVm() { node = new TreeNode(), returnlist = new List<Sysrole>() };
        }
        public TreeNode GetRoleTree()
        {
            List<TreeNode> nodes = new List<TreeNode>();
            List<Sysrole> list = new List<Sysrole>();
            list = _sysroleRepository.Table.Where(x => x.Isenabled == 1).ToList();
            if (list.Count > 0)
            {
                Dictionary<int, Sysrole[]> dictForPid = list.GroupBy(p => p.RolePid)
        .ToDictionary(g => Convert.ToInt32(g.Key), g => g.Select(p => p).OrderBy(p => p.OrderNum).ToArray());
                Sysrole rootDefine = list.First(p => p.RolePid == 0);
                Queue<TreeNode> queue = new Queue<TreeNode>();
                TreeNode rootTreeNode = ParseToTreeNode(rootDefine);

                queue.Enqueue(rootTreeNode);
                while (queue.Count > 0)
                {
                    TreeNode curNode = queue.Dequeue();
                    if (dictForPid.ContainsKey(curNode.NodeID))
                    {
                        Sysrole[] childNotes = dictForPid[curNode.NodeID];
                        foreach (Sysrole property in childNotes)
                        {
                            TreeNode node = ParseToTreeNode(property);
                            curNode.Children.Add(node);

                            queue.Enqueue(node);
                        }
                    }
                }
                return rootTreeNode;
            }
            return new TreeNode();
        }
        private TreeNode ParseToTreeNode(Sysrole model)
        {
            TreeNode node = new TreeNode();
            node.NodeID = Convert.ToInt32(model.RoleId);
            node.NodeName = model.RoleName;
            node.ParentID = Convert.ToInt32(model.RolePid);
            node.Children = new List<TreeNode>();
            return node;
        }
    }
}
