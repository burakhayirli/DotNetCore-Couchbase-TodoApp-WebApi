using Business.Abstract;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Messages;
using DataAccess.Abstract;
using System.Collections.Generic;
using System;

namespace Business.Concrete
{
    public class UserService : ServiceBase, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [ValidationAspect(typeof(UserValidator), Priority = 1)]
        public IResult Add(User user)
        {
            var existUser = _userRepository.GetByMail(user.Email);
            if (existUser != null) return new ErrorResult(BusinessMessages.UserAlreadyExists);

            var result = _userRepository.Save(user);
            if (result.Success) return new SuccessResult(BusinessMessages.Added);
            return new ErrorResult();
        }

        public IDataResult<User> GetById(Guid id)
        {
            var result = _userRepository.GetById(id);
            if (result != null) return new SuccessDataResult<User>(result);
            return new ErrorDataResult<User>(BusinessMessages.UserNotFound);
        }

        public IDataResult<User> GetByMail(string email)
        {
            var result= _userRepository.GetByMail(email);
            if (result != null) return new SuccessDataResult<User>(result);
            return new ErrorDataResult<User>(BusinessMessages.UserNotFound);
        }

        public IDataResult<List<OperationClaim>> GetClaims(User user)
        {
            return new ErrorDataResult<List<OperationClaim>>(new List<OperationClaim>());
        }
    }
}
