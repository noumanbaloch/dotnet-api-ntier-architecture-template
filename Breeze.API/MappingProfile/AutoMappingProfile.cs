using AutoMapper;
using Breeze.Models.Dtos.Auth.Request;
using Breeze.Models.Dtos.DropDown.Response;
using Breeze.Models.Dtos.Email.Request;
using Breeze.Models.Dtos.OTP.Request;
using Breeze.Models.Dtos.OTP.Response;
using Breeze.Models.Dtos.Token.Request;
using Breeze.Models.Entities;
using Breeze.Utilities;

namespace Breeze.API.MappingProfile;

public class AutoMappingProfile : Profile
{
    public AutoMappingProfile()
    {
        ConfigureDtoToDtoMapping();
        ConfigureDtoToEntityMapping();
        ConfigureEntityToDtoMapping();
        ConfigureEntityToEntityMapping();
    }

    private void ConfigureDtoToDtoMapping()
    {
        CreateMap<GenerateOTPRequestDto, OTPResponseDto>().ReverseMap();
        CreateMap<LoginRequestDto, GenerateOTPRequestDto>().ReverseMap();
        CreateMap<LoginRequestDto, VerifyOTPRequestDto>().ReverseMap();
        CreateMap<ForgotPasswordRequestDto, GenerateOTPRequestDto>().ReverseMap();
        CreateMap<ForgotPasswordRequestDto, VerifyOTPRequestDto>().ReverseMap();
        CreateMap<OTPResponseDto, OTPEmailRequestDTO>().ReverseMap();
        CreateMap<OTPResponseDto, SaveOTPRequestDto>().ReverseMap();
        CreateMap<VerifyOTPRequestDto, CreateTokenRequestDto>().ReverseMap();
        CreateMap<RegisterRequestDto, GenerateOTPRequestDto>().ReverseMap();
        CreateMap<RegisterRequestDto, VerifyOTPRequestDto>().ReverseMap();
    }

    private void ConfigureDtoToEntityMapping()
    {
        CreateMap<UpdateProfileRequestDto, UserEntity>()
        .ForMember(dest => dest.UserHandle, opt => opt.MapFrom(src => $"{src.FirstName.Replace(" ", string.Empty).ToLower()}{src.LastName.Replace(" ", string.Empty).ToLower()}{Helper.GenerateRandomNumber()}"));
    }

    private void ConfigureEntityToDtoMapping()
    {
        CreateMap<BoardDetailEntity, DropDownResponseDto>()
         .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.BoardName));
        CreateMap<CollegeEntity, DropDownResponseDto>()
          .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CollegeName));
    }

    private void ConfigureEntityToEntityMapping()
    {
    }
}
