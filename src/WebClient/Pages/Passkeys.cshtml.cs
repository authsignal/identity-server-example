using Authsignal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebClient.Pages;

public class PasskeysModel : PageModel
{
  private readonly IAuthsignalClient _authsignal;

  public string? enrollmentToken { get; set; }

  public PasskeysModel(IAuthsignalClient authsignal)
  {
    _authsignal = authsignal;
  }

  public async Task<IActionResult> OnGet()
  {
    var userId = User.Claims.First(x => x.Type == "sub").Value!;
    var action = "addPasskey";
    var scope = "add:authenticators";

    var trackRequest = new TrackRequest(
      UserId: userId,
      Action: action,
      Attributes: new TrackAttributes(Scope: scope)
    );

    var trackResponse = await _authsignal.Track(trackRequest);

    this.enrollmentToken = trackResponse.Token;

    return Page();
  }
}
