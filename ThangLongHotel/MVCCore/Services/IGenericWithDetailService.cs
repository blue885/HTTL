﻿using MVCModel;
using MVCDTO;

namespace MVCCore.Services
{
    public interface IGenericWithDetailService<TEntity, TEntityDetail, TDto, TPrimitiveDto, TDtoDetail> : IGenericService<TEntity, TDto, TPrimitiveDto>

        where TEntity : class, IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<TEntityDetail>, new()
        where TEntityDetail : class, IPrimitiveEntity, new()
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
        where TDtoDetail : class, IPrimitiveEntity

    {
        
    }
}
