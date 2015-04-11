#region

using System.Collections.Generic;

#endregion

namespace Hsr.Model.Models
{
    public class TreeNode
    {
        /// <summary>
        ///     属性ID
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        ///     属性Name
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        ///     父节点ID
        /// </summary>
        public int ParentID { get; set; }


        /// <summary>
        ///     是否已选择
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        ///     是否有备注
        /// </summary>
        public bool HasText { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        ///     子节点
        /// </summary>
        public List<TreeNode> Children { get; set; }

        /// <summary>
        ///     是否是叶节点
        /// </summary>
        public bool IsLeaf
        {
            get { return Children == null || Children.Count == 0; }
        }
    }
}