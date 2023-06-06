using Ecommerce.Application.Contracts.Infrastructure;
using Ecommerce.Application.Exceptions;
using Ecommerce.Application.Models.Email;
using Ecommerce.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Ecommerce.Application.Features.Auths.Users.Commands.SendPassword;

public class SendPasswordCommandHandler : IRequestHandler<SendPasswordCommand, string>
{
    private readonly IEmailService _serviceEmail;
    private readonly UserManager<User> _userManager;

    public SendPasswordCommandHandler(IEmailService serviceEmail, UserManager<User> userManager)
    {
        _serviceEmail = serviceEmail;
        _userManager = userManager;
    }
    public async Task<string> Handle(SendPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email!);
        if (user is null)
        {
            throw new BadRequestException("Username does not exist");
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var plainTextBytes = Encoding.UTF8.GetBytes(token);
        token = Convert.ToBase64String(plainTextBytes);

        var emailMessage = new EmailMessage
        {
            To = request.Email,
            Body = "Reset the password, click here:",
            Subject = "Change password"
        };

        var result = await _serviceEmail.SendEmail(emailMessage, token);

        if (!result)
        {
            throw new Exception("Could not send email");
        }

        return $"The email was sent to the account {request.Email}";
    }
}