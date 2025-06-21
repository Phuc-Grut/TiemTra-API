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
        


        public BrandService(IBrandRepository brandRepository, IMapper mapper, IUserRepository userRepository, BlobServiceClient blobServiceClient)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _blobServiceClient = blobServiceClient;
        }

        public async Task<BrandDTO> CreateAsync(CreateBrandDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var lastBrand = await _brandRepository.GetAllBrandsAsync(cancellationToken);
                var nextId = lastBrand.Any() ? lastBrand.Max(b => b.BrandId) + 1 : 1;

                var newBrand = new Brand
                {
                    BrandId = nextId,
                    BrandName = dto.BrandName,
                    Logo = dto.Logo,
                    Description = dto.Description,
                    CreatedBy = Guid.NewGuid(), // TODO: Get from current user context
                    UpdatedBy = Guid.NewGuid(), // TODO: Get from current user context
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var result = await _brandRepository.AddBrandAsync(newBrand, cancellationToken);

                return _mapper.Map<BrandDTO>(result); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CreateAsync] Lỗi: {ex.Message}");
                Console.WriteLine($"[CreateAsync] Stack Trace: {ex.StackTrace}");
                throw new Exception($"Thêm thương hiệu thất bại: {ex.Message}");
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

                //// Xoá liên kết BrandId trong Product
                //await _productRepository.RemoveBrandFromProducts(id, cancellationToken);
            }

            // Sau khi đã xoá BrandId khỏi Product, xoá brand
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
        public async Task<PagedResult<BrandDTO>> GetAllPagedAsync(BrandFilterDto filters, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var query = (await _brandRepository.GetAllBrandsAsync(cancellationToken)).AsQueryable();

            var keyword = filters.Keyword?.Trim();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(b => EF.Functions.Like(b.BrandName, $"%{keyword}%"));
            }

            int totalItems = query.Count();

            var brands = query.OrderBy(b => b.BrandName)
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();

            var brandDtos = brands.Select(b => new BrandDTO
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
                TotalPages = (int)Math.Ceiling((double)totalItems / pageSize),
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
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

        

        public async Task<bool> UpdateAsync(UpdateBrandDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var existing = await _brandRepository.GetBrandByIdAsync(dto.BrandId, cancellationToken);
                if (existing == null)
                {
                    Console.WriteLine($"[BrandService][Update] Brand ID {dto.BrandId} not found.");
                    return false;
                }

                _mapper.Map(dto, existing);
                return await _brandRepository.UpdateBrandAsync(existing, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BrandService][Update] Error: {ex.Message}");
                throw new Exception("Cập nhật thương hiệu thất bại.");
            }
        }
    }
}

