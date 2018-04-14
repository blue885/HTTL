using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using MVCModel;
using MVCDTO;
using MVCCore.Repositories;
using MVCCore.Services;


namespace MVCService
{
    public class GenericService<TEntity, TDto, TPrimitiveDto> : IGenericService<TEntity, TDto, TPrimitiveDto>

        where TEntity : class, IPrimitiveEntity, IBaseEntity, new()
        where TDto : class, TPrimitiveDto
        where TPrimitiveDto : BaseDTO, IPrimitiveEntity, IPrimitiveDTO
    {

        private readonly IGenericRepository<TEntity> genericRepository;

        public GenericService(IGenericRepository<TEntity> genericRepository)
        {
            this.genericRepository = genericRepository;
        }


        public virtual TEntity GetByID(int id)
        {
            return this.genericRepository.GetByID(id);
        }


        public virtual bool Editable(TDto dto)
        {
            return true;
        }


        protected virtual bool TryValidateModel(TDto dto)
        {
            return true;
        }




        public virtual bool Save(TDto dto)
        {
            using (var dbContextTransaction = this.genericRepository.BeginTransaction())
            {
                try
                {
                    if (!this.TryValidateModel(dto)) throw new System.ArgumentException("Insufficient save", "Save validate");
                    if (!this.Editable(dto)) throw new System.ArgumentException("Insufficient save", "Uneditable");

                    TEntity entity = this.SaveMe(dto);

                    this.PostSaveValidate(entity);

                    dbContextTransaction.Commit();

                    dto.SetID(entity.GetID());

                    return true;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw ex;
                }
            }
        }

        public virtual bool Delete(int id)
        {
            if (id <= 0) return false;

            using (var dbContextTransaction = this.genericRepository.BeginTransaction())
            {
                try
                {
                    TEntity entity = this.genericRepository.GetByID(id);
                    TDto dto = Mapper.Map<TDto>(entity);

                    if (!this.TryValidateModel(dto)) throw new System.ArgumentException("Insufficient delete", "Save Delete");
                    if (!this.Editable(dto)) throw new System.ArgumentException("Insufficient delete", "Uneditable");

                    this.DeleteMaster(dto, entity);

                    this.genericRepository.SaveChanges();

                    this.PostSaveValidate(entity);

                    dbContextTransaction.Commit();

                    return true;
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw ex;
                }
            }
        }

        protected virtual TEntity SaveMe(TDto dto)
        {
            TEntity entity = this.SaveMaster(dto);

            if (this.genericRepository.IsDirty())
                entity.EditedDate = DateTime.Now;

            this.genericRepository.SaveChanges();

            return entity;
        }

        protected virtual TEntity SaveMaster(TDto dto)
        {
            TEntity entity;

            if (dto.GetID() > 0) //Edit existing ModelClass
            {
                entity = this.genericRepository.GetByID(dto.GetID());
                if (entity == null) throw new System.ArgumentException("", "Không tìm thấy dữ liệu. Dữ liệu cần điều chỉnh có thể đã bị xóa.");
            }
            else//New ModelClass
            {
                entity = new TEntity();
                entity.CreatedDate = DateTime.Now;

                this.genericRepository.Add(entity);
            }

            //Convert from DTOModel to Model
            Mapper.Map<TPrimitiveDto, TEntity>((TPrimitiveDto)dto, entity);

            entity.UserID = 1;
            entity.UserPositionID = 1;

            return entity;
        }


        protected virtual void DeleteMaster(TDto dto, TEntity entity)
        {
            this.genericRepository.Remove(entity);
        }

        protected virtual void PostSaveValidate(TEntity entity)
        {
        }




    }
}
