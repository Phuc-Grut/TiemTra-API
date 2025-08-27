using Application.Interface;
using Domain.DTOs.Profile;
using Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProfileService : IProfileServices
    {
        private readonly IProfileRepository _profileRepository;

        public ProfileService(IProfileRepository profileRepository)
        {
                _profileRepository = profileRepository;
        }

        public async Task<ProfileDto?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await _profileRepository.GetProfileByUserIdAsync(userId, ct);
        }

        public async Task<bool> UpdateAsync(Guid userId, UpdateProfileRequest req, CancellationToken ct = default)
        {
            var user = await _profileRepository.FindUserForUpdateAsync(userId, ct);
            if (user is null) return false;

           
            var phone = req.PhoneNumber.Trim();
            if (phone.Length > 0 && !Regex.IsMatch(phone, @"^(0|\+84)\d{9,10}$"))
                    throw new ArgumentException("Số điện thoại không hợp lệ");
            user.PhoneNumber = phone;
           

            user.FullName = req.FullName.Trim();

            user.Address = req.Address.Trim();

            user.Avatar = req.Avatar;

            user.UpdatedAt = DateTime.UtcNow;
            user.UpdatedBy = userId;

            await _profileRepository.SaveChangesAsync(ct);
            return true;
        }
    }
}
