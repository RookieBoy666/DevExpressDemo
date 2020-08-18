namespace DXApplication.Page
{
    public class PageData
    {
        private int pageIndex;
        private int totalPage;
        private int pageSize;

        public int PageIndex { set { pageIndex = value; } get { if (pageIndex < 1) pageIndex = 1; return pageIndex; } }
        public int TotalCount { set; get; }
        public int PageSize { set { pageSize = value; } get { if (pageSize < 1) pageSize = 20; return pageSize; } }


        public int TotalPage
        {
            set { this.totalPage = value; }
            get
            {
                if (this.TotalCount > 0)
                {
                    this.totalPage = (TotalCount / PageSize) + 1;
                }
                return totalPage;
            }
        }

    }
}
