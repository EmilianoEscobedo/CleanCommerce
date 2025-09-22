using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Security.Application.DTOs;
using Security.Domain.Commons;
using Security.Domain.Entities;
using Security.Domain.Repositories;

namespace Security.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IValidator<RegisterUserDto> _registerValidator;
    private readonly IValidator<LoginDto> _loginValidator;
    private readonly IMapper _mapper;

    public AuthService(
        IUserRepository userRepository,
        ITokenService tokenService,
        IPasswordHasher<User> passwordHasher,
        IValidator<RegisterUserDto> registerValidator,
        IValidator<LoginDto> loginValidator,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
        _mapper = mapper;
    }

    Task<Result<UserDto>> IAuthService.RegisterAsync(RegisterUserDto registerDto) => RegisterAsync(registerDto);
    Task<Result<TokenDto>> IAuthService.LoginAsync(LoginDto loginDto) => LoginAsync(loginDto);
    Result<bool> IAuthService.ValidateToken(string token) => ValidateToken(token);

    private async Task<Result<UserDto>> RegisterAsync(RegisterUserDto registerDto)
    {
        var validationResult = await _registerValidator.ValidateAsync(registerDto);
        if (!validationResult.IsValid)
            return Result<UserDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage));

        var emailCheck = await _userRepository.EmailExistsAsync(registerDto.Email);
        if (emailCheck.IsFailure)
            return Result<UserDto>.Failure(emailCheck.Errors);

        if (emailCheck.Value)
            return Result<UserDto>.Failure("Email already exists");

        var user = new User(registerDto.Email, _passwordHasher.HashPassword(null, registerDto.Password));
        var addResult = await _userRepository.AddAsync(user);
        if (addResult.IsFailure)
            return Result<UserDto>.Failure(addResult.Errors);

        var userDto = _mapper.Map<UserDto>(user);
        return Result<UserDto>.Success(userDto);
    }

    private async Task<Result<TokenDto>> LoginAsync(LoginDto loginDto)
    {
        var validationResult = await _loginValidator.ValidateAsync(loginDto);
        if (!validationResult.IsValid)
            return Result<TokenDto>.Failure(validationResult.Errors.Select(e => e.ErrorMessage));

        var userResult = await _userRepository.GetByEmailAsync(loginDto.Email);
        if (userResult.IsFailure)
            return Result<TokenDto>.Failure("Invalid credentials");

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(
            userResult.Value, 
            userResult.Value.PasswordHash, 
            loginDto.Password
        );

        if (passwordVerificationResult == PasswordVerificationResult.Failed)
            return Result<TokenDto>.Failure("Invalid credentials");

        var tokenDto = _tokenService.GenerateToken(userResult.Value); 
        return Result<TokenDto>.Success(tokenDto);
    }
    
    private Result<bool> ValidateToken(string token)
    {
        var isValid = _tokenService.ValidateToken(token);
        return Result<bool>.Success(isValid);
    }
}
