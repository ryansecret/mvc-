//Contributor : MVCContrib

using System;
using System.Collections;
using System.Collections.Generic;

namespace MVC.Controls.Paging
{
    public  class BasePageableModel : IPageableModel
    {
        private int _pageNumber=1;
        private int _pageSize=15;
         
        #region Methods

        public IEnumerator GetEnumerator()
        {
            return new List<string>().GetEnumerator();
        }

        public virtual void LoadPagedList<T>(IPagedList<T> pagedList)
        {
            FirstItem = (pagedList.PageIndex * pagedList.PageSize) + 1;
            HasNextPage = pagedList.HasNextPage;
            HasPreviousPage = pagedList.HasPreviousPage;
            LastItem = Math.Min(pagedList.TotalItemCount, ((pagedList.PageIndex * pagedList.PageSize) + pagedList.PageSize));
            PageNumber = pagedList.PageIndex + 1;
            PageSize = pagedList.PageSize;
            TotalItems = pagedList.TotalItemCount;
            TotalPages = pagedList.PageCount;
        }

        #endregion

        #region Properties

        public int FirstItem { get; set; }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public int LastItem { get; set; }

        public int PageIndex
        {
            get
            { 
                if (PageNumber > 0)
                    return PageNumber - 1;
                else
                    return 0;
            }
        }

        public int PageNumber
        {
            get { return _pageNumber; }
            set { _pageNumber = value; }
        }

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        public int TotalItems { get; set; }

        public int TotalPages { get; set; }

        #endregion
    }
}