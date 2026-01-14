using Microsoft.EntityFrameworkCore;
using StudentBase.Domain;
using StudentBase.Domain.Entities;
using StudentBase.Domain.Repositories;

namespace StudentBase.Infrastructure.EntityFramework.Repositories
{
    public class GroupRepository : IGroupRepository
    {
        private readonly AppDbContext _context;
        public GroupRepository (AppDbContext context)
        {
            _context = context;
        }
        public async Task<int> CreateAsync(GroupEntity entity)
        {
            await _context.Groups.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null) return false;

            _context.Remove(group);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<GroupEntity>?> GetAllAsync()
        {
            return await _context.Groups.ToListAsync();
        }

        public async Task<IEnumerable<GroupEntity>?> GetAllByProgramIdAsync(int programId)
        {
            return await _context.Groups.Where(g => g.ProgramId == programId).ToListAsync();
        }

        public async Task<IEnumerable<GroupEntity>?> GetAllByYearOfEntryAsync(DateOnly yearOfEntry)
        {
            return await _context.Groups.Where(g => g.YearOfEntry ==  yearOfEntry).ToListAsync();   
        }

        public async Task<GroupEntity?> GetByIdAsync(int id)
        {
            return await _context.Groups.FindAsync(id);
        }

        public async Task<GroupEntity?> GetByNameAsync(string name)
        {
            return await _context.Groups.FirstOrDefaultAsync(g => g.Name == name);
        }

        public async Task<StatusGroups?> GetStatusGroupsAsync(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null) return null;
            return group.Status;
        }

        public async Task<bool> UpdateAsync(GroupEntity entity)
        {
            var group = await _context.Groups.FindAsync(entity.Id);
            if (group == null) return false;

            UpdateEntity(group, entity);
            return true;
        }

        public static void UpdateEntity(GroupEntity entityInDatabase, GroupEntity updatedEntity)
        {
            entityInDatabase.ProgramId = updatedEntity.ProgramId;
            entityInDatabase.Name = updatedEntity.Name;
            entityInDatabase.YearOfEntry = updatedEntity.YearOfEntry;
            entityInDatabase.DurationYears = updatedEntity.DurationYears;
            entityInDatabase.Status = updatedEntity.Status;
        }
    }
}
