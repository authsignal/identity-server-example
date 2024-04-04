using System.Text;
using Authsignal;
using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace IdentityServer.Pages.Login;

[SecurityHeaders]
[AllowAnonymous]
public class Callback : PageModel
{
  private readonly TestUserStore _users;
  private readonly IIdentityServerInteractionService _interaction;
  private readonly IEventService _events;
  private readonly IAuthsignalClient _authsignal;

  public Callback(
      IIdentityServerInteractionService interaction,
      IEventService events,
      IAuthsignalClient authsignal,
      TestUserStore users = null)
  {
    // this is where you would plug in your own custom identity management library (e.g. ASP.NET Identity)
    _users = users ?? throw new Exception("Please call 'AddTestUsers(TestUsers.Users)' on the IIdentityServerBuilder in Startup or remove the TestUserStore from the AccountController.");

    _interaction = interaction;
    _events = events;
    _authsignal = authsignal;
  }

  public async Task<IActionResult> OnGet(string returnUrl, string token)
  {
    var decodedReturnUrl = Encoding.UTF8.GetString(Convert.FromBase64String(returnUrl));

    if (token == null)
    {
      return Redirect("https://localhost:5001/Account/Login?ReturnUrl=" + decodedReturnUrl);
    }

    var context = await _interaction.GetAuthorizationContextAsync(decodedReturnUrl);

    var validateChallengeRequest = new ValidateChallengeRequest(token);

    var validateChallengeResponse = await _authsignal.ValidateChallenge(validateChallengeRequest);

    var userId = validateChallengeResponse.UserId;

    var user = _users.FindBySubjectId(userId);

    if (validateChallengeResponse.State != UserActionState.CHALLENGE_SUCCEEDED)
    {
      return Redirect("https://localhost:5001/Account/Login?ReturnUrl=" + decodedReturnUrl);
    }

    await _events.RaiseAsync(new UserLoginSuccessEvent(user.Username, user.SubjectId, user.Username, clientId: context?.Client.ClientId));

    // issue authentication cookie with subject ID and username
    var isuser = new IdentityServerUser(user.SubjectId)
    {
      DisplayName = user.Username
    };

    await HttpContext.SignInAsync(isuser);

    if (context != null)
    {
      return Redirect(decodedReturnUrl);
    }

    return Page();
  }
}