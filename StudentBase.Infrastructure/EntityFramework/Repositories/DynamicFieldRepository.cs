using Microsoft.EntityFrameworkCore;
using StudentBase.Domain.Entities;
using StudentBase.Domain.Entities.Dynamic;
using StudentBase.Domain.Repositories;

namespace StudentBase.Infrastructure.EntityFramework.Repositories
{
    public class DynamicFieldRepository : IDynamicFieldRepository
    {
        private readonly AppDbContext _context;
        public DynamicFieldRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(DynamicField entity)
        {
            await _context.DynamicFields.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.DynamicFields.FindAsync(id);
            if (entity == null) return false;

            _context.DynamicFields.Remove(entity);
            await _context.SaveChangesAsync();
            return true;    
        }

        public async Task<IEnumerable<DynamicField>?> GetAllAsync()
        {
            return await _context.DynamicFields.ToListAsync();
        }

        public async Task<DynamicField?> GetByIdAsync(int id)
        {
            var entity = await _context.DynamicFields.FindAsync(id);
            return entity ?? null;
        }

        public async Task<bool> UpdateAsync(DynamicField entity)
        {
            var field = await _context.DynamicFields.FindAsync(entity.Id);
            if (field == null) return false;
            UpdateEntity(field, entity);
            await _context.SaveChangesAsync();
            return true;
        }
        public static void UpdateEntity(DynamicField entityInDatabase, DynamicField updatedEntity)
        {
            entityInDatabase.EntityId = updatedEntity.EntityId;
            entityInDatabase.CustomFieldId = updatedEntity.CustomFieldId;
            entityInDatabase.Value = updatedEntity.Value;
            entityInDatabase.CustomField = updatedEntity.CustomField;
        }

        public async Task<IEnumerable<DynamicField>?> GetAllByEntityIdAsync(int entityId)
        {
            return await _context.DynamicFields.Where(d => d.EntityId == entityId).ToListAsync();
        }
    }
}
