using AutoMapper;
using Breeze.Models.Constants;
using Breeze.Models.Dtos.Email.Request;
using Breeze.Models.Dtos.OTP.Request;
using Breeze.Models.GenericResponses;
using Breeze.Services.Auth;
using Breeze.Services.HttpHeader;

namespace Breeze.Services.OTP;
public class OTPFacadeService : IOTPFacadeService
{
    private readonly IOTPService _otpService;
    private readonly IAuthService _authService;
    private readonly IHttpHeaderService _httpHeaderService;
    private readonly IMapper _mapper;

    public OTPFacadeService(IOTPService otpService,
        IAuthService authService,
        IHttpHeaderService httpHeaderService,
        IMapper mapper)
    {
        _otpService = otpService;
        _authService = authService;
        _httpHeaderService = httpHeaderService;
        _mapper = mapper;
    }

    public async Task<GenericResponse<bool>> GenerateOTP(GenerateOTPRequestDto requestDto)
    {
        var otpResponseDto = _otpService.GenerateOTP(requestDto);
        await _otpService.SaveOTP(_mapper.Map<SaveOTPRequestDto>(otpResponseDto));
        await _otpService.SendOTPEmail(_mapper.Map<OTPEmailRequestDto>(otpResponseDto));
        return GenericResponse<bool>.Success(true, ApiResponseMessages.VERIFICATION_CODE_SENT, ApiStatusCodes.VERIFICATION_CODE_SENT);
    }

    public async Task<GenericResponse<bool>> VerifyOTP(VerifyOTPRequestDto requestDto)
    {
        var user = await _authService.GetUserByUsername(requestDto.UserName);
        if (user is null || !await _authService.UserExists(requestDto.UserName))
        {
            return GenericResponse<bool>.Failure(ApiResponseMessages.USER_DOES_NOT_EXIST, ApiStatusCodes.USER_DOES_NOT_EXIST);
        }

        if (!await _otpService.IsValideOTP(requestDto))
            return GenericResponse<bool>.Failure(ApiResponseMessages.INVALID_VERIFICATION_CODE, ApiStatusCodes.INVALID_VERIFICATION_CODE);

        if (user.TrustedDeviceId.ToLower() != _httpHeaderService.GetHeader(PropertyNames.DEVICE_ID).ToString())
        {
            await _authService.UpdateDevice(requestDto.UserName);
        }

        return GenericResponse<bool>.Success(true, ApiResponseMessages.OTP_VERIFIED_SUCCESSFULLY, ApiStatusCodes.OTP_VERIFIED_SUCCESSFULLY);
    }
}
