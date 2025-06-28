using Application.DTOs;
using Application.DTOs.Admin.Brand;
using Application.Interface;
using AutoMapper;
using Azure.Storage.Blobs;
using Domain.Data.Entities;
using Domain.Interface;
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

        public async Task<ApiResponse> AddBrandAsync(CreateBrandDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                // Kiểm tra tên thương hiệu đã tồn tại
                var existingBrands = await _brandRepository.GetAllBrandsAsync(cancellationToken);
                if (existingBrands.Any(b => b.BrandName.ToLower().Trim() == dto.BrandName.ToLower().Trim()))
                {
                    return new ApiResponse(false, "Tên thương hiệu đã tồn tại.");
                }

                // Khởi tạo brand mới
                var brand = new Brand
                {
                    BrandName = dto.BrandName,
                    Description = dto.Description,
                    Logo = dto.Logo,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var created = await _brandRepository.AddBrandAsync(brand, cancellationToken);

                if (created == null)
                    return new ApiResponse(false, "Thêm thương hiệu thất bại.");

                return new ApiResponse(true, "Thêm thương hiệu thành công.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BrandService][AddBrandAsync] Error: {ex.Message}");
                return new ApiResponse(false, $"Lỗi khi thêm thương hiệu: {ex.Message}");
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
                    results.Add(new BrandDeleteResult
                    {
                        BrandId = id,
                        IsDeleted = false,
                        Message = "Thương hiệu không tồn tại."
                    });
                    continue;
                }

                await _productRepository.RemoveBrandFromProducts(id, cancellationToken);
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

        public async Task<IEnumerable<BrandDTO>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                var brands = await _brandRepository.GetAllBrandsAsync(cancellationToken);
                return brands.Select(_mapper.Map<BrandDTO>);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BrandService][GetAll] Error: {ex.Message}");
                return Enumerable.Empty<BrandDTO>();
            }
        }

        public async Task<BrandDTO?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var brand = await _brandRepository.GetBrandByIdAsync(id, cancellationToken);
                return brand == null ? null : _mapper.Map<BrandDTO>(brand);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BrandService][GetById] Error: {ex.Message}");
                return null;
            }
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
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var pagedBrands = query.OrderBy(b => b.BrandName)
                                   .Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToList();

            var brandDtos = pagedBrands.Select(b => new BrandDTO
            {
                BrandId = b.BrandId,
                BrandName = b.BrandName,
                Logo = b.Logo,
                Description = b.Description
            }).ToList();

            return new PagedResult<BrandDTO>
            {
                Items = brandDtos,
                TotalItems = totalItems,
                TotalPages = totalPages,
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
                {
                    return false;
                }

                _mapper.Map(dto, existing);
                return await _brandRepository.UpdateBrandAsync(existing, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BrandService][Update] Error: {ex.Message}");
                return false;
            }
        }
    }
}

