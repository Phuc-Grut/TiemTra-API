using Application.DTOs;
using Application.DTOs.Admin.Brand;
using Application.Interface;
using AutoMapper;
using Azure.Storage.Blobs;
using Domain.Data.Entities;
using Domain.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BrandService : IBrandService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IProductRepository _productRepository;

        public BrandService(IBrandRepository brandRepository, IMapper mapper, IUserRepository userRepository, BlobServiceClient blobServiceClient)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _blobServiceClient = blobServiceClient;
        }

        public async Task<ApiResponse> AddBrandAsync(CreateBrandDTO dto, ClaimsPrincipal user, CancellationToken cancellationToken)
        {
            try
            {
                var userId = GetUserIdFromClaims.GetUserId(user);
                var existing = await _brandRepository.GetAllBrandsAsync(cancellationToken);
                if (existing.Any(b => b.BrandName.Trim().ToLower() == dto.BrandName.Trim().ToLower()))
                {
                    return new ApiResponse(false, "Tên thương hiệu đã tồn tại.");
                }

                var brandId = await GenerateUniqueBrandIdAsync(cancellationToken);

                var brand = new Brand
                {
                    BrandId = brandId,
                    BrandName = dto.BrandName,
                    Description = dto.Description,
                    Logo = dto.Logo,
                    CreatedBy = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var result = await _brandRepository.AddBrandAsync(brand, cancellationToken);

                if (result == null)
                {
                    return new ApiResponse(false, "Thêm thương hiệu thất bại.");
                }

                return new ApiResponse(true, "Thêm thương hiệu thành công.", new
                {
                    brandId = result.BrandId,
                    brandName = result.BrandName,
                    logo = result.Logo,
                    description = result.Description
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BrandService][Add] Error: {ex.Message}");
                return new ApiResponse(false, "Lỗi hệ thống khi thêm thương hiệu.");
            }
        }

        public async Task<List<BrandDeleteResult>> DeleteManyAsync(List<int> brandIds, CancellationToken cancellationToken)
        {
            var results = new List<BrandDeleteResult>();

            foreach (var id in brandIds)
            {
                var brand = await _brandRepository.GetBrandByIdAsync(id, cancellationToken);
                if (brand == null)
                {
                    results.Add(new BrandDeleteResult { BrandId = id, IsDeleted = false, Message = "Thương hiệu không tồn tại." });
                    continue;
                }
            }

            var deletedIds = await _brandRepository.DeleteBrandsAsync(brandIds, cancellationToken);

            foreach (var id in brandIds)
            {
                results.Add(new BrandDeleteResult
                {
                    BrandId = id,
                    IsDeleted = deletedIds.Contains(id),
                    Message = deletedIds.Contains(id) ? "Xoá thành công." : "Xoá thất bại."
                });
            }

            return results;
        }

        public async Task<int> GenerateUniqueBrandIdAsync(CancellationToken cancellationToken)
        {
            var existingIds = (await _brandRepository.GetAllBrandsAsync(cancellationToken))
                            .Select(b => b.BrandId)
                            .ToHashSet();

            int newId;
            var random = new Random();

            do
            {
                newId = random.Next(100, 999);
            } while (existingIds.Contains(newId));

            return newId;
        }

        public async Task<IEnumerable<BrandDTO>> GetAllAsync(CancellationToken cancellationToken)
        {
            var brands = await _brandRepository.GetAllBrandsAsync(cancellationToken);
            return brands.Select(_mapper.Map<BrandDTO>);
        }

        public async Task<BrandDTO?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var brand = await _brandRepository.GetBrandByIdAsync(id, cancellationToken);
            return brand == null ? null : _mapper.Map<BrandDTO>(brand);
        }

        public async Task<PagedResult<BrandDTO>> GetPagingAsync(BrandFilterDto filters, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var allBrands = await _brandRepository.GetAllBrandsAsync(cancellationToken);
            var query = allBrands.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filters.Keyword))
            {
                var keyword = filters.Keyword.Trim();
                query = query.Where(b => EF.Functions.Like(b.BrandName, $"%{keyword}%"));
            }

            var totalItems = query.Count();

            var pagedBrands = query
                .OrderBy(b => b.BrandName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Lấy danh sách ID người tạo/cập nhật
            var userIds = pagedBrands
                .Select(b => b.CreatedBy)
                .Union(pagedBrands.Select(b => b.UpdatedBy))
                .Where(id => id != null)
                .Select(id => id)
                .Distinct()
                .ToList();

            var users = await _userRepository.GetUsersByIdsAsync(userIds, cancellationToken);

            var brandDtos = pagedBrands.Select(b =>
            {
                var creator = users.FirstOrDefault(u => u.UserId == b.CreatedBy);
                var updater = users.FirstOrDefault(u => u.UserId == b.UpdatedBy);

                return new BrandDTO
                {
                    BrandId = b.BrandId,
                    BrandName = b.BrandName,
                    Logo = b.Logo,
                    Description = b.Description,
                    CreatedAt = b.CreatedAt,
                    UpdatedAt = b.UpdatedAt,
                    CreatorName = creator?.FullName,
                    UpdaterName = updater?.FullName
                };
            }).ToList();

            return new PagedResult<BrandDTO>
            {
                Items = brandDtos,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<bool> UpdateAsync(UpdateBrandDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var existing = await _brandRepository.GetBrandByIdAsync(dto.BrandId, cancellationToken);
                if (existing == null)
                    return false;

                _mapper.Map(dto, existing);
                existing.UpdatedAt = DateTime.UtcNow;

                return await _brandRepository.UpdateBrandAsync(existing, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BrandService][Update] Error: {ex.Message}");
                return false;
            }
        }

        public async Task<string> UploadBrandImageAsync(IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File không hợp lệ");

            try
            {
                var container = _blobServiceClient.GetBlobContainerClient("brands");

                // Tạo container nếu chưa có
                await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

                // Tạo tên file ngẫu nhiên để tránh trùng
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var blobClient = container.GetBlobClient(fileName);

                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, overwrite: true, cancellationToken);
                }

                return blobClient.Uri.ToString(); // Trả về URL ảnh
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UploadBrandImageAsync] Error: {ex.Message}");
                throw;
            }
        }
    }
}

