using Breeze.Models.Constants;
using Breeze.Models.Dtos.Auth.Request;
using Breeze.Models.Dtos.OTP.Request;
using Breeze.Models.Entities;
using Breeze.Utilities;

namespace Breeze.Models.ModelMapping;

public static class DtoToEntityMappingExtensions
{
    public static OTPCodeEntity ToOTPCodeEntity(this SaveOTPRequestDto requestDto, string hashingKey)
    {
        return new OTPCodeEntity()
        {
            UserName = requestDto.UserName,
            OTPCode = Helper.ComputeHmacSha512Hash(requestDto.OTPCode.ToString(), hashingKey),
            ExpirationTime = Helper.GetCurrentDate().AddMinutes(MagicNumbers.OTP_EXPIRY_MINUTES),
            OTPUseCase = requestDto.OTPUseCase,
            CreatedBy = requestDto.UserName,
            CreatedDate = Helper.GetCurrentDate(),
        };
    }

    public static UserEntity ToUserEntity(this RegisterRequestDto requestDto, string deviceId)
    {
        return new UserEntity()
        {
            FirstName = requestDto.FirstName.Trim(),
            LastName = requestDto.LastName.Trim(),
            UserName = requestDto.UserName.Trim(),
            UserHandle = $"{requestDto.FirstName.Replace(" ", string.Empty).ToLower()}{requestDto.LastName.Replace(" ", string.Empty).ToLower()}{Helper.GenerateRandomNumber()}",
            TrustedDeviceId = deviceId,
            AcceptedTermsAndConditions = requestDto.AcceptedTermsAndConditions,
            CreatedBy = requestDto.UserName.Trim(),
            CreatedDate = Helper.GetCurrentDate(),
            Email = Helper.IsEmail(requestDto.UserName) ? requestDto.UserName.Trim() : null,
            PhoneNumber = requestDto.PhoneNumber,
            EmailConfirmed = true,
        };
    }
}
