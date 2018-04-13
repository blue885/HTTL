using System.Linq;

using AutoMapper;

using MVCModel;
using MVCDTO;
using MVCCore.Repositories;
using MVCCore.Services;


namespace MVCService
{
    public class GenericWithDetailService<TEntity, TEntityDetail, TDto, TPrimitiveDto, TDtoDetail> : GenericService<TEntity, TDto, TPrimitiveDto>, IGenericWithDetailService<TEntity, TEntityDetail, TDto, TPrimitiveDto, TDtoDetail>

        where TEntity : class, IPrimitiveEntity, IBaseEntity, IBaseDetailEntity<TEntityDetail>, new()
        where TEntityDetail : class, IPrimitiveEntity, new()
        where TDto : TPrimitiveDto, IBaseDetailEntity<TDtoDetail>
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
        where TDtoDetail : class, IPrimitiveEntity
    {

        private readonly IGenericWithDetailRepository<TEntity, TEntityDetail> genericWithDetailRepository;

        public GenericWithDetailService(IGenericWithDetailRepository<TEntity, TEntityDetail> genericWithDetailRepository)
            : base(genericWithDetailRepository)
        {
            this.genericWithDetailRepository = genericWithDetailRepository;
        }


        protected override TEntity SaveMaster(TDto dto)
        {
            TEntity entity = base.SaveMaster(dto);

            if (dto.GetID() > 0) //Edit existing ModelClass
                this.SaveUndo(dto, entity, false);


            this.SaveDetail(dto, entity);

            return entity;
        }


        protected virtual void SaveDetail(TDto dto, TEntity entity)
        {
            if (dto.GetDetails() != null && dto.GetDetails().Count > 0)
                dto.GetDetails().Each(detailDTO =>
                {
                    TEntityDetail entityDetail;

                    if (detailDTO.GetID() <= 0 || (entityDetail = entity.GetDetails().First(detailModel => detailModel.GetID() == detailDTO.GetID())) == null)
                    {
                        entityDetail = new TEntityDetail();
                        entity.GetDetails().Add(entityDetail);
                    }

                    Mapper.Map<TDtoDetail, TEntityDetail>(detailDTO, entityDetail);
                });
        }

        protected virtual void SaveUndo(TDto dto, TEntity entity, bool IsDelete)
        {
            //Remove saved detail entity which is not in cusrrent dto details collection (The 'saved detail entity' is the entity which is saved in database, The dto details collection: is the new detail collection)
            if (entity.GetID() > 0 && entity.GetDetails().Count > 0)
                if (IsDelete || dto.GetDetails() == null || dto.GetDetails().Count == 0)
                    this.genericWithDetailRepository.RemoveRangeDetail(entity.GetDetails());
                else
                    entity.GetDetails().ToList()
                            .Where(detailModel => !dto.GetDetails().Any(detailDTO => detailDTO.GetID() == detailModel.GetID()))
                            .Each(deleted => this.genericWithDetailRepository.RemoveDetail(deleted)); //remove deleted details

        }


        protected override void DeleteMaster(TDto dto, TEntity entity)
        {
            this.SaveUndo(dto, entity, true);

            base.DeleteMaster(dto, entity);
        }

    }
}
