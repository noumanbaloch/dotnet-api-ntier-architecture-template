using Breeze.Models.ApplicationEnums;
using Breeze.Models.Constants;

namespace Breeze.Models.GenericResponses;
public static class GenericResponseHelper
{
    public static GenericResponse GenerateEnumToGenericResponse(ResponseEnums result)
    {
        return result switch
        {
            //Success
            ResponseEnums.EmailVerifiedSuccessfully => GenericResponse.Success(ApiResponseMessages.EMAIL_VERIFIED, ApiStatusCodes.EMAIL_VERIFIED),
            ResponseEnums.VerificationCodeSent => GenericResponse.Success(ApiResponseMessages.VERIFICATION_CODE_SENT, ApiStatusCodes.VERIFICATION_CODE_SENT),

            //Failure
            ResponseEnums.InvalidVerificationCode => GenericResponse.Failure(ApiResponseMessages.INVALID_VERIFICATION_CODE, ApiStatusCodes.INVALID_VERIFICATION_CODE),
            ResponseEnums.UserDoesNotExist => GenericResponse.Failure(ApiResponseMessages.USER_DOES_NOT_EXIST, ApiStatusCodes.USER_DOES_NOT_EXIST),

            _ => GenericResponse.Failure(ApiResponseMessages.SOMETHING_WENT_WRONG, ApiStatusCodes.SOMETHING_WENT_WRONG),
        };
    }

    public static GenericResponse<T> GenerateEnumToGenericResponse<T>(ResponseEnums result, T? payload, string? message = null)
    {
        return result switch
        {
            //Success
            ResponseEnums.VerificationCodeSent => GenericResponse<T>.Success(ApiResponseMessages.VERIFICATION_CODE_SENT, ApiStatusCodes.VERIFICATION_CODE_SENT),
            ResponseEnums.UserRegisteredSuccessfully => GenericResponse<T>.Success(payload, $"{ApiResponseMessages.WELCOME_MESSAGE}{message}", ApiStatusCodes.WELCOME_MESSAGE),
            ResponseEnums.UserLoginSuccessfully => GenericResponse<T>.Success(payload, $"{ApiResponseMessages.WELCOME_BACK_MESSAGE}{message}", ApiStatusCodes.WELCOME_BACK_MESSAGE),
            ResponseEnums.PasswordChangedSuccessfully => GenericResponse<T>.Success(payload, ApiResponseMessages.PASSWORD_CHANGED, ApiStatusCodes.PASSWORD_CHANGED),
            ResponseEnums.ProfileUpdatedSuccessfully => GenericResponse<T>.Success(payload, ApiResponseMessages.PROFILE_UPDATED_SUCCESSFULLY, ApiStatusCodes.PROFILE_UPDATED_SUCCESSFULLY),

            //Failure
            ResponseEnums.UserAlreadyExist => GenericResponse<T>.Failure(ApiResponseMessages.USER_ALREADY_EXIST, ApiStatusCodes.USER_ALREADY_EXIST),
            ResponseEnums.InvalidVerificationCode => GenericResponse<T>.Failure(ApiResponseMessages.INVALID_VERIFICATION_CODE, ApiStatusCodes.INVALID_VERIFICATION_CODE),
            ResponseEnums.UnableToCompleteProcess => GenericResponse<T>.Failure(ApiResponseMessages.UNABLE_TO_COMPLETE_PROCESS, ApiStatusCodes.UNABLE_TO_COMPLETE_PROCESS),
            ResponseEnums.InvalidUsernamePassword => GenericResponse<T>.Failure(ApiResponseMessages.INVALID_USERNAME_OR_PASSWORD, ApiStatusCodes.INVALID_USERNAME_OR_PASSWORD),
            ResponseEnums.InvalidPassword => GenericResponse<T>.Failure(ApiResponseMessages.PASSWORD_IS_INVALID, ApiStatusCodes.PASSWORD_IS_INVALID),
            ResponseEnums.SomethingWentWrong => GenericResponse<T>.Failure(ApiResponseMessages.SOMETHING_WENT_WRONG, ApiStatusCodes.SOMETHING_WENT_WRONG),
            ResponseEnums.UserDoesNotExist => GenericResponse<T>.Failure(ApiResponseMessages.USER_DOES_NOT_EXIST, ApiStatusCodes.USER_DOES_NOT_EXIST),
            _ => GenericResponse<T>.Failure(ApiResponseMessages.SOMETHING_WENT_WRONG, ApiStatusCodes.SOMETHING_WENT_WRONG),
        };
    }
}