function initPasskeyAutofill() {
  var client = new window.authsignal.Authsignal({
    tenantId: "YOUR_TENANT_ID",
    baseUrl: "https://api.authsignal.com/v1", // Update for your region
  });

  client.passkey.signIn({ autofill: true }).then((token) => {
    if (token) {
      var returnUrl = document.getElementById("Input_ReturnUrl").value;
      var encodedReturnUrl = btoa(returnUrl);

      window.location = `https://localhost:5001/Account/Login/Callback?returnUrl=${encodedReturnUrl}&token=${token}`;
    }
  });
}

if (document.readyState === "loading") {
  document.addEventListener("DOMContentLoaded", initPasskeyAutofill);
} else {
  initPasskeyAutofill();
}
