using Breeze.DbCore.UnitOfWork;
using Breeze.Models.Configurations;
using Breeze.Models.Constants;
using Breeze.Models.Dtos.Email.Request;
using Breeze.Models.Dtos.OTP.Request;
using Breeze.Models.Dtos.OTP.Response;
using Breeze.Models.Entities;
using Breeze.Models.ModelMapping;
using Breeze.Services.ClaimResolver;
using Breeze.Services.Email;
using Breeze.Utilities;
using Microsoft.Extensions.Options;
using OtpNet;
using System.Security.Cryptography;

namespace Breeze.Services.OTP;
public class OTPService : IOTPService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly HashingConfiguration _hashingConfiguration;
    private readonly IEmailService _emailService;
    private readonly IClaimResolverService _claimResolverService;

    public OTPService(IUnitOfWork unitOfWork,
        IOptions<HashingConfiguration> hashingConfiguration,
        IEmailService emailService
,
        IClaimResolverService claimResolverService)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _hashingConfiguration = hashingConfiguration.Value;
        _claimResolverService = claimResolverService;
    }

    public async Task<bool> IsValideOTP(VerifyOTPRequestDto requestDto)
    {
        var OTPCode = Helper.ComputeHmacSha512Hash(requestDto.OTPCode.ToString(), _hashingConfiguration.HashingKey);

        var OTPRepo = _unitOfWork.GetRepository<OTPCodeEntity>();

        var OTPEntity = await OTPRepo.FindByFirstOrDefaultNoTrackingAsync(x =>
            x.UserName!.ToLower() == requestDto.UserName.ToLower()
            && x.OTPUseCase == requestDto.OTPUseCase
            && x.OTPCode == OTPCode
            && x.ExpirationTime >= DateTime.Now);

        return OTPEntity is not null && ConstantTimeComparison(OTPEntity.OTPCode, OTPCode);
    }

    public async Task SendOTPEmail(OTPEmailRequestDto requestDto)
    {
        await _emailService.SendEmailAsync(new EmailRequestDto<OTPEmailRequestDto>()
        {
            Subject = EmailSubjects.VERIFICATION_CODE_EMAIL,
            To = requestDto.UserName,
            TemplateName = _emailService.GetOTPEmailTemplateBasedOnUseCase(requestDto.OTPUseCase),
            Data = requestDto
        });
    }

    public OTPResponseDto GenerateOTP(GenerateOTPRequestDto requestDto)
    {
        byte[] secretKey = new byte[32];
        using (var randomNumberGenerator = RandomNumberGenerator.Create())
        {
            randomNumberGenerator.GetBytes(secretKey);
        }
        var totp = new Totp(secretKey, step: 30, mode: OtpHashMode.Sha256, totpSize: MagicNumbers.OTP_LENGTH);
        var otp = totp.ComputeTotp();

        return requestDto.ToOTPResponseDto(otp);
    }

    public async Task SaveOTP(SaveOTPRequestDto requestDto)
    {
        var entity = requestDto.ToOTPCodeEntity(_hashingConfiguration.HashingKey);
        var repo = _unitOfWork.GetRepository<OTPCodeEntity>();
        await repo.AddAsync(entity);
        await _unitOfWork.CommitAsync();
    }

    public async Task InvalidateExistingOTPs(string userName)
    {
        var repo = _unitOfWork.GetRepository<OTPCodeEntity>();
        var entities = await repo.FindByAsync(x => x.UserName!.ToLower() == userName.ToLower());

        if (entities is not null && entities.Any())
        {
            foreach (var entity in entities)
            {
                entity.Deleted = true;
                entity.ModifiedBy = Usernames.SYSTEM_USERNAME;
                entity.ModifiedDate = DateTime.Now;
            }

            repo.UpdateList(entities);

            await _unitOfWork.CommitAsync();
        }
    }

    #region Private Methods

    private static bool ConstantTimeComparison(string a, string b)
    {
        uint diff = (uint)a.Length ^ (uint)b.Length;
        for (int i = 0; i < a.Length && i < b.Length; i++)
        {
            diff |= (uint)a[i] ^ (uint)b[i];
        }
        return diff == 0;
    }
    #endregion
}
