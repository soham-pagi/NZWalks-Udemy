﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Walk>> GetAllAsync()
        {
            var walks = await dbContext.Walks.Include("Difficulty")
                .Include("Difficulty")
                .Include("Region")
                .ToListAsync();

            return walks;
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