export class Constants {
  public static apiRoot = "https://localhost:9000";
  public static get clientRoot(): string {
    return typeof window !== 'undefined' ? window.location.origin : "http://localhost:4200";
  }
  public static idpAuthority = "https://tired-queens-battle.loca.lt";
  public static clientId = "angular-client";
}