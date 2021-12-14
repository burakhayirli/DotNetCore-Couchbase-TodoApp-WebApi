using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Mapping;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Utilities.Messages;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;

namespace Business.Concrete
{
    public class AuthService : ServiceBase, IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;
        public AuthService(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var claims = _userService.GetClaims(user);
            var accessToken = _tokenHelper.CreateToken(user, claims.Data);
            return new SuccessDataResult<AccessToken>(accessToken, BusinessMessages.AccessTokenCreated);
        }

        [ValidationAspect(typeof(UserForLoginDtoValidator), Priority = 1)]
        public IDataResult<User> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = _userService.GetByMail(userForLoginDto.Email);
            if (userToCheck == null || !userToCheck.Success)
            {
                return new ErrorDataResult<User>(BusinessMessages.UserNotFound);
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.Data.PasswordHash, userToCheck.Data.PasswordSalt))
            {
                return new ErrorDataResult<User>(BusinessMessages.PasswordError);
            }

            return new SuccessDataResult<User>(userToCheck.Data, BusinessMessages.SuccessfulLogin);
        }

        [ValidationAspect(typeof(UserForRegisterDtoValidator), Priority = 1)]
        public IDataResult<User> Register(UserForRegisterDto userForRegisterDto, string password)
        {
            HashingHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            var user = new User
            {
                Email = userForRegisterDto.Email,
                FirstName = userForRegisterDto.FirstName,
                LastName = userForRegisterDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true
            };
            var newUser=_userService.Add(user);
            if(newUser.Success) return new SuccessDataResult<User>(user, BusinessMessages.Added);
            return new ErrorDataResult<User>(user, BusinessMessages.UserAlreadyExists);
        }

        public IResult IsUserExists(string email)
        {
            if (_userService.GetByMail(email).Data != null)
            {
                return new SuccessResult(BusinessMessages.UserAlreadyExists);
            }
            return new ErrorResult();
        }

        [SecuredOperation]
        public IDataResult<UserViewModel> GetCurrentUser()
        {
            var userId = GetCurrentUserId();
            var user = _userService.GetById(userId);
            if(user.Data==null) return new ErrorDataResult<UserViewModel>(BusinessMessages.UserNotFound);

            var result = ObjectMapper.Mapper.Map<UserViewModel>(user.Data);
            return new SuccessDataResult<UserViewModel>(result);            
        }
    }
}
