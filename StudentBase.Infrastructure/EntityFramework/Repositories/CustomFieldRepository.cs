using Microsoft.EntityFrameworkCore;
using StudentBase.Domain.Entities;
using StudentBase.Domain.Entities.Dynamic;
using StudentBase.Domain.Repositories;

namespace StudentBase.Infrastructure.EntityFramework.Repositories
{
    public class CustomFieldRepository : ICustomFieldRepository
    {
        private readonly AppDbContext _context;
        public CustomFieldRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(CustomField entity)
        {
            entity.CreatedAt = DateTime.Now;
            await _context.CustomFields.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.CustomFields.FindAsync(id);
            if (entity == null) return false;

            _context.CustomFields.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CustomField>?> GetAllAsync()
        {
            return await _context.CustomFields.ToListAsync();
        }

        public async Task<CustomField?> GetByIdAsync(int id)
        {
            var entity = await _context.CustomFields.FindAsync(id);
            return entity == null ? null : entity;
        }

        public async Task<bool> UpdateAsync(CustomField entity)
        {
            var field = await _context.CustomFields.FindAsync(entity.Id);
            if (field == null) return false;
            UpdateEntity(field, entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public static void UpdateEntity(CustomField entityInDatabase, CustomField updatedEntity)
        {
            entityInDatabase.FieldName = updatedEntity.FieldName;
            entityInDatabase.DisplayName = updatedEntity.DisplayName;
            entityInDatabase.FieldType = updatedEntity.FieldType;
            entityInDatabase.DynamicValues = updatedEntity.DynamicValues;
        }
    }
}
