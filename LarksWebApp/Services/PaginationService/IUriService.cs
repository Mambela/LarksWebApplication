using LarksWebApp.DTOs.PaginationDTO;

namespace LarksWebApp.Services.PaginationService
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
