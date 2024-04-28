using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLWalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();

            return walk;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true)
        {
            var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            // Filter
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
            }

            // Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            return await walks.ToListAsync();

            //var walks = await dbContext.Walks.Include("Difficulty")
            //    .Include("Difficulty")
            //    .Include("Region")
            //    .ToListAsync();

            //return walks;
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            var walkDomain = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            return walkDomain;
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalkDomain = await dbContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);

            if (existingWalkDomain == null)
            {
                return null;
            }

            existingWalkDomain.Name = walk.Name;
            existingWalkDomain.Description = walk.Description;
            existingWalkDomain.LengthInKm = walk.LengthInKm;
            existingWalkDomain.WalkImageUrl = walk.WalkImageUrl;
            existingWalkDomain.RegionId = walk.RegionId;
            existingWalkDomain.DifficultyId = walk.DifficultyId;

            await dbContext.SaveChangesAsync();

            return existingWalkDomain;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var walkDomain = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (walkDomain == null)
            {
                return null;
            }

            dbContext.Walks.Remove(walkDomain);
            await dbContext.SaveChangesAsync();

            return walkDomain;
        }
    }
}
