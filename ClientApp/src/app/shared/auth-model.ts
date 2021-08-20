export class AuthModel {

  constructor() {
    this.client_id = "console";
    this.client_secret = "388D45FA-B36B-4988-BA59-B187D329C207";
    this.scope = "openid api";
    this.grant_type = "client_credentials";
  }

  grant_type: string;
  scope: string;
  client_id: string;
  client_secret: string;
  username: string;
  password: string;
}
