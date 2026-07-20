namespace LarksWebApp.DTOs.PaginationDTO
{
    public class DataTableResponse<T>
    {
        public string Draw { get; set; }

        public int RecordsFiltered { get; set; }

        public int RecordsTotal { get; set; }

        public T Data { get; set; }
    }
}
