namespace Breeze.Models.ApplicationEnums;
public enum ResponseEnums
{
    //Success
    UserRegisteredSuccessfully,
    UserLoginSuccessfully,
    VerificationCodeSent,
    PasswordChangedSuccessfully,
    EmailVerifiedSuccessfully,
    ProfileUpdatedSuccessfully,

    //Failure
    UserAlreadyExist,
    InvalidVerificationCode,
    InvalidUsernamePassword,
    UnableToCompleteProcess,
    InvalidPassword,
    SomethingWentWrong,
    UserDoesNotExist
}
