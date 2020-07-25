import Oidc from 'oidc-client'

export class OpenIdConnectService {
  private readonly config = {
    authority: "https://localhost:5001",
    client_id: "js",
    redirect_uri: "http://localhost:8080/login-callback",
    response_type: "code",
    scope:"openid profile test",
    post_logout_redirect_uri : "http://localhost:8080",
  };

  private readonly mgr: Oidc.UserManager;

  constructor () {
    this.mgr = new Oidc.UserManager(this.config)
    this.mgr.getUser().then((user) => console.log(user));
  }

  login () {
    this.mgr.signinRedirect()
  }

  logout () {
    this.mgr.signoutRedirect()
  }

  async api () {
    const user = await this.mgr.getUser();
    console.log(user);
    const url = 'https://localhost:6001/identity'

    const response = await fetch(url, {
      method: 'get',
      headers: new Headers({
        Authorization: `Bearer ${user?.access_token}`
      })
    })

    console.log(this.mgr);
    const data = await response.json()
    console.log(data)
  }
}
