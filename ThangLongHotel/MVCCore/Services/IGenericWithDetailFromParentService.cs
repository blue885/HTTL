using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using MVCModel;
using MVCDTO;


namespace MVCCore.Services
{
    public interface IGenericWithDetailFromParentService<TEntity, TEntityDetail, TDto, TPrimitiveDto, TDtoDetail> : IGenericWithDetailService<TEntity, TEntityDetail, TDto, TPrimitiveDto, TDtoDetail>

        where TEntity : class, IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<TEntityDetail>, new()
        where TEntityDetail : class, IPrimitiveEntity, new()
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
        where TDtoDetail : class, IPrimitiveEntity
    {
        ICollection<TEntityViewDetail> GetViewDetails<TEntityViewDetail>(params ObjectParameter[] parameters);
    }
}
