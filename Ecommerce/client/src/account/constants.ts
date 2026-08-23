export class Constants {
  public static apiRoot = "https://localhost:9000";
  public static get clientRoot(): string {
    return typeof window !== 'undefined' ? window.location.origin : "http://localhost:4200";
  }
  public static idpAuthority = "http://localhost:9011";
  public static clientId = "angular-client";
}