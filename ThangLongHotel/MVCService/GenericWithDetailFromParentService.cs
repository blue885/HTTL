using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using MVCModel;
using MVCDTO;
using MVCCore.Repositories;
using MVCCore.Services;



namespace MVCService
{
    public enum SaveParentOption
    {
        Undo = -1,
        Update = 1
    }

    public class GenericWithDetailFromParentService<TEntity, TEntityDetail, TDto, TPrimitiveDto, TDtoDetail> : GenericWithDetailService<TEntity, TEntityDetail, TDto, TPrimitiveDto, TDtoDetail>, IGenericWithDetailFromParentService<TEntity, TEntityDetail, TDto, TPrimitiveDto, TDtoDetail>

        where TEntity : class, IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<TEntityDetail>, new()
        where TEntityDetail : class, IPrimitiveEntity, new()
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
        where TDtoDetail : class, IPrimitiveEntity
    {
        private readonly IGenericWithDetailRepository<TEntity, TEntityDetail> genericWithDetailRepository;

        private readonly string functionNameGetViewDetails;
        private readonly string functionNameSaveParent;
        private readonly string functionNameSaveConflict;

        public GenericWithDetailFromParentService(IGenericWithDetailRepository<TEntity, TEntityDetail> genericWithDetailRepository, string functionNameGetViewDetails, string functionNameSaveParent, string functionNameSaveConflict)
            : base(genericWithDetailRepository)
        {
            this.genericWithDetailRepository = genericWithDetailRepository;

            this.functionNameGetViewDetails = functionNameGetViewDetails;
            this.functionNameSaveParent = functionNameSaveParent;
            this.functionNameSaveConflict = functionNameSaveConflict;
        }

        protected override TEntity SaveMe(TDto dto)
        {
            TEntity entity = base.SaveMe(dto);

            this.SaveParent(entity, SaveParentOption.Update);

            return entity;
        }

        protected override void SaveUndo(TDto dto, TEntity entity, bool IsDelete)
        {
            this.SaveParent(entity, SaveParentOption.Undo);

            base.SaveUndo(dto, entity, IsDelete);
        }

        protected virtual void SaveParent(TEntity entity, SaveParentOption saveParentOption)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("EntityID", entity.GetID()), new ObjectParameter("SaveParentOption", (int)saveParentOption) };
            this.genericWithDetailRepository.ExecuteFunction(this.functionNameSaveParent, parameters);
        }

        public virtual ICollection<TEntityViewDetail> GetViewDetails<TEntityViewDetail>(params ObjectParameter[] parameters)
        {
            return this.genericWithDetailRepository.ExecuteFunction<TEntityViewDetail>(this.functionNameGetViewDetails, parameters);
        }


    }


}
