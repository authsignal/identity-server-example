function addPasskey() {
  var client = new window.authsignal.Authsignal({
    tenantId: "YOUR_TENANT_ID",
    baseUrl: "https://api.authsignal.com/v1", // Update for your region
  });

  var token = document.getElementById("enrollmentToken").value;

  client.passkey.signUp({ token }).then((resultToken) => {
    if (resultToken) {
      alert("Passkey added");
    }
  });
}
