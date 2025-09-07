using Application.DTOs;
using Domain.Data.Entities;
using Domain.Enum;
using Domain.Interface;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly AppDbContext _context;

        public VoucherRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Voucher> CreateAsync(Voucher voucher, CancellationToken cancellationToken)
        {
            _context.Vouchers.Add(voucher);
            await _context.SaveChangesAsync(cancellationToken);
            return voucher;
        }

        public async Task<Voucher?> GetByIdAsync(Guid voucherId, CancellationToken cancellationToken)
        {
            return await _context.Vouchers
                .Include(v => v.Creator)
                .Include(v => v.Updater)
                .FirstOrDefaultAsync(v => v.VoucherId == voucherId, cancellationToken);
        }

        public async Task<Voucher?> GetByCodeAsync(string voucherCode, CancellationToken cancellationToken)
        {
            return await _context.Vouchers
                .FirstOrDefaultAsync(v => v.VoucherCode == voucherCode, cancellationToken);
        }


        public async Task<List<Voucher>> GetPublicVouchersAsync(CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            return await _context.Vouchers
                .Where(v => v.Status == VoucherStatus.Publish && 
                           v.EndDate > now &&
                           v.UsedQuantity < v.Quantity)
                .OrderBy(v => v.EndDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> UpdateAsync(Voucher voucher, CancellationToken cancellationToken)
        {
            _context.Vouchers.Update(voucher);
            var result = await _context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> UpdateStatusAsync(Guid voucherId, VoucherStatus status, Guid updatedBy, CancellationToken cancellationToken)
        {
            var voucher = await _context.Vouchers.FindAsync(voucherId);
            if (voucher == null) return false;

            voucher.Status = status;
            voucher.UpdatedBy = updatedBy;
            voucher.UpdatedAt = DateTime.UtcNow;

            var result = await _context.SaveChangesAsync(cancellationToken);
            return result > 0;
        }

        public async Task<bool> VoucherCodeExistsAsync(string voucherCode, CancellationToken cancellationToken)
        {
            return await _context.Vouchers
                .AnyAsync(v => v.VoucherCode == voucherCode, cancellationToken);
        }

        public async Task<string> GenerateUniqueVoucherCodeAsync(CancellationToken cancellationToken)
        {
            var random = new Random();
            string voucherCode;
            bool exists;
            
            do
            {
                int randomNumber = random.Next(100000, 999999);
                voucherCode = $"VC{randomNumber}";
                exists = await VoucherCodeExistsAsync(voucherCode, cancellationToken);
            }
            while (exists);
            
            return voucherCode;
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

    async Task<Domain.DTOs.PagedResult<Voucher>> IVoucherRepository.GetPagedAsync(int pageNumber, int pageSize, VoucherStatus? status, string? keyword, CancellationToken cancellationToken)
    {
     var query = _context.Vouchers
                .Include(v => v.Creator)
                .Include(v => v.Updater)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(v => v.Status == status.Value);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var searchTerm = keyword.Trim().ToLower();
                query = query.Where(v => 
                    v.VoucherName.ToLower().Contains(searchTerm) ||
                    v.VoucherCode.ToLower().Contains(searchTerm) ||
                    (v.Description != null && v.Description.ToLower().Contains(searchTerm))
                );
            }

            var totalItems = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(v => v.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new Domain.DTOs.PagedResult<Voucher>
            {
                Items = items,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
    }
    
  }
}