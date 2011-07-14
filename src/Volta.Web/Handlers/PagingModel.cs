namespace Volta.Web.Handlers
{
    public class PagingModel
    {
        public PagingModel(int totalRecords, int pageSize, int currentPage)
        {
            TotalRecords = totalRecords;
            RecordStart = ((currentPage - 1) * pageSize) + 1;
            RecordEnd = RecordStart + pageSize > totalRecords ? totalRecords : (RecordStart + pageSize) - 1;
            SelectedRecords = RecordEnd - RecordStart + 1;
            NotTheFirstPage = currentPage > 1;
            NotTheLastPage = currentPage * pageSize < totalRecords;
            InTheMiddle = NotTheFirstPage && NotTheLastPage;
            PreviousPage = NotTheFirstPage ? currentPage - 1 : 0;
            NextPage = NotTheLastPage ? currentPage + 1 : 0;
        }

        public int TotalRecords { get; private set; }
        public int RecordStart { get; private set; }
        public int RecordEnd { get; private set; }
        public int SelectedRecords { get; private set; }
        public bool NotTheFirstPage { get; private set; }
        public bool InTheMiddle { get; private set; }
        public bool NotTheLastPage { get; private set; }
        public int PreviousPage { get; private set; }
        public int NextPage { get; private set; }
    }
}