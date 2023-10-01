// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Pages.Login;

public class InputModel
{
  public string Username { get; set; }

  public string Password { get; set; }

  public string PasskeyUsername { get; set; }

  public bool RememberLogin { get; set; }

  public string ReturnUrl { get; set; }

  public string Button { get; set; }
}