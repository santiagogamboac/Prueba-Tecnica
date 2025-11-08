using AutoMapper;
using Backend.Application.Domain.Common;
using Backend.Application.Models.ViewModels;

namespace Backend.Application.Helpers
{
    public class PaginationHelper
    {
        private readonly IMapper _mapper;
        public PaginationHelper(IMapper mapper) => _mapper = mapper;

        public PaginationVm<TVm> Paginate<TEntity, TVm>(IReadOnlyList<TEntity> rows, int totalRows, int pageSize, int pageIndex)
            where TEntity : Entity
            where TVm : class
        {
            var rounded = Math.Ceiling(Convert.ToDecimal(totalRows) / Convert.ToDecimal(pageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<TVm>>(rows);
            var pagination = new PaginationVm<TVm>
            {
                Data = data,
                Count = totalRows,
                PageCount = totalPages,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            return pagination;
        }
        public PaginationVm<TVm> PaginateClass<TVm>(IReadOnlyList<TVm> rows, int totalRows, int pageSize, int pageIndex)
            where TVm : class
        {
            var rounded = Math.Ceiling(Convert.ToDecimal(totalRows) / Convert.ToDecimal(pageSize));
            var totalPages = Convert.ToInt32(rounded);

            var data = _mapper.Map<IReadOnlyList<TVm>>(rows);
            var pagination = new PaginationVm<TVm>
            {
                Data = data,
                Count = totalRows,
                PageCount = totalPages,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            return pagination;
        }
    }
}
